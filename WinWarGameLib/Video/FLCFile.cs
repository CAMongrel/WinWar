using FLCLib.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FLCLib
{
    public delegate void FrameUpdated(FLCFile file);
    public delegate void PlaybackStarted(FLCFile file);
    public delegate void PlaybackFinished(FLCFile file, bool didFinishNormally);

    public class FLCFile : IDisposable
    {
        public event FrameUpdated OnFrameUpdated;
        public event PlaybackStarted OnPlaybackStarted;
        public event PlaybackFinished OnPlaybackFinished;

        public bool IsPaused { get; private set; }

        public bool IsPlaying { get; private set; }

        public bool ShouldLoop { get; set; }

        public bool PauseAfterFirstFrame { get; set; }

        public int Width
        {
            get
            {
                return header.width;
            }
        }

        public int Height
        {
            get
            {
                return header.height;
            }
        }

        private FLCHeader header;
        private Stream stream;
        private BinaryReader reader;
        private int frameCounter;
        private FLCChunkColor256 currentPalette;
        private FLCFrameBuffer currentFrame;

        public FLCFile(Stream setStream)
        {
            stream = setStream;
            header = null;

            ShouldLoop = true;
            IsPlaying = false;
            IsPaused = false;
            PauseAfterFirstFrame = false;

            currentFrame = null;
            currentPalette = null;
        }

        public bool Open()
        {
            reader = new BinaryReader(stream);
            header = FLCHeader.ReadFromStream(reader);

            if (header.type != unchecked((short)0xAF12))
                throw new Exception("Can only open FLC videos (Type 0xAF12)");

            currentFrame = new FLCFrameBuffer(this);

            return true;
        }

        public FLCColor[] GetFramebufferCopy()
        {
            return currentFrame.GetFramebufferCopy();
        }

        private FLCChunk ReadNextChunk()
        {
            FLCChunk frm = FLCChunk.CreateFromStream(reader, this);
            if (frm == null)
            {
                return null;
            }

            FLCChunkColor256 newPalette = frm.GetChunkByType(ChunkType.COLOR_256) as FLCChunkColor256;
            if (newPalette != null)
            {
                currentPalette = newPalette;
            }

            currentFrame.UpdateFromFLCChunk(frm, currentPalette);

            OnFrameUpdated?.Invoke(this);

            return frm;
        }

        private void PlayInternal()
        {
            OnPlaybackStarted?.Invoke(this);

            while (IsPlaying)
            {
                if (this.IsPaused)
                {
                    //Task.Delay (8).Wait ();
                    Thread.Sleep(8);
                }
                else
                {
                    FLCChunk frm = ReadNextChunk();
                    frameCounter++;

                    //Task.Delay(header.speed).Wait();
                    Thread.Sleep(header.speed);

                    if (!ShouldLoop)
                    {
                        if (frameCounter >= header.frames)
                        {
                            InternalStop(true);
                        }
                    }
                    else
                    {
                        if (frameCounter > header.frames)
                        {
                            frameCounter = 1;

                            if (reader == null)
                            {
                                InternalStop(true);
                                break;
                            }

                            reader.BaseStream.Seek(header.oframe2, SeekOrigin.Begin);
                        }
                    }
                }
            }
        }

        public void Play()
        {
            if (this.IsPlaying && this.IsPaused)
            {
                this.IsPaused = false;
            }
            else
            {
                if (header == null || reader == null)
                    throw new Exception("File has not been opened successfully. Did you call Open()?");

                IsPlaying = true;

                // Read the first frame
                reader.BaseStream.Seek(header.oframe1, SeekOrigin.Begin);
                ReadNextChunk();

                if (this.PauseAfterFirstFrame)
                    this.IsPaused = true;

                frameCounter = 1;
                reader.BaseStream.Seek(header.oframe2, SeekOrigin.Begin);

                Task t = new Task(() =>
                {
                   PlayInternal();
                });
                t.Start();
            }
        }

        public void Pause()
        {
            this.IsPaused = !this.IsPaused;
        }

        public void Stop()
        {
            InternalStop(false);
        }

        private void InternalStop(bool didFinishNormally)
        {
            IsPlaying = false;

            OnPlaybackFinished?.Invoke(this, didFinishNormally);
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose called on " + stream?.ToString());
            if (reader != null)
            {
                reader.BaseStream.Dispose();
                reader.Dispose();
                reader = null;
            }
        }
    }
}
