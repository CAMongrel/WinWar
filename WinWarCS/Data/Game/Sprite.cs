using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Resources;
using WinWarCS.Graphics;

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
      private SpriteFrame[] frames;

      internal int MaxWidth { get; private set; }
      internal int MaxHeight { get; private set; }

      private List<SpriteAnimation> allAnimations;

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
            if (frames == null || frames.Length <= 0)
               return null;

            if (CurrentAnimation == null) 
            {
               return frames [0];
            }

            if (CurrentAnimation.CurrentFrameIndex < 0 || CurrentAnimation.CurrentFrameIndex >= frames.Length)
               return null;

            return frames[CurrentAnimation.CurrentFrameIndex];
         }
      }

      internal WWTexture CurrentFrameTexture
      {
         get
         {
            if (frames == null || frames.Length <= 0)
               return null;

            if (CurrentAnimation == null) 
            {
               return frames [0].texture;
            }

            if (CurrentAnimation.CurrentFrameIndex < 0 || CurrentAnimation.CurrentFrameIndex >= frames.Length)
               return null;

            return frames[CurrentAnimation.CurrentFrameIndex].texture;
         }
      }

      internal Sprite(SpriteResource resource)
      {
         allAnimations = new List<SpriteAnimation>();
         CurrentAnimation = null;

         if (resource.Frames == null)
            resource.CreateImageData(false, false, false);

         MaxWidth = resource.MaxWidth;
         MaxHeight = resource.MaxHeight;

         frames = new SpriteFrame[resource.FrameCount];

         for (int i = 0; i < resource.FrameCount; i++)
         {
            Texture2D DXTexture = new Texture2D(MainGame.Device, resource.MaxWidth, resource.MaxHeight, false, SurfaceFormat.Color);
            DXTexture.SetData<byte>(resource.Frames[i].image_data);

            frames [i] = new SpriteFrame ();
            frames [i].OffsetX = resource.Frames [i].disp_x;
            frames [i].OffsetY = resource.Frames [i].disp_y;
            frames [i].Width = resource.Frames [i].width;
            frames [i].Height = resource.Frames [i].height;
            frames[i].texture = WWTexture.FromDXTexture(DXTexture);
         }
      }

#if !NETFX_CORE
      internal void DumpToDirectory(string fullDirectory, string prefix)
      {
         if (System.IO.Directory.Exists (fullDirectory) == false)
            System.IO.Directory.CreateDirectory (fullDirectory);

         for (int i = 0; i < frames.Length; i++)
         {
            frames [i].texture.WriteToFile (System.IO.Path.Combine (fullDirectory, prefix + i + ".png"));
         }
      }
#endif

      internal void AddAnimation(string name, double delay, SpriteAnimationParams setParams, params int[] frames)
      {
         SpriteAnimation anim = new SpriteAnimation(name, setParams);
         anim.FrameDelay = delay;
         anim.AddAnimationFrames(frames);
         allAnimations.Add(anim);
      }

      internal virtual void SetCurrentAnimationByName(string name)
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
            CurrentAnimation.Reset();
         }
      }

      internal void Update(GameTime gameTime)
      {
         if (CurrentAnimation != null)
            CurrentAnimation.Update(gameTime);
      }
   }
}
