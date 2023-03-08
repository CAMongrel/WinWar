using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Graphics;
using WinWarGame.Gui;

namespace WinWarGame.GameScreens.Windows
{
   class MenuBackgroundWindow : UIWindow
   {
      private WWTexture backgroundImage;

      internal MenuBackgroundWindow()
      {
         backgroundImage = WWTexture.FromImageResource("Mainmenu Background");
      }

      internal override void Draw()
      {
         base.Draw();

         Color col = Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha));
         backgroundImage.RenderOnScreen(0, 0, col);
      }
   }
}
