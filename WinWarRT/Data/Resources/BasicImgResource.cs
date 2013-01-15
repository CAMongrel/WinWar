using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WinWarRT.Data.Resources
{
	public abstract class BasicImgResource : BasicResource
	{
		public WarResource palette;

		public ushort width;
		public ushort height;
		public byte[] image_data;

		protected BasicImgResource(WarResource palette, WarResource data)
		{
			this.data = data;
			this.palette = palette;

			image_data = null;
		}

		public virtual void CreateImageData(bool bForceGrayscale)
		{
			//
		}

		public void SaveToFile(string filename)
		{
            throw new NotImplementedException();
			//SaveToFile(filename, ImageFormat.Bmp);
		}

		public virtual void SaveToFile(string filename, string format)
		{
            throw new NotImplementedException();
			/*Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			BitmapData bd = bm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			unsafe
			{
				byte* b_ptr = (byte*)bd.Scan0.ToPointer();

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						*b_ptr++ = image_data[(x * 4) + (y * width * 4) + 0];
						*b_ptr++ = image_data[(x * 4) + (y * width * 4) + 1];
						*b_ptr++ = image_data[(x * 4) + (y * width * 4) + 2];
						*b_ptr++ = image_data[(x * 4) + (y * width * 4) + 3];
					}
				}
			}
			bm.UnlockBits(bd);

			bm.Save(filename, format);
			bm.Dispose();*/
		}
	}
}
