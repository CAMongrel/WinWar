using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib.Chunks
{
    class FLCChunkFLICopy : FLCChunk
    {
        public byte[] PixelData;

        public FLCChunkFLICopy(FLCFile setOwnerFile)
            : base(setOwnerFile)
        {
        }

        protected override void ReadFromStream(System.IO.BinaryReader reader)
        {
            base.ReadFromStream(reader);

            PixelData = reader.ReadBytes(OwnerFile.Width * OwnerFile.Height);
        }
    }
}
