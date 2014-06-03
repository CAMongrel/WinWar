using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data;
using WinWarRT.Data.Game;
using WinWarRT.Data.Resources;
using WinWarRT.Graphics;

namespace WinWarRT.Gui
{
   class UISpriteImage : UIBaseComponent
   {
      private Sprite sprite;

      internal UISpriteImage(Sprite setSprite)
      {
         sprite = setSprite;

         Width = sprite.MaxWidth;
         Height = sprite.MaxHeight;
      }

      internal static UISpriteImage FromSpriteResource(string name)
      {
         UISpriteImage res = new UISpriteImage(new Sprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName(name))));
         return res;
      }

      internal static UISpriteImage FromSpriteResource(SpriteResource resource)
      {
         UISpriteImage res = new UISpriteImage(new Sprite(resource));
         return res;
      }

      internal override void Update(GameTime gameTime)
      {
         base.Update(gameTime);

         if (sprite != null)
            sprite.Update(gameTime);
      }

      internal override void Render()
      {
         base.Render();

         if (sprite == null)
            return;

         Vector2 screenPos = ScreenPosition;

         WWTexture image = sprite.CurrentFrame;
         if (image != null)
            image.RenderOnScreen(screenPos.X, screenPos.Y, Width, Height);
      }
   }
}
