using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WinWarCS.Data.Resources
{
   internal abstract class BasicResource
	{
      public ContentFileType Type { get; protected set; }

		protected BasicResource()
		{
         Type = ContentFileType.FileUnknown;
		}

      protected byte[] ReadBytes(int index, int count, byte[] data)
      {
         int diff = data.Length - (index + count);
         if (diff < 0)
            count += diff;
         if (count < 0)
            return new byte[0];

         byte[] result = new byte[count];
         Array.Copy(data, index, result, 0, count);
         return result;
      }

      protected ushort ReadUShort(int index, byte[] data)
      {
         return (ushort)(data[index + 0] + (data[index + 1] << 8));
      }

      protected uint ReadUInt(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
            return 0;

         return (uint)(data[index + 0] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24));
      }

      protected int ReadInt(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
            return 0;

         return data[index + 0] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24);
      }

      protected ushort ReadResourceIndexDirectUShort(int offset, WarResource res)
      {
         ushort resIndex = ReadUShort(offset, res.data);
         if (resIndex == 0 || resIndex == 0xFFFF)
            return 0;

         return (ushort)(resIndex - 2);
      }

      protected int ReadResourceIndexDirectInt(int offset, WarResource res)
      {
         int resIndex = ReadInt(offset, res.data);
         if (resIndex == 0 || resIndex >= 0xFFFF)
            return 0;

         return resIndex - 2;
      }

      protected string ReadStringDirect(int offset, WarResource res)
      {
         StringBuilder result = new StringBuilder();

         byte b = res.data[offset++];
         // Nullterminated string
         while (b != 0x00)
         {
            result.Append((char)b);
            b = res.data[offset++];
         }

         return result.ToString();
      }

#if !NETFX_CORE
      internal virtual void WriteToStream(BinaryWriter writer)
      {
         // Implemented in derived classes
      }

      internal virtual void WriteToFile(string filename)
      {
         // Implemented in derived classes
      }
#endif
	}
}
