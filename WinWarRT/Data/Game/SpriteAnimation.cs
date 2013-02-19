using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Data.Game
{
   class SpriteAnimation
   {
      private int currentAnimationFrame;
      private List<int> animationFrames;

      internal string Name { get; private set; }

      private double currentFrameTime;
      internal double FrameDelay { get; set; }

      internal int CurrentFrameIndex
      {
         get
         {
            return animationFrames[currentAnimationFrame];
         }
      }

      internal SpriteAnimation(string setName)
      {
         currentFrameTime = 0;
         Name = setName;
         currentAnimationFrame = 0;
         animationFrames = new List<int>();
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
         currentFrameTime += gameTime.ElapsedGameTime.TotalSeconds;

         int frameSteps = (int)(currentFrameTime / FrameDelay);
         currentAnimationFrame += frameSteps;

         if (frameSteps > 0)
         {
            currentFrameTime = 0;
         }

         // Loop
         if (currentAnimationFrame >= animationFrames.Count)
            currentAnimationFrame %= animationFrames.Count;
      }

      internal void Reset()
      {
         currentAnimationFrame = 0;
         currentFrameTime = 0;
      }
   }
}
