using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    internal enum ChunkType
    {
        CEL_DATA = 3,           // registration and transparency
        COLOR_256 = 4,          // 256-level colour palette
        DELTA_FLC = 7,          // delta image, word oriented RLE
        COLOR_64 = 11,          // 64-level colour palette
        DELTA_FLI = 12,         // delta image, byte oriented RLE
        BLACK = 13,             // full black frame (rare)
        BYTE_RUN = 15,          // full image, byte oriented RLE
        FLI_COPY = 16,          // uncompressed image (rare)
        PSTAMP = 18,            // postage stamp (icon of the first frame)
        DTA_BRUN = 25,          // full image, pixel oriented RLE
        DTA_COPY = 26,          // uncompressed image
        DTA_LC = 27,            // delta image, pixel oriented RLE
        LABEL = 31,             // frame label
        BMP_MASK = 32,          // bitmap mask
        MLEV_MASK = 33,         // multilevel mask
        SEGMENT = 34,           // segment information
        KEY_IMAGE = 35,         // key image, similar to BYTE_RUN / DTA_BRUN
        KEY_PAL = 36,           // key palette, similar to COLOR_256
        REGION = 37,            // region of frame differences
        WAVE = 38,              // digitized audio
        USERSTRING = 39,        // general purpose user data
        RGN_MASK = 40,          // region mask
        LABELEX = 41,           // extended frame label (includes symbolic name)
        SHIFT = 42,             // scanline delta shifts (compression)
        PATHMAP = 43,           // path map (segment transitions)

        PREFIX_TYPE = 0xF100,          // prefix chunk
        SCRIPT_CHUNK = 0xF1E0,         // embedded "Small" script
        FRAME_TYPE = 0xF1FA,           // frame chunk
        SEGMENT_TABLE = 0xF1FB,        // segment table chunk
        HUFFMAN_TABLE = 0xF1FC,        // Huffman compression table chunk
    }

    class FLCChunkHeader
    {
        public int size;        // Bytes in this chunk.
        public ChunkType type;      // Type of chunk (see below).
        public short subchunks;
        public int reserved1;
        public int reserved2;

        public const int SizeOf = 16;

        public static FLCChunkHeader ReadFromStream(BinaryReader reader)
        {
            FLCChunkHeader result = new FLCChunkHeader();
            result.size = reader.ReadInt32();
            result.type = (ChunkType)reader.ReadUInt16();
            result.subchunks = reader.ReadInt16();
            result.reserved1 = reader.ReadInt32();
            result.reserved2 = reader.ReadInt32();
            return result;
        }
    }
}
