using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib.Chunks
{
    class FLCChunkDeltaFLI : FLCChunk
    {
        public byte[] Payload;

        public FLCChunkDeltaFLI(FLCFile setOwnerFile)
            : base(setOwnerFile)
        {
        }

        protected override void ReadFromStream(System.IO.BinaryReader reader)
        {
            base.ReadFromStream(reader);

            Payload = reader.ReadBytes((int)Size - 6);
        }
    }
}
