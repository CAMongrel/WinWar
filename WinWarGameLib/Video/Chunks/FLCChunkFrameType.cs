using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib.Chunks
{
    class FLCChunkFrameType : FLCChunk
    {
        public ushort Chunks { get; private set; }          // Number of subchunks
        public ushort Delay { get; private set; }           // Delay in milliseconds
        public short Reserved { get; private set; }         // Always zero
        public ushort Width { get; private set; }           // Frame width override (if non-zero)
        public ushort Height { get; private set; }          // Frame height override (if non-zero)

        public FLCChunkFrameType(FLCFile setOwnerFile)
            : base(setOwnerFile)
        {
        }

        protected override void ReadFromStream(System.IO.BinaryReader reader)
        {
            base.ReadFromStream(reader);

            Chunks = reader.ReadUInt16();
            Delay = reader.ReadUInt16();
            Reserved = reader.ReadInt16();
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();

            if (Width != 0 || Height != 0)
                throw new NotImplementedException("Overlay width or height is not supported yet.");

            ReadSubChunks(reader, Chunks);
        }
    }
}
