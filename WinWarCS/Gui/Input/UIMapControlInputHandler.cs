using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WinWarCS.Data.Game;

namespace WinWarCS.Gui.Input
{
   enum InputMode
   {
      Classic,
      EnhancedMouse,
      EnhancedTouch
   }

   internal delegate void MapDidScroll (float setMapOffsetX, float setMapOffsetY);

   class UIMapControlInputHandler
   {
      internal InputMode InputMode { get; private set; }

      internal UIMapControl MapControl { get; private set; }

      internal event MapDidScroll OnMapDidScroll;

      protected UIMapControlInputHandler (InputMode setInputMode, UIMapControl setUIMapControl)
      {
         MapControl = setUIMapControl;
         InputMode = setInputMode;
      }

      internal virtual void SetCameraOffset (float setCamOffsetX, float setCamOffsetY)
      {
         //
      }

      internal virtual bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return true;
      }

      internal virtual bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return true;
      }

      internal virtual bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         return true;
      }

      protected void InvokeOnMapDidScroll (float setMapOffsetX, float setMapOffsetY)
      {
         if (OnMapDidScroll != null)
            OnMapDidScroll (setMapOffsetX, setMapOffsetY);
      }

      #region Input logic for all handlers
      protected Entity GetSelectedEntity()
      {
         if (MapControl.CurrentMap != null) 
         {
            return MapControl.CurrentMap.SelectedEntity;
         }
         return null;
      }

      protected void Deselect()
      {
         MapControl.CurrentMap.SelectEntity (null);
      }

      protected void GetTileAt(Microsoft.Xna.Framework.Vector2 position, out int tileX, out int tileY)
      {
         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);
      }

      protected Entity GetEntityAt(Microsoft.Xna.Framework.Vector2 position)
      {
         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         int tileX = 0;
         int tileY = 0;
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);

         return GetEntityAtTileXY (tileX, tileY);
      }

      protected Entity GetEntityAtTileXY(int tileX, int tileY)
      {
         return MapControl.CurrentMap.GetEntityAt (tileX, tileY);
      }

      protected bool SelectUnitAt(Microsoft.Xna.Framework.Vector2 position)
      {
         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         int tileX = 0;
         int tileY = 0;
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);

         if (MapControl.CurrentMap != null) 
         {
            Entity ent = MapControl.CurrentMap.GetEntityAt (tileX, tileY);
            MapControl.CurrentMap.SelectEntity (ent);
            return true;
         }

         return false;
      }

      protected bool ShowMagnifierAt(Microsoft.Xna.Framework.Vector2 position)
      {
         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         int tileX = 0;
         int tileY = 0;
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);

         if (MapControl.CurrentMap != null) 
         {
            Entity ent = MapControl.CurrentMap.GetEntityAt (tileX, tileY);
            if (ent != null && ent.CanBeSelected) 
            {
               MouseCursor.State = MouseCursorState.Magnifier;
               return true;
            }
         }

         return false;
      }
      #endregion
   }
}
