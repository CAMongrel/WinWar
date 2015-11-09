using System;
using WinWarCS.Data.Game;
using WinWarCS.Data;

namespace WinWarCS.Gui
{
   internal class UISpriteButton : UIButton
   {
      private Sprite curSpr;
      private UISpriteImage img;

      internal UISpriteButton(Sprite spr, string spriteName)
         : this(spr, WarFile.KnowledgeBase.IconIDByName(spriteName))
      {
         //
      }

      internal UISpriteButton(Sprite spr, int spriteFrameIndex)
         : base(string.Empty, 364, 365)
      {
         curSpr = spr;
         curSpr.FixedSpriteFrame = spriteFrameIndex;

         img = new UISpriteImage(curSpr);
         img.X = 2;
         img.Y = 2;
         AddComponent(img);
      }

      internal override void Draw()
      {
         img.Y = (isPressed ? 3 : 2);

         base.Draw();
      }
   }
}

