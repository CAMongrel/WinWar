using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Util;

namespace WinWarCS.Data.Game
{
   [Flags]
   enum SpriteAnimationParams
   {
      None,
      RandomDuration,
      FiveFrameDirection,
      Loop
   }

   enum SpriteAnimationPhase
   {
      Initialized,
      Running,
      Finished
   }

   internal delegate void AnimationDidChange();

   class SpriteAnimation
   {
      private int currentAnimationFrame;
      private List<int> animationFrames;

      internal string Name { get; private set; }

      private double currentFrameTime;
      internal double FrameDelay { get; set; }

      internal SpriteAnimationParams Params { get; private set; }
      internal SpriteAnimationPhase Phase { get; private set; }

      internal event AnimationDidChange OnAnimationDidStart;
      internal event AnimationDidChange OnAnimationDidFinish;

      internal int CurrentFrameIndex
      {
         get
         {
            return animationFrames[currentAnimationFrame + fiveFrameOffset];
         }
      }

      internal int fiveFrameOffset { get; private set; }

      internal bool ShouldFlipX { get; private set; }

      private Orientation orientation;
      internal Orientation Orientation
      {
         get
         { 
            return orientation;
         }
         set
         {
            orientation = value;
            UpdateFiveFrameoffset ();
         }
      }

      internal SpriteAnimation(string setName, SpriteAnimationParams setParams)
      {
         fiveFrameOffset = 0;
         Params = setParams;
         currentFrameTime = 0;
         Name = setName;
         currentAnimationFrame = 0;
         animationFrames = new List<int>();
         Phase = SpriteAnimationPhase.Initialized;
         Orientation = Orientation.North;
      }

      internal void AddAnimationFrame(int frameIndex)
      {
         animationFrames.Add(frameIndex);
      }

      internal void AddAnimationFrames(params int[] frameIndices)
      {
         animationFrames.AddRange(frameIndices);
      }

      private void UpdateFiveFrameoffset()
      {
         if (Params.HasFlag (SpriteAnimationParams.FiveFrameDirection) == false) 
         {
            fiveFrameOffset = 0;
            return;
         }

         int orientIndex = (int)orientation;
         if (orientIndex >= 0 && orientIndex <= 4) 
         {
            ShouldFlipX = false;
            fiveFrameOffset = orientIndex;
         } 
         else 
         {
            ShouldFlipX = true;
            fiveFrameOffset = ((3 - (orientIndex - 5)) % (int)Orientation.NorthWest);
         }
      }

      internal void Update(GameTime gameTime)
      {
         if (Phase == SpriteAnimationPhase.Initialized)
         {
            Phase = SpriteAnimationPhase.Running;
            if (OnAnimationDidStart != null)
               OnAnimationDidStart ();
         }

         if (Phase == SpriteAnimationPhase.Finished)
            return;

         currentFrameTime += gameTime.ElapsedGameTime.TotalSeconds;

         int frameSteps = (int)(currentFrameTime / FrameDelay);
         if (Params.HasFlag(SpriteAnimationParams.FiveFrameDirection))
            frameSteps *= 5;
         currentAnimationFrame += frameSteps;

         if (frameSteps > 0)
         {
            currentFrameTime = 0;
         }

         // Loop
         if (currentAnimationFrame >= animationFrames.Count)
         {
            if (Params.HasFlag (SpriteAnimationParams.Loop))
            {
               currentAnimationFrame %= animationFrames.Count;
            } else
            {
               int step = 1;
               if (Params.HasFlag(SpriteAnimationParams.FiveFrameDirection))
                  step *= 5;
               currentAnimationFrame = animationFrames.Count - step;
               Phase = SpriteAnimationPhase.Finished;
               if (OnAnimationDidFinish != null)
                  OnAnimationDidFinish ();
            }
         }
      }

      internal void Reset()
      {
         currentAnimationFrame = 0;
         currentFrameTime = 0;
         Phase = SpriteAnimationPhase.Initialized;
         UpdateFiveFrameoffset ();
      }
   }
}
