using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Resources;
using WinWarGame.Graphics;

namespace WinWarGame.Gui
{
   class UIImage : UIBaseComponent
   {
      private WWTexture image;

      internal UIImage (WWTexture setImage)
      {
         image = setImage;

         if (image != null)
         {
            Width = image.Width;
            Height = image.Height;
         }
      }

      internal static UIImage FromImageResource (string name)
      {
         UIImage res = new UIImage (WWTexture.FromImageResource (name));
         return res;
      }

      internal static UIImage FromImageResource (ImageResource resource)
      {
         UIImage res = new UIImage (WWTexture.FromImageResource (resource));
         return res;
      }

      internal override void Draw()
      {
         base.Draw();

         if (image != null)
         {
            Vector2 screenPos = ScreenPosition;

            image.RenderOnScreen (screenPos.X, screenPos.Y, Width, Height, Color.FromNonPremultiplied (new Vector4 (Vector3.One, CompositeAlpha)));
         }
      }
   }
}
