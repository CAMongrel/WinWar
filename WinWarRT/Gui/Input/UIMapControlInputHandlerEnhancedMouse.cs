using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui.Input
{
    class UIMapControlInputHandlerEnhancedMouse : UIMapControlInputHandler
    {
        private float camOffsetX;
        private float camOffsetY;

        public UIMapControlInputHandlerEnhancedMouse(UIMapControl setUIMapControl)
            : base(InputMode.EnhancedMouse, setUIMapControl)
        {
        }

        public override void SetCameraOffset(float setCamOffsetX, float setCamOffsetY)
        {
            camOffsetX = setCamOffsetX;
            camOffsetY = setCamOffsetY;

            InvokeOnMapDidScroll(camOffsetX, camOffsetY);
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            if (position.X <= 0)
            {

            }

            return true;
        }
    }
}
