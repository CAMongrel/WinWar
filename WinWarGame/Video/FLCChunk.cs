using FLCLib.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    abstract class FLCChunk
    {
        public uint Size { get; private set; }
        public ChunkType Type { get; private set; }

        public List<FLCChunk> SubChunks { get; private set; }

        public FLCFile OwnerFile { get; private set; }

        public FLCChunk(FLCFile setOwnerFile)
        {
            OwnerFile = setOwnerFile;
            SubChunks = new List<FLCChunk>();
        }

        protected virtual void ReadFromStream(BinaryReader reader)
        {
        }

        protected void ReadSubChunks(BinaryReader reader, int count)
        {
            for (int i = 0; i < count; i++)
            {
                FLCChunk chunk = FLCChunk.CreateFromStream(reader, OwnerFile);
                SubChunks.Add(chunk);
            }
        }

        public static FLCChunk CreateFromStream(BinaryReader reader, FLCFile file)
        {
            if (reader == null)
            {
                return null;
            }

            uint size = reader.ReadUInt32();
            ChunkType type = (ChunkType)reader.ReadUInt16();

            FLCChunk result = null;
            switch (type)
            {
                case ChunkType.FRAME_TYPE:
                    result = new FLCChunkFrameType(file);
                    break;

                case ChunkType.COLOR_256:
                    result = new FLCChunkColor256(file);
                    break;

                case ChunkType.BYTE_RUN:
                    result = new FLCChunkByteRun(file);
                    break;

                case ChunkType.FLI_COPY:
                    result = new FLCChunkFLICopy(file);
                    break;

                case ChunkType.DELTA_FLC:
                    result = new FLCChunkDeltaFLC(file);
                    break;

                case ChunkType.DELTA_FLI:
                    result = new FLCChunkDeltaFLI(file);
                    break;

                default:
                    result = new FLCChunkUnknown(file);
                    reader.BaseStream.Seek(size - 6, SeekOrigin.Current);
                    break;
            }

            if (result != null)
            {
                result.Size = size;
                result.Type = type;

                result.ReadFromStream(reader);
            }

            return result;
        }

        public FLCChunk GetChunkByType(ChunkType type)
        {
            for (int i = 0; i < SubChunks.Count; i++)
            {
                if (SubChunks[i].Type == type)
                    return SubChunks[i];
            }

            return null;
        }
    }
}
