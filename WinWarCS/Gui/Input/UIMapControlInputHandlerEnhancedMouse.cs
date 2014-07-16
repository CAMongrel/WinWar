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

      internal bool PerformRightClick(int tileX, int tileY)
      {
         Entity selEnt = GetSelectedEntity ();
         if (selEnt == null || selEnt.CanGiveCommands() == false)
            return true;

         Entity ent = GetEntityAtTileXY (tileX, tileY);

         if (ent == null || ent.Owner == null) 
         {
            selEnt.MoveTo (tileX, tileY);
         } 
         else 
         {
            // If we right-clicked a neutral builind. Move towards it.
            // TODO: Handle other orders (like harvesting)
            if (ent.Owner.IsNeutralTowards (selEnt.Owner))
               selEnt.MoveTo (ent.TileX, ent.TileY);

            if (ent.Owner.IsFriendlyTowards (selEnt.Owner))
               selEnt.MoveTo (ent.TileX, ent.TileY);

            if (ent.Owner.IsHostileTowards (selEnt.Owner))
               selEnt.Attack(ent);
         }

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
            int tileX, tileY;
            GetTileAt (position, out tileX, out tileY);

            return PerformRightClick (tileX, tileY);
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
