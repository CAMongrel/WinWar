using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
   internal abstract class BasicResource
	{
      public ContentFileType Type { get; protected set; }

		protected BasicResource()
		{
         Type = ContentFileType.FileUnknown;
		}

      protected ushort ReadUShort(int index, byte[] data)
      {
         return (ushort)(data[index + 0] + (data[index + 1] << 8));
      }

      protected int ReadInt(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
            return 0;

         return data[index + 0] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24);
      }
	}
}
