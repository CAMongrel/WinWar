using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib.Chunks
{
    // RLE encoded full frame
    class FLCChunkByteRun : FLCChunk
    {
        public byte[] PixelData;

        public FLCChunkByteRun(FLCFile setOwnerFile)
            : base(setOwnerFile)
        {
        }

        protected override void ReadFromStream(System.IO.BinaryReader reader)
        {
            base.ReadFromStream(reader);

            PixelData = new byte[OwnerFile.Width * OwnerFile.Height];

            int pixelDataPointer = 0;

            for (int line = 0; line < OwnerFile.Height; line++)
            {
                byte packetCnt = reader.ReadByte();
                // Ignore packet count and just uncompress

                int uncompressedWidth = 0;

                while (uncompressedWidth < OwnerFile.Width)
                {
                    sbyte controlByte = reader.ReadSByte();
                    if (controlByte < 0)
                    {
                        byte copyCount = (byte)-controlByte;
                        uncompressedWidth += copyCount;
                        for (int i = 0; i < copyCount; i++)
                        {
                            PixelData[pixelDataPointer++] = reader.ReadByte();
                        }
                    }
                    else
                    {
                        uncompressedWidth += controlByte;
                        byte dataByte = reader.ReadByte();
                        for (int i = 0; i < controlByte; i++)
                            PixelData[pixelDataPointer++] = dataByte;
                    }
                }
            }
        }
    }
}
