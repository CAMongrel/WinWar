using System;
using WinWarCS.Data.Game;

namespace WinWarCS.Gui
{
   internal class UISpriteButton : UIButton
   {
      private Sprite curSpr;
      private UISpriteImage img;

      internal UISpriteButton(Sprite spr, int spriteFrameIndex)
         : base(string.Empty, 364, 365)
      {
         curSpr = spr;
         curSpr.FixedSpriteFrame = spriteFrameIndex;

         img = new UISpriteImage(curSpr);
         img.X = 2;
         img.Y = 2;
         AddComponent(img);

         OnMouseUpInside += (Microsoft.Xna.Framework.Vector2 position) => 
         {
            curSpr.FixedSpriteFrame = curSpr.FixedSpriteFrame + 1;
         };
      }

      internal override void Render()
      {
         img.Y = (isPressed ? 3 : 2);

         base.Render();
      }
   }
}

