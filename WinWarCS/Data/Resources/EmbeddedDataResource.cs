using System;
using System.Collections.Generic;
using System.IO;

namespace WinWarCS.Data.Resources
{
   internal class EmbeddedDataResource : BasicResource
   {
      internal EmbeddedDataResource(WarResource data)
      {
         Init(data);
      }

      private void Init(WarResource res)
      {
         ushort nmbrEntries = ReadUShort(0, res.data);
         ushort unk = ReadUShort(2, res.data);

         int[] offsets = new int[nmbrEntries];
         for (int i = 0; i < nmbrEntries; i++)
         {
            unk = ReadUShort(4 + (i * 8) + 0, res.data);
            unk = ReadUShort(4 + (i * 8) + 2, res.data);
            offsets[i] = ReadInt(4 + (i * 8) + 4, res.data);
         }

         int offset = offsets[0];
         List<byte[]> datas = new List<byte[]>();
         for (int i = 0; i < nmbrEntries; i++)
         {
            int length = 0;
            if (i < nmbrEntries - 1)
               length = offsets[i + 1] - offsets[i];
            else
               length = res.data.Length - offsets[i];

            byte[] data = ReadBytes(offsets[i], length, res.data);
            datas.Add(WarResource.RLEUncompress(data));

            offset += length;
         }
      }
   }
}

