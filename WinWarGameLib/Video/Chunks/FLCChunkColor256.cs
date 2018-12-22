using FLCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib.Chunks
{
    class FLCChunkColor256 : FLCChunk
    {
        public FLCColor[] Colors;

        public FLCChunkColor256(FLCFile setOwnerFile)
            : base(setOwnerFile)
        {
            Colors = new FLCColor[256];
        }

        protected override void ReadFromStream(System.IO.BinaryReader reader)
        {
            base.ReadFromStream(reader);

            ushort nrPackets = reader.ReadUInt16();
            for (int i = 0; i < nrPackets; i++)
            {
                byte skipCount = reader.ReadByte();
                byte copyCount = reader.ReadByte();

                if (copyCount == 0)
                {
                    byte[] rgbData = reader.ReadBytes(256 * 3);
                    for (int j = 0; j < 256; j++)
                    {
                        Colors[j] = new FLCColor(rgbData[j * 3 + 0], rgbData[j * 3 + 1], rgbData[j * 3 + 2], 255);
                    }
                }
                else
                {
                }
            }
        }
    }
}
