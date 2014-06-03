using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarRT.Data.Resources
{
	internal abstract class BasicImgResource : BasicResource
	{
		internal WarResource palette;

		internal ushort width;
		internal ushort height;
		internal byte[] image_data;

		protected BasicImgResource(WarResource palette, WarResource data)
		{
			this.data = data;
			this.palette = palette;

			image_data = null;
		}

		internal abstract void CreateImageData(bool bForceGrayscale);
	}
}
