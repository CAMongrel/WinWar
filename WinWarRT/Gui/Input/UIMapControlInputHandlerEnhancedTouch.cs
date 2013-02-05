using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui.Input
{
    class UIMapControlInputHandlerEnhancedTouch : UIMapControlInputHandler
    {
        private float mapOffsetX;
        private float mapOffsetY;
        private bool isPressed;
        private Vector2 lastPos;

        public UIMapControlInputHandlerEnhancedTouch(UIMapControl setUIMapControl)
            : base(InputMode.EnhancedTouch, setUIMapControl)
        {
            isPressed = false;
            mapOffsetX = 0;
            mapOffsetY = 0;
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            isPressed = true;
            lastPos = position;

            return true;
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            isPressed = false;

            return true;
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            if (isPressed)
            {
                float dx = lastPos.X - position.X;
                float dy = lastPos.Y - position.Y;

                mapOffsetX += dx;
                mapOffsetY += dy;

                if (mapOffsetX < 0)
                    mapOffsetX = 0;
                if (mapOffsetY < 0)
                    mapOffsetY = 0;

                float maxX = (MapControl.MapWidth * MapControl.TileWidth) - MapControl.Width;
                if (mapOffsetX > maxX)
                    mapOffsetX = maxX;
                float maxY = (MapControl.MapHeight * MapControl.TileHeight) - MapControl.Height;
                if (mapOffsetY > maxY)
                    mapOffsetY = maxY;

                lastPos = position;

                InvokeOnMapDidScroll(mapOffsetX, mapOffsetY);
            }

            return true;
        }
    }
}
