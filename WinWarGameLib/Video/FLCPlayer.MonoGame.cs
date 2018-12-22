using FLCLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    public delegate void PlayerFrameUpdated(Texture2D texture, FLCFile file);

    public class FLCPlayer : IDisposable
    {
        private GraphicsDevice device;

        public event PlayerFrameUpdated OnFrameUpdated;
        public event PlaybackStarted OnPlaybackStarted;
        public event PlaybackFinished OnPlaybackFinished;

        public bool IsPaused
        {
            get
            {
                if (this.flcFile == null)
                    return false;
                else
                    return this.flcFile.IsPaused;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (flcFile == null)
                    return false;

                return flcFile.IsPlaying;
            }
        }

        public bool ShouldLoop
        {
            get
            {
                if (flcFile == null)
                    return false;

                return flcFile.ShouldLoop;
            }
            set
            {
                if (flcFile == null)
                    return;

                flcFile.ShouldLoop = value;
            }
        }

        public bool PauseAfterFirstFrame
        {
            get
            {
                if (this.flcFile == null)
                    return false;
                else
                    return this.flcFile.PauseAfterFirstFrame;
            }
            set
            {
                if (this.flcFile == null)
                    return;
                this.flcFile.PauseAfterFirstFrame = value;
            }
        }

        private Stream file;
        private FLCFile flcFile;
        private Texture2D currentFrame;

        public FLCPlayer(GraphicsDevice setDevice)
        {
            device = setDevice;

            file = null;
            flcFile = null;

            currentFrame = null;
        }

        public bool Open(Stream setFile)
        {
            file = setFile;

            if (flcFile != null)
            {
                flcFile.OnFrameUpdated -= flcFile_OnFrameUpdated;
                flcFile.OnPlaybackStarted -= flcFile_OnPlaybackStarted;
                flcFile.OnPlaybackFinished -= flcFile_OnPlaybackFinished;

                flcFile.Dispose();
                flcFile = null;
            }

            flcFile = new FLCFile(file);
            flcFile.OnFrameUpdated += flcFile_OnFrameUpdated;
            flcFile.OnPlaybackStarted += flcFile_OnPlaybackStarted;
            flcFile.OnPlaybackFinished += flcFile_OnPlaybackFinished;
            flcFile.Open();

            return true;
        }

        void flcFile_OnPlaybackFinished(FLCFile file, bool didFinishNormally)
        {
            OnPlaybackFinished?.Invoke(file, didFinishNormally);
        }

        void flcFile_OnPlaybackStarted(FLCFile file)
        {
            OnPlaybackStarted?.Invoke(file);
        }

        void flcFile_OnFrameUpdated(FLCFile file)
        {
            FLCColor[] colors = file.GetFramebufferCopy();

            if (currentFrame != null && 
                (currentFrame.Width != file.Width ||
                 currentFrame.Height != file.Height))
            {
                currentFrame.Dispose();
                currentFrame = null;
            }

            if (currentFrame == null)
            {
                currentFrame = new Texture2D(device, file.Width, file.Height, false, SurfaceFormat.Color);
            }

            Color[] colorData = new Color[currentFrame.Width * currentFrame.Height];

            for (int i = 0; i < colors.Length; i++)
            {
                colorData[i] = new Color(colors[i].R, colors[i].G, colors[i].B, colors[i].A);
            }

            lock (currentFrame)
            {
                currentFrame.SetData<Color>(colorData);

                OnFrameUpdated?.Invoke(currentFrame, file);
            }
        }

        public void Play()
        {
            if (flcFile == null)
            {
                throw new Exception("File has not been opened successfully. Did you call Open()?");
            }

            flcFile.Play();
        }

        public void Stop()
        {
            if (flcFile == null)
            {
                throw new Exception("File has not been opened successfully. Did you call Open()?");
            }

            flcFile.Stop();
        }

        public void Pause()
        {
            if (flcFile == null)
            {
                throw new Exception("File has not been opened successfully. Did you call Open()?");
            }

            flcFile.Pause();
        }

        public void Dispose()
        {
            Stop();

            if (currentFrame != null)
            {
                currentFrame.Dispose();
                currentFrame = null;
            }

            if (flcFile != null)
            {
                flcFile.Dispose();
                flcFile = null;
            }
        }
    }
}
