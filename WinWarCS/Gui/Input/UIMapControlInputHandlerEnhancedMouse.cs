using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Game;

namespace WinWarCS.Gui.Input
{
   class UIMapControlInputHandlerEnhancedMouse : UIMapControlInputHandler
   {
      private float camOffsetX;
      private float camOffsetY;

      internal UIMapControlInputHandlerEnhancedMouse (UIMapControl setUIMapControl)
         : base (InputMode.EnhancedMouse, setUIMapControl)
      {
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
         return true;
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         Entity ent = GetEntityAt (position);


         SelectUnitAt (position);

         return true;
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         ShowMagnifierAt (position);

         return true;
      }
   }
}
