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
   struct SpriteFrame
   {
      internal WWTexture texture;
   }

   class Sprite
   {
      private SpriteFrame[] frames;

      internal int MaxWidth { get; private set; }
      internal int MaxHeight { get; private set; }

      private List<SpriteAnimation> allAnimations;

      internal SpriteAnimation CurrentAnimation { get; private set; }

      internal WWTexture CurrentFrame
      {
         get
         {
            if (frames == null || frames.Length <= 0 || CurrentAnimation == null || CurrentAnimation.CurrentFrameIndex < 0 || CurrentAnimation.CurrentFrameIndex >= frames.Length)
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

            frames[i].texture = WWTexture.FromDXTexture(DXTexture);
         }
      }

      internal void AddAnimation(string name, double delay, params int[] frames)
      {
         SpriteAnimation anim = new SpriteAnimation(name);
         anim.FrameDelay = delay;
         anim.AddAnimationFrames(frames);
         allAnimations.Add(anim);
      }

      internal void SetCurrentAnimation(string name)
      {
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
