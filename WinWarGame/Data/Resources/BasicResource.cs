using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WinWarGame.Data.Resources
{
   public abstract class BasicResource
   {
      private WarResource originalResource;

      public WarResource Resource => originalResource;
      
      public ContentFileType Type { get; protected set; }

      protected BasicResource(WarResource setResource)
      {
         originalResource = setResource;
         Type = ContentFileType.FileUnknown;
      }

      protected byte[] ReadBytes(int index, int count, byte[] data)
      {
         if (count < 0)
         {
            return Array.Empty<byte>();
         }
         
         int diff = data.Length - (index + count);
         if (diff < 0)
         {
            count += diff;
         }

         byte[] result = new byte[count];
         Array.Copy(data, index, result, 0, count);
         return result;
      }

      protected short ReadShort (int index, byte [] data)
      {
         if (index < 0 || index >= data.Length)
         {
            return 0;
         }

         return (short)(data [index + 0] + (data[index + 1] << 8));
      }

      protected ushort ReadUShort(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
         {
            return 0;
         }

         return (ushort)(data[index + 0] + (data[index + 1] << 8));
      }

      protected uint ReadUInt(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
         {
            return 0;
         }

         return (uint)(data[index + 0] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24));
      }

      protected int ReadInt(int index, byte[] data)
      {
         if (index < 0 || index >= data.Length)
         {
            return 0;
         }

         return data[index + 0] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24);
      }

      protected ushort ReadResourceIndexDirectUShort(int offset, WarResource res)
      {
         ushort resIndex = ReadUShort(offset, res.data);
         if (resIndex == 0 || resIndex == 0xFFFF)
         {
            return 0;
         }

         return (ushort)(resIndex - 2);
      }

      protected int ReadResourceIndexDirectInt(int offset, WarResource res)
      {
         int resIndex = ReadInt(offset, res.data);
         if (resIndex == 0 || resIndex >= 0xFFFF)
         {
            return 0;
         }

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

      internal virtual void WriteToStream(System.IO.BinaryWriter writer)
      {
         writer.Write(originalResource.data);
      }
      
      internal virtual void WriteToFile(string filename)
      {
         using (var stream = File.OpenWrite(filename))
         {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
               WriteToStream(writer);
            }
         }
      }
   }
}
