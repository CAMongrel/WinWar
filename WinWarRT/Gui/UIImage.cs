using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data.Resources;
using WinWarRT.Graphics;

namespace WinWarRT.Gui
{
    class UIImage : UIBaseComponent
    {
        private WWTexture image;

        public UIImage(WWTexture setImage)
        {
            image = setImage;

            Width = image.Width;
            Height = image.Height;
        }

        public static UIImage FromImageResource(string name)
        {
            UIImage res = new UIImage(WWTexture.FromImageResource(name));
            return res;
        }

        public static UIImage FromImageResource(ImageResource resource)
        {
            UIImage res = new UIImage(WWTexture.FromImageResource(resource));
            return res;
        }

        public override void Render()
        {
            base.Render();

            if (image == null)
                return;

            image.RenderOnScreen(X, Y, Width, Height);
        }
    }
}
