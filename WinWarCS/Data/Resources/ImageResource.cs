using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
	internal class ImageResource : BasicResource
	{
      internal ushort width;
      internal ushort height;
      internal byte[] image_data;

      internal ImageResource(WarResource imgResource, WarResource palette, WarResource addPalette)
		{
         Type = ContentFileType.FileImage;

         CreateImageData(imgResource, palette, addPalette);
		}

      internal void CreateImageData(WarResource imgResource, WarResource palette, WarResource addPalette)
		{
         bool bForceGrayscale = palette == null;

         WarResource addPal = addPalette;

			unsafe
			{
            fixed (byte* org_ptr = &imgResource.data[0])
				{
					byte* b_ptr = org_ptr;

               width = *(ushort*)b_ptr;
               b_ptr += 2;
               height = *(ushort*)b_ptr;
               b_ptr += 2;
					image_data = new byte[width * height * 4];

					int cnt = 0;

					int x, y;

					if ((palette == null) || (bForceGrayscale))	// No palette for this image or grayscale forced ... use grayscale palette
					{
						for (y = 0; y < height; y++)
							for (x = 0; x < width; x++)
							{
								image_data[cnt] = b_ptr[x + y * width];
								cnt++;
								image_data[cnt] = b_ptr[x + y * width];
								cnt++;
								image_data[cnt] = b_ptr[x + y * width];
								cnt++;
								image_data[cnt] = 255;
								cnt++;
							}
					}
					else
					{
						// We have a palette ... use it!
						fixed (byte* pal_org_ptr = &palette.data[0])
						{
							byte* pal_dataptr = pal_org_ptr;
							int pal_index;

							for (y = 0; y < height; y++)
							{
								for (x = 0; x < width; x++)
								{
									pal_index = b_ptr[x + y * width] * 3;

									image_data[cnt] = (byte)(pal_dataptr[pal_index + 0] * 4);
									cnt++;
									image_data[cnt] = (byte)(pal_dataptr[pal_index + 1] * 4);
									cnt++;
									image_data[cnt] = (byte)(pal_dataptr[pal_index + 2] * 4);
									cnt++;
                           image_data[cnt] = 255;// (byte)(((image_data[cnt - 3] == 0) && (image_data[cnt - 2] == 0) && (image_data[cnt - 1] == 0)) ? 0 : 255);
									cnt++;

									if ((image_data[cnt - 4] == 228) &&
										 (image_data[cnt - 3] == 108) &&
										 (image_data[cnt - 2] == 228))
									{
                              image_data[cnt - 4] = (byte)(addPal.data[pal_index] * 4);
                              image_data[cnt - 3] = (byte)(addPal.data[pal_index + 1] * 4);
                              image_data[cnt - 2] = (byte)(addPal.data[pal_index + 2] * 4);
									}
								}
							}
						}
					}
				}
			}
		}

#if !NETFX_CORE
      internal override void WriteToStream(System.IO.BinaryWriter writer)
      {
         base.WriteToStream(writer);
      }

      internal override void WriteToFile(string filename)
      {
         base.WriteToFile(filename);

         System.Drawing.Bitmap bm = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
         // Yes, this is very slow ... but I don't care, this is only for testing
         for (int y = 0; y < height; y++)
         {
            for (int x = 0; x < width; x++)
            {
               int index = (x + (y * width)) * 4;
               System.Drawing.Color col = System.Drawing.Color.FromArgb(image_data[index + 3], image_data[index + 0], image_data[index + 1], image_data[index + 2]);

               bm.SetPixel(x, y, col);
            }
         }
         bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
      }
#endif
	}
}
