using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;
using WinWarCS.Data.Game;
using WinWarCS.Data.Resources;
using WinWarCS.Graphics;

namespace WinWarCS.Gui
{
   class UISpriteImage : UIBaseComponent
   {
      public UnitSprite Sprite { get; private set; }

      public Orientation Orientation
      {
         get
         {
            return Sprite.SpriteOrientation;
         }
         set
         {
            Sprite.SpriteOrientation = value;
         }
      }

      internal UISpriteImage(UnitSprite setSprite)
      {
         Sprite = setSprite;

         Width = Sprite.MaxWidth;
         Height = Sprite.MaxHeight;
      }

      internal static UISpriteImage FromSpriteResource(string name)
      {
         UISpriteImage res = new UISpriteImage(new UnitSprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName(name))));
         return res;
      }

      internal static UISpriteImage FromSpriteResource(SpriteResource resource)
      {
         UISpriteImage res = new UISpriteImage(new UnitSprite(resource));
         return res;
      }

      internal override void Update(GameTime gameTime)
      {
         base.Update(gameTime);

         if (Sprite != null)
            Sprite.Update(gameTime);
      }

      internal override void Render()
      {
         base.Render();

         if (Sprite == null)
            return;

         Vector2 screenPos = ScreenPosition;

         WWTexture image = Sprite.CurrentFrameTexture;
         if (image != null)
            image.RenderOnScreen(screenPos.X, screenPos.Y, Width, Height);
      }
   }
}
