using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    class FLCHeader
    {
        public int size;          /* Size of FLIC including this header */
        public short type;          /* File type 0xAF11, 0xAF12, 0xAF30, 0xAF44, ... */
        public short frames;        /* Number of frames in first segment */
        public short width;         /* FLIC width in pixels */
        public short height;        /* FLIC height in pixels */
        public short depth;         /* Bits per pixel (usually 8) */
        public short flags;         /* Set to zero or to three */
        public int speed;         /* Delay between frames */
        public short reserved1;     /* Set to zero */
        public int created;       /* Date of FLIC creation (FLC only) */
        public int creator;       /* Serial number or compiler id (FLC only) */
        public int updated;       /* Date of FLIC update (FLC only) */
        public int updater;       /* Serial number (FLC only), see creator */
        public short aspect_dx;     /* Width of square rectangle (FLC only) */
        public short aspect_dy;     /* Height of square rectangle (FLC only) */
        public short ext_flags;     /* EGI: flags for specific EGI extensions */
        public short keyframes;     /* EGI: key-image frequency */
        public short totalframes;   /* EGI: total number of frames (segments) */
        public int req_memory;    /* EGI: maximum chunk size (uncompressed) */
        public short max_regions;   /* EGI: max. number of regions in a CHK_REGION chunk */
        public short transp_num;    /* EGI: number of transparent levels */
        public byte[] reserved2; /* Set to zero */
        public int oframe1;       /* Offset to frame 1 (FLC only) */
        public int oframe2;       /* Offset to frame 2 (FLC only) */
        public byte[] reserved3; /* Set to zero */

        public FLCHeader()
        {
            reserved2 = new byte[24];
            reserved3 = new byte[40];
        }

        public static FLCHeader ReadFromStream(BinaryReader reader)
        {
            FLCHeader result = new FLCHeader();
            result.size = reader.ReadInt32();
            result.type = reader.ReadInt16();
            result.frames = reader.ReadInt16();
            result.width = reader.ReadInt16();
            result.height = reader.ReadInt16();
            result.depth = reader.ReadInt16();
            result.flags = reader.ReadInt16();
            result.speed = reader.ReadInt32();
            result.reserved1 = reader.ReadInt16();
            result.created = reader.ReadInt32();
            result.creator = reader.ReadInt32();
            result.updated = reader.ReadInt32();
            result.updater = reader.ReadInt32();
            result.aspect_dx = reader.ReadInt16();
            result.aspect_dy = reader.ReadInt16();
            result.ext_flags = reader.ReadInt16();
            result.keyframes = reader.ReadInt16();
            result.totalframes = reader.ReadInt16();
            result.req_memory = reader.ReadInt32();
            result.max_regions = reader.ReadInt16();
            result.transp_num = reader.ReadInt16();
            result.reserved2 = reader.ReadBytes(24);
            result.oframe1 = reader.ReadInt32();
            result.oframe2 = reader.ReadInt32();
            result.reserved3 = reader.ReadBytes(40);
            return result;
        }
    }
}
