using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WinWarGame.Data.Game;

namespace WinWarGame.Gui.Input
{
   class UIMapControlInputHandlerClassic : UIMapControlInputHandler
   {
      internal UIMapControlInputHandlerClassic (UIMapControl setUIMapControl)
         : base (InputMode.Classic, setUIMapControl)
      {
         MouseCursor.IsVisible = true;
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return true;
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         SelectUnitAt (position);

         return true;
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         if (ShowUnitOrderAt(position) == false)
            ShowMagnifierAt (position);

         return true;
      }
   }
}
