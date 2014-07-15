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
         if (pointerType == PointerType.LeftMouse) 
         {
            SelectUnitAt (position);
         } 
         else 
         {
            Entity selEnt = GetSelectedEntity ();
            if (selEnt == null || selEnt.CanGiveCommands() == false)
               return true;

            Entity ent = GetEntityAt (position);

            if (ent == null || ent.Owner == null) 
            {
               int tileX, tileY;
               GetTileAt (position, out tileX, out tileY);
               selEnt.MoveTo (tileX, tileY);
            } 
            else 
            {
               // If we right-clicked a neutral builind. Move towards it.
               // TODO: Handle other orders (like harvesting)
               if (ent.Owner.IsNeutralTowards (selEnt))
                  selEnt.MoveTo (ent.TileX, ent.TileY);

               if (ent.Owner.IsFriendlyTowards (selEnt))
                  selEnt.MoveTo (ent.TileX, ent.TileY);

               if (ent.Owner.IsHostileTowards (selEnt))
                  selEnt.Attack(ent);
            }
         }

         return true;
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         ShowMagnifierAt (position);

         return true;
      }
   }
}
