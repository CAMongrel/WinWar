using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Graphics;
using WinWarCS.Gui;

namespace WinWarCS.GameScreens.Windows
{
   class MenuBackgroundWindow : UIWindow
   {
      WWTexture backgroundImage;

      internal MenuBackgroundWindow()
      {
         backgroundImage = WWTexture.FromImageResource("Mainmenu Background");
      }

      internal override void Render()
      {
         Color col = Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha));
         backgroundImage.RenderOnScreen(0, 0, col);

         base.Render();
      }
   }
}
