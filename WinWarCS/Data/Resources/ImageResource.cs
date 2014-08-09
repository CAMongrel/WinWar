using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
	internal class ImageResource : BasicImgResource
	{
      internal ImageResource(WarResource data, WarResource palette, WarResource addPalette)
			: base(palette, data)
		{
         Type = ContentFileType.FileImage;

         width = ReadUShort(0);
         height = ReadUShort(2);

         CreateImageData(palette == null, addPalette);
		}

      internal void CreateImageData(bool bForceGrayscale, WarResource addPalette)
		{
         WarResource addPal = addPalette;

			unsafe
			{
				fixed (byte* org_ptr = &Resource.data[0])
				{
					byte* b_ptr = org_ptr + 4;

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

                           if (this.Resource.resource_index == 280)
                              Console.Write ((pal_index / 3).ToString(" 000"));

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
                        if (this.Resource.resource_index == 280)
                           Console.WriteLine ();
							}
						}
					}
				}
			}
		}
	}
}
