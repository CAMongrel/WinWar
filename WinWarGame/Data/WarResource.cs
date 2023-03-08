using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WinWarGame.Data
{
   public class WarResource
   {
      bool bCompressed;
      long offset;
      int compr_length;
      int length;
      internal byte[] data;
      internal int resource_index;

      internal WarResource(BinaryReader br, long offset, int compr_length, int resource_index)
      {
         this.resource_index = resource_index;
         this.compr_length = compr_length;
         this.offset = offset;
         this.length = 0;

         ReadHeader(br);

         ReadData(br);
      }

      private void ReadHeader(BinaryReader br)
      {
         br.BaseStream.Seek(offset, SeekOrigin.Begin);
         ushort len = br.ReadUInt16();
         byte align = br.ReadByte();

         length = len + (align << 16);

         byte comprFlag = br.ReadByte();
         bCompressed = (comprFlag != 0);
      }

      internal static byte[] RLEUncompress(byte[] inputData)
      {
         int length = inputData[0];

         BinaryReader reader = new BinaryReader(new MemoryStream(inputData, 1, inputData.Length - 1));

         int i, j, br, b1pos, b2pos;
         int b1, b2, _bytes_read, cnt;
         BitMask bm;

         byte[] _compr_data = new byte[4096];
         byte[] _out = new byte[4096];

         _bytes_read = reader.Read(_compr_data, 0, 4096);

         br = 0;
         b1pos = 0;
         b2pos = 0;

         int nbrBytes = 0;
         byte[] localData = null;
         List<byte> outData = new List<byte>();

         cnt = 0;

         int compr_length = inputData.Length - 1;

         while (br <= compr_length)
         {
            bm = (BitMask)_compr_data[b1pos];
            b1pos++;
            if (b1pos >= 4096)
            {
               _bytes_read = reader.Read(_compr_data, 0, 4096);
               b1pos = 0;
            }
            br++;

            for (i = 0; i < 8; i++)
            {
               if (bm.bits[i])      // uncompressed
               {
                  _out[b2pos] = _compr_data[b1pos];
                  b2pos++;
                  if (b2pos >= 4096)
                  {
                     nbrBytes = ((length - cnt > 4096) ? _bytes_read : length - cnt);
                     localData = new byte[nbrBytes];
                     Array.Copy(_out, 0, localData, 0, nbrBytes);
                     outData.InsertRange(cnt, localData);

                     cnt += ((length - cnt > 4096) ? _bytes_read : length - cnt);
                     b2pos = 0;
                  }
                  b1pos++;
                  if (b1pos >= 4096)
                  {
                     _bytes_read = reader.Read(_compr_data, 0, 4096);
                     b1pos = 0;
                  }
                  br++;
               }
               else
               {                    // compressed
                  b1 = _compr_data[b1pos];
                  b1pos++;
                  if (b1pos >= 4096)
                  {
                     _bytes_read = reader.Read(_compr_data, 0, 4096);
                     b1pos = 0;
                  }
                  b2 = _compr_data[b1pos];
                  b1pos++;
                  if (b1pos >= 4096)
                  {
                     _bytes_read = reader.Read(_compr_data, 0, 4096);
                     b1pos = 0;
                  }
                  br = br + 2;

                  b1 = (((b2 & 0x0F) << 8) | b1);
                  b2 = ((b2 & 0xF0) >> 4) + 3 + b1;

                  for (j = b1; j < b2; j++)
                  {
                     _out[b2pos] = _out[j % 4096];
                     b2pos++;
                     if (b2pos >= 4096)
                     {
                        nbrBytes = ((length - cnt > 4096) ? _bytes_read : length - cnt);
                        localData = new byte[nbrBytes];
                        Array.Copy(_out, 0, localData, 0, nbrBytes);
                        outData.InsertRange(cnt, localData);

                        cnt += ((length - cnt > 4096) ? _bytes_read : length - cnt);
                        b2pos = 0;
                     }
                  }
               }
            }
         }

         nbrBytes = ((length - cnt > 4096) ? _bytes_read : length - cnt);
         localData = new byte[nbrBytes];
         Array.Copy(_out, 0, localData, 0, nbrBytes);
         outData.InsertRange(cnt, localData);

         return outData.ToArray();
      }

      private void ReadData(BinaryReader read)
      {
         if (!bCompressed)
         {
            read.BaseStream.Seek(offset + 4, SeekOrigin.Begin);
            data = read.ReadBytes(length);
         }
         else
         {
            #region Decompress
            int i, j, br, b1pos, b2pos;
            int b1, b2, _bytes_read, cnt;
            BitMask bm;
            byte[] _compr_data;
            byte[] _out;

            if ((!bCompressed) || (length == 0))
               return;

            data = new byte[length];

            _compr_data = new byte[4096];
            _out = new byte[4096];

            read.BaseStream.Seek(offset + 4, SeekOrigin.Begin);

            _bytes_read = read.Read(_compr_data, 0, 4096);

            br = 0;
            b1pos = 0;
            b2pos = 0;

            cnt = 0;

            while (br <= compr_length)
            {
               bm = (BitMask)_compr_data[b1pos];
               b1pos++;
               if (b1pos >= 4096)
               {
                  _bytes_read = read.Read(_compr_data, 0, 4096);
                  b1pos = 0;
               }
               br++;

               for (i = 0; i < 8; i++)
               {
                  if (bm.bits[i])		// uncompressed
                  {
                     _out[b2pos] = _compr_data[b1pos];
                     b2pos++;
                     if (b2pos >= 4096)
                     {
                        Array.Copy(_out, 0, data, cnt, ((length - cnt > 4096) ? _bytes_read : length - cnt));
                        cnt += ((length - cnt > 4096) ? _bytes_read : length - cnt);
                        b2pos = 0;
                     }
                     b1pos++;
                     if (b1pos >= 4096)
                     {
                        _bytes_read = read.Read(_compr_data, 0, 4096);
                        b1pos = 0;
                     }
                     br++;
                  }
                  else
                  {							// compressed
                     b1 = _compr_data[b1pos];
                     b1pos++;
                     if (b1pos >= 4096)
                     {
                        _bytes_read = read.Read(_compr_data, 0, 4096);
                        b1pos = 0;
                     }
                     b2 = _compr_data[b1pos];
                     b1pos++;
                     if (b1pos >= 4096)
                     {
                        _bytes_read = read.Read(_compr_data, 0, 4096);
                        b1pos = 0;
                     }
                     br = br + 2;

                     b1 = (((b2 & 0x0F) << 8) | b1);
                     b2 = ((b2 & 0xF0) >> 4) + 3 + b1;

                     for (j = b1; j < b2; j++)
                     {
                        _out[b2pos] = _out[j % 4096];
                        b2pos++;
                        if (b2pos >= 4096)
                        {
                           Array.Copy(_out, 0, data, cnt, ((length - cnt > 4096) ? _bytes_read : length - cnt));
                           cnt += ((length - cnt > 4096) ? _bytes_read : length - cnt);
                           b2pos = 0;
                        }
                     }
                  }
               }
            }

            Array.Copy(_out, 0, data, cnt, ((length - cnt > 4096) ? _bytes_read : length - cnt));
            #endregion
         }
      }
   }

   struct BitMask
   {
      internal bool[] bits;

      public static explicit operator BitMask(byte b)
      {
         BitMask bm = new BitMask();
         bm.bits = new bool [8];
         bm.bits[7] = (b & 128) > 0;
         bm.bits[6] = (b & 64) > 0;
         bm.bits[5] = (b & 32) > 0;
         bm.bits[4] = (b & 16) > 0;
         bm.bits[3] = (b & 8) > 0;
         bm.bits[2] = (b & 4) > 0;
         bm.bits[1] = (b & 2) > 0;
         bm.bits[0] = (b & 1) > 0;
         return bm;
      }
   }
}
