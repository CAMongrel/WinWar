using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Resources;
using WinWarCS.Graphics;
using WinWarCS.Util;

namespace WinWarCS.Data.Game
{
   class SpriteFrame
   {
      internal byte OffsetX;
      internal byte OffsetY;
      internal byte Width;
      internal byte Height;
      internal WWTexture texture;
   }

   internal class Sprite
   {
      private List<SpriteAnimation> allAnimations;
      private SpriteFrameData frameData;

      internal SpriteAnimation CurrentAnimation { get; private set; }

      internal bool ShouldFlipX
      {
         get
         {
            if (CurrentAnimation == null)
               return false;

            return CurrentAnimation.ShouldFlipX;
         }
      }

      internal SpriteFrame CurrentFrame
      {
         get
         {
            if (frameData == null || frameData.Frames == null || frameData.Frames.Length <= 0)
               return null;

            if (CurrentAnimation == null) 
            {
               return frameData.Frames[0];
            }

            if (CurrentAnimation.CurrentFrameIndex < 0 || CurrentAnimation.CurrentFrameIndex >= frameData.Frames.Length)
               return null;

            return frameData.Frames[CurrentAnimation.CurrentFrameIndex];
         }
      }

      internal WWTexture CurrentFrameTexture
      {
         get
         {
            if (frameData == null || frameData.Frames == null || frameData.Frames.Length <= 0)
               return null;

            if (CurrentAnimation == null) 
            {
               return frameData.Frames[0].texture;
            }

            if (CurrentAnimation.CurrentFrameIndex < 0 || CurrentAnimation.CurrentFrameIndex >= frameData.Frames.Length)
               return null;

            return frameData.Frames[CurrentAnimation.CurrentFrameIndex].texture;
         }
      }

      internal int MaxWidth
      {
         get
         {
            if (frameData == null)
               return 0;
            return frameData.MaxWidth;
         }
      }
      internal int MaxHeight
      {
         get
         {
            if (frameData == null)
               return 0;
            return frameData.MaxHeight;
         }
      }

      internal Sprite(SpriteResource resource)
      {
         Performance.Push("Sprite ctor");
         allAnimations = new List<SpriteAnimation>();
         CurrentAnimation = null;

         frameData = SpriteFrameData.LoadSpriteFrameData(resource);

         Performance.Pop();
      }

#if !NETFX_CORE && !IOS
      internal void DumpToDirectory(string fullDirectory, string prefix)
      {
         if (System.IO.Directory.Exists (fullDirectory) == false)
            System.IO.Directory.CreateDirectory (fullDirectory);

         for (int i = 0; i < frameData.Frames.Length; i++)
         {
            frameData.Frames[i].texture.WriteToFile (System.IO.Path.Combine (fullDirectory, prefix + i + ".png"));
         }
      }
#endif

      internal void AddAnimation(string name, double delay, SpriteAnimationParams setParams, params int[] frames)
      {
         int[] newFrames = null;
         if (setParams.HasFlag (SpriteAnimationParams.FiveFrameDirection)) 
         {
            newFrames = new int[frames.Length * 5];
            for (int i = 0; i < frames.Length; i++) 
            {
               for (int j = 0; j < 5; j++) 
               {
                  newFrames[i * 5 + j] = frames[i] + j;
               }
            }
         }
         else
            newFrames = frames;

         SpriteAnimation anim = new SpriteAnimation(name, setParams);
         anim.FrameDelay = delay;
         anim.AddAnimationFrames(newFrames);
         allAnimations.Add(anim);
      }

      internal virtual void SetCurrentAnimationByName(string name, double? overrideDelay = null)
      {
         if (CurrentAnimation != null)
         {
            //CurrentAnimation.OnAnimationDidStart = null;
            //CurrentAnimation.OnAnimationDidFinish = null;
         }

         SpriteAnimation animation = (from anim in allAnimations where anim.Name.ToLowerInvariant() == name.ToLowerInvariant() select anim).FirstOrDefault();
         if (animation != null)
         {
            CurrentAnimation = animation;
            if (overrideDelay != null)
               CurrentAnimation.FrameDelay = overrideDelay.Value;
            CurrentAnimation.Reset();
         }
      }

      internal void Update(GameTime gameTime)
      {
         if (CurrentAnimation != null)
            CurrentAnimation.Update(gameTime);
      }

      public void ApplyCorpseAnimationSet()
      {
         AddAnimation ("Death1", 0.5, SpriteAnimationParams.None, 10, 25, 40);
         AddAnimation ("Death2", 0.5, SpriteAnimationParams.None, 12, 27, 42);
      }
   }
}
