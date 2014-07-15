using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Gui.Input
{
   class UIMapControlInputHandlerEnhancedTouch : UIMapControlInputHandler
   {
      private float camOffsetX;
      private float camOffsetY;
      private bool isPressed;
      private Vector2 lastPos;

      internal UIMapControlInputHandlerEnhancedTouch (UIMapControl setUIMapControl)
         : base (InputMode.EnhancedTouch, setUIMapControl)
      {
         isPressed = false;
         camOffsetX = 0;
         camOffsetY = 0;

         MouseCursor.IsVisible = true;
      }

      internal override void SetCameraOffset (float setCamOffsetX, float setCamOffsetY)
      {
         camOffsetX = setCamOffsetX;
         camOffsetY = setCamOffsetY;

         InvokeOnMapDidScroll (camOffsetX, camOffsetY);
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         isPressed = true;
         lastPos = position;

         return true;
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         isPressed = false;

         SelectUnitAt (position);

         return true;
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         if (isPressed) 
         {
            float dx = lastPos.X - position.X;
            float dy = lastPos.Y - position.Y;

            camOffsetX += dx;
            camOffsetY += dy;

            if (camOffsetX < 0)
               camOffsetX = 0;
            if (camOffsetY < 0)
               camOffsetY = 0;

            float maxX = (MapControl.MapWidth * MapControl.TileWidth) - MapControl.Width;
            if (camOffsetX > maxX)
               camOffsetX = maxX;
            float maxY = (MapControl.MapHeight * MapControl.TileHeight) - MapControl.Height;
            if (camOffsetY > maxY)
               camOffsetY = maxY;

            lastPos = position;

            InvokeOnMapDidScroll (camOffsetX, camOffsetY);
         }

         return true;
      }
   }
}
