using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarRT.Data.Resources
{
	public class ImageResource : BasicImgResource
	{
		public ImageResource(WarResource data, WarResource palette)
			: this(data, palette, false)
		{
		}

		public ImageResource(WarResource data, WarResource palette, bool bForceGrayscale)
			: base(palette, data)
		{
			unsafe
			{
				fixed (byte* org_ptr = &data.data[0])
				{
					ushort* usptr = (ushort*)org_ptr;
					width = *usptr++;
					height = *usptr++;
				}
			}

			CreateImageData(bForceGrayscale);
		}

		public override void CreateImageData(bool bForceGrayscale)
		{
			base.CreateImageData(bForceGrayscale);

			unsafe
			{
				fixed (byte* org_ptr = &data.data[0])
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
										image_data[cnt - 4] = KnowledgeBase.hardcoded_pal[pal_index];
										image_data[cnt - 3] = KnowledgeBase.hardcoded_pal[pal_index + 1];
										image_data[cnt - 2] = KnowledgeBase.hardcoded_pal[pal_index + 2];
									}

									if ((image_data[cnt - 4] == 204) &&
										(image_data[cnt - 3] == 0) &&
										(image_data[cnt - 2] == 212))
									{
										image_data[cnt - 4] = KnowledgeBase.hardcoded_pal[pal_index];
										image_data[cnt - 3] = KnowledgeBase.hardcoded_pal[pal_index + 1];
										image_data[cnt - 2] = KnowledgeBase.hardcoded_pal[pal_index + 2];
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
