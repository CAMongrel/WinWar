using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
	internal class BasicResource
	{
      public ContentFileType Type { get; protected set; }

		internal WarResource Resource;

		protected BasicResource()
		{
         Type = ContentFileType.FileUnknown;
			this.Resource = null;
		}

      internal BasicResource(WarResource setData)
      {
         this.Resource = setData;
      }

      protected ushort ReadUShort(int index)
      {
         return (ushort)(Resource.data[index + 0] + (Resource.data[index + 1] << 8));
      }

      protected int ReadInt(int index)
      {
         return Resource.data[index + 0] + (Resource.data[index + 1] << 8) + (Resource.data[index + 2] << 16) + (Resource.data[index + 3] << 24);
      }
	}
}
