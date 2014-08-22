using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Game;
#if !NETFX_CORE
using RectangleF = System.Drawing.RectangleF;
#else
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#endif

namespace WinWarCS.Gui.Input
{
   class UIMapControlInputHandlerEnhancedMouse : UIMapControlInputHandler
   {
      private float camOffsetX;
      private float camOffsetY;

      private bool isLeftPressed;
      private bool isRightPressed;

      internal UIMapControlInputHandlerEnhancedMouse (UIMapControl setUIMapControl)
         : base (InputMode.EnhancedMouse, setUIMapControl)
      {
         MouseCursor.IsVisible = true;

         isLeftPressed = false;
         isRightPressed = false;
      }

      internal override void SetCameraOffset (float setCamOffsetX, float setCamOffsetY)
      {
         camOffsetX = setCamOffsetX;
         camOffsetY = setCamOffsetY;

         InvokeOnMapDidScroll (camOffsetX, camOffsetY);
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (pointerType == PointerType.LeftMouse)
         {
            isLeftPressed = true;

            selectionRectangle.X = position.X;
            selectionRectangle.Y = position.Y;
         }
         if (pointerType == PointerType.RightMouse)
            isRightPressed = true;

         return true;
      }

      private void PerformRightClickForEntity(Entity selEnt, int tileX, int tileY)
      {
         Entity ent = GetEntityAtTileXY (tileX, tileY);

         if (ent == null || ent.Owner == null) 
         {
            selEnt.MoveTo (tileX, tileY);
         } 
         else 
         {
            // If we right-clicked a neutral entity. Move towards it.
            // TODO: Handle other orders (like harvesting)
            if (ent.Owner.IsNeutralTowards (selEnt.Owner))
               selEnt.MoveTo (ent.TileX, ent.TileY);

            if (ent.Owner.IsFriendlyTowards (selEnt.Owner))
               selEnt.MoveTo (ent.TileX, ent.TileY);

            if (ent.Owner.IsHostileTowards (selEnt.Owner))
               selEnt.Attack(ent);
         }
      }

      internal bool PerformRightClick(int tileX, int tileY)
      {
         Entity[] selEnts = GetSelectedEntities ();
         if (selEnts == null || selEnts.Length == 0)
            return true;

         for (int i = 0; i < selEnts.Length; i++)
         {
            if (selEnts[i].CanGiveCommands() == false)
               continue;

            PerformRightClickForEntity (selEnts [i], tileX, tileY);
         }

         return true;
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (pointerType == PointerType.LeftMouse) 
         {
            isLeftPressed = false;

            if (IsSpanningRectangle)
            {
               SelectUnitsInRectangle (selectionRectangle);

               IsSpanningRectangle = false;
            } 
            else
            {
               SelectUnitAt (position);
            }
         } 
         else if (pointerType == PointerType.RightMouse)
         {
            isRightPressed = false;

            int tileX, tileY;
            GetTileAt (position, out tileX, out tileY);

            return PerformRightClick (tileX, tileY);
         }

         selectionRectangle = RectangleF.Empty;

         return true;
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         if (isLeftPressed)
         {
            float deltaX = position.X - selectionRectangle.X;
            float deltaY = position.Y - selectionRectangle.Y;

            selectionRectangle.Width = deltaX;
            selectionRectangle.Height = deltaY;

            if (Math.Abs(deltaX) > 2 && Math.Abs(deltaY) > 2)
               IsSpanningRectangle = true;
         }

         if (isLeftPressed == false && isRightPressed == false)
            ShowMagnifierAt (position);

         return true;
      }
   }
}
