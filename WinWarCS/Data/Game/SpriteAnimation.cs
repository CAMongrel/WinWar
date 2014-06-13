using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      internal Orientation Orientation
      {
         get
         { 
            if (ShouldFlipX == false)
               return (Orientation)fiveFrameOffset;
            else
               return (Orientation)((3 - (fiveFrameOffset - 5)) + 5);
         }
         set
         {
            int orientIndex = (int)value;
            if (orientIndex >= 0 && orientIndex <= 4) 
            {
               ShouldFlipX = false;
               fiveFrameOffset = orientIndex;
            } 
            else 
            {
               ShouldFlipX = true;
               fiveFrameOffset = (3 - (orientIndex - 5));
            }
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
               currentAnimationFrame = animationFrames.Count - 1;
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
      }
   }
}
