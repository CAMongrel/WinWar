using FLCLib.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    class FLCFrameBuffer
    {
        private FLCFile file;

        public FLCColor[] framebuffer;

        public FLCFrameBuffer(FLCFile setFile)
        {
            file = setFile;

            framebuffer = new FLCColor[setFile.Width * setFile.Height];
        }

        public FLCColor[] GetFramebufferCopy()
        {
            FLCColor[] result = new FLCColor[framebuffer.Length];
            Array.Copy(framebuffer, result, framebuffer.Length);
            return result;
        }

        public void UpdateFromFLCChunk(FLCChunk chunk, FLCChunkColor256 paletteChunk)
        {
            if (chunk.Type != ChunkType.FRAME_TYPE)
                return;

            for (int chnkIdx = 0; chnkIdx < chunk.SubChunks.Count; chnkIdx++)
            {
                switch (chunk.SubChunks[chnkIdx].Type)
                {
                    case ChunkType.BYTE_RUN:
                        {
                            FLCChunkByteRun brun = (FLCChunkByteRun)chunk.SubChunks[chnkIdx];
                            for (int i = 0; i < brun.PixelData.Length; i++)
                            {
                                framebuffer[i] = paletteChunk.Colors[brun.PixelData[i]];
                            }
                        }
                        break;

                    case ChunkType.FLI_COPY:
                        {
                            FLCChunkFLICopy flicpy = (FLCChunkFLICopy)chunk.SubChunks[chnkIdx];
                            for (int i = 0; i < flicpy.PixelData.Length; i++)
                            {
                                framebuffer[i] = paletteChunk.Colors[flicpy.PixelData[i]];
                            }
                        }
                        break;

                    case ChunkType.DELTA_FLI:
                        {
                            FLCChunkDeltaFLI delta = (FLCChunkDeltaFLI)chunk.SubChunks[chnkIdx];

                            using (BinaryReader reader = new BinaryReader(new MemoryStream(delta.Payload)))
                            {
                                ushort lineSkipCount = reader.ReadUInt16();
                                ushort lineCount = reader.ReadUInt16();

                                int curLine = lineSkipCount;

                                for (int lineIdx = 0; lineIdx < lineCount; lineIdx++)
                                {
                                    byte packetCount = reader.ReadByte();

                                    int curColumn = 0;

                                    for (int pcktIdx = 0; pcktIdx < packetCount; pcktIdx++)
                                    {
                                        byte colSkipCount = reader.ReadByte();
                                        sbyte rleByteCount = reader.ReadSByte();

                                        curColumn += colSkipCount;

                                        if (rleByteCount > 0)
                                        {
                                            for (int i = 0; i < rleByteCount; i++)
                                            {
                                                byte val = reader.ReadByte();

                                                framebuffer[(curColumn + curLine * file.Width)] = paletteChunk.Colors[val];
                                                curColumn++;
                                            }
                                        }
                                        else if (rleByteCount < 0)
                                        {
                                            byte cpyCount = (byte)-rleByteCount;
                                            byte val = reader.ReadByte();

                                            for (int i = 0; i < cpyCount; i++)
                                            {
                                                framebuffer[(curColumn + curLine * file.Width)] = paletteChunk.Colors[val];
                                                curColumn++;
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }

                                    curLine++;
                                }
                            }
                        }
                        break;

                    case ChunkType.DELTA_FLC:
                        {
                            FLCChunkDeltaFLC delta = (FLCChunkDeltaFLC)chunk.SubChunks[chnkIdx];

                            using (BinaryReader reader = new BinaryReader(new MemoryStream(delta.Payload)))
                            {
                                ushort lineCount = reader.ReadUInt16();

                                int curLine = 0;

                                for (int lineIdx = 0; lineIdx < lineCount; lineIdx++)
                                {
                                    ushort? lineSkipCount = null;
                                    byte? lastPixel = null;
                                    ushort? packetCount = null;

                                    while (packetCount == null)
                                    {
                                        short opcode = reader.ReadInt16();

                                        if ((opcode & (1 << 15)) > 0)
                                        {
                                            if ((opcode & (1 << 14)) > 0)
                                            {
                                                lineSkipCount = (ushort)-opcode;
                                            }
                                            else
                                            {
                                                lastPixel = (byte)(opcode & 0x00FF);
                                            }
                                        }
                                        else
                                        {
                                            if ((opcode & (1 << 14)) > 0)
                                            {
                                                //
                                            }
                                            else
                                            {
                                                packetCount = (ushort)opcode;
                                            }
                                        }
                                    }

                                    if (packetCount > 100)
                                        return;

                                    if (lineSkipCount != null)
                                        curLine += lineSkipCount.Value;

                                    int curColumn = 0;

                                    for (int pcktIdx = 0; pcktIdx < packetCount; pcktIdx++)
                                    {
                                        byte colSkipCount = reader.ReadByte();
                                        sbyte rleByteCount = reader.ReadSByte();

                                        curColumn += colSkipCount;

                                        if (rleByteCount > 0)
                                        {
                                            for (int i = 0; i < rleByteCount; i++)
                                            {
                                                ushort val = reader.ReadUInt16();
                                                byte val1 = (byte)(val >> 8);
                                                byte val2 = (byte)val;

                                                framebuffer[(curColumn + curLine * file.Width) + 0] = paletteChunk.Colors[val2];
                                                framebuffer[(curColumn + curLine * file.Width) + 1] = paletteChunk.Colors[val1];
                                                curColumn += 2;
                                            }
                                        }
                                        else if (rleByteCount < 0)
                                        {
                                            byte cpyCount = (byte)-rleByteCount;
                                            ushort val = reader.ReadUInt16();
                                            byte val1 = (byte)(val >> 8);
                                            byte val2 = (byte)val;

                                            for (int i = 0; i < cpyCount; i++)
                                            {
                                                framebuffer[(curColumn + curLine * file.Width) + 0] = paletteChunk.Colors[val2];
                                                framebuffer[(curColumn + curLine * file.Width) + 1] = paletteChunk.Colors[val1];
                                                curColumn += 2;
                                            }
                                        }
                                        else
                                        {                                            
                                        }
                                    }

                                    curLine++;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
