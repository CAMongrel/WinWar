using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Resources;
using WinWarCS.Graphics;

namespace WinWarCS.Gui
{
    class UIImage : UIBaseComponent
    {
        private WWTexture image;

        internal UIImage(WWTexture setImage)
        {
            image = setImage;

            Width = image.Width;
            Height = image.Height;
        }

        internal static UIImage FromImageResource(string name)
        {
            UIImage res = new UIImage(WWTexture.FromImageResource(name));
            return res;
        }

        internal static UIImage FromImageResource(ImageResource resource)
        {
            UIImage res = new UIImage(WWTexture.FromImageResource(resource));
            return res;
        }

        internal override void Render()
        {
            base.Render();

            if (image == null)
                return;

            Vector2 screenPos = ScreenPosition;

            image.RenderOnScreen(screenPos.X, screenPos.Y, Width, Height, Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha)));
        }
    }
}
