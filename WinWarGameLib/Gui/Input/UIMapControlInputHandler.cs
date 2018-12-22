using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WinWarCS.Data.Game;
#if !NETFX_CORE
using RectangleF = System.Drawing.RectangleF;
#else
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#endif

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

      protected RectangleF selectionRectangle;
      public RectangleF SelectionRectangle 
      { 
         get { return selectionRectangle; }
      }
      public bool IsSpanningRectangle { get; protected set; }

      private MapUnitOrder mapUnitOrder;

      protected UIMapControlInputHandler (InputMode setInputMode, UIMapControl setUIMapControl)
      {
         mapUnitOrder = MapUnitOrder.None;
         IsSpanningRectangle = false;
         selectionRectangle = RectangleF.Empty;

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
      internal void SetMapUnitOrder(MapUnitOrder setMapUnitOrder)
      {
         mapUnitOrder = setMapUnitOrder;
      }

      protected bool ShowUnitOrderAt(Vector2 position)
      {
         if (mapUnitOrder == MapUnitOrder.None)
            return false;

         MouseCursor.State = MouseCursorState.CrosshairOrange;

         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         int tileX = 0;
         int tileY = 0;
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);

         if (MapControl.CurrentMap != null) 
         {
            if (MapControl.CurrentMap.GetDiscoverStateAtTile(tileX, tileY) != MapDiscover.Visible)
            {
               MouseCursor.State = MouseCursorState.CrosshairOrange;
               return true;
            }

            Entity ent = MapControl.CurrentMap.GetEntityAt (tileX, tileY);
            if (ent != null) 
            {
               if (ent.IsNeutralTowards(MapControl.CurrentMap.HumanPlayer))
                  MouseCursor.State = MouseCursorState.CrosshairOrange;
               if (ent.IsHostileTowards(MapControl.CurrentMap.HumanPlayer))
                  MouseCursor.State = MouseCursorState.CrosshairRed;
               if (ent.IsFriendlyTowards(MapControl.CurrentMap.HumanPlayer))
                  MouseCursor.State = MouseCursorState.CrosshairGreen;
            }
         }

         return true;
      }

      protected Entity[] GetSelectedEntities()
      {
         if (MapControl.CurrentMap != null) 
         {
            return MapControl.CurrentMap.GetSelectedEntities();
         }
         return null;
      }

      protected void Deselect()
      {
         MapControl.CurrentMap.SelectEntities (null);
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
            if (MapControl.CurrentMap.GetDiscoverStateAtTile (tileX, tileY) != MapDiscover.Visible)
               return false;

            Entity ent = MapControl.CurrentMap.GetEntityAt (tileX, tileY);
            MapControl.CurrentMap.SelectEntities (ent);
            return true;
         }

         return false;
      }

      protected void SelectUnitsInRectangle (RectangleF selectionRectangle)
      {
         Vector2 localPosition = new Vector2 (selectionRectangle.X - MapControl.X, selectionRectangle.Y - MapControl.Y);
         int startTileX = 0;
         int startTileY = 0;
         int endTileX = 0;
         int endTileY = 0;

         MapControl.GetTileXY (localPosition.X, localPosition.Y, out startTileX, out startTileY);
         MapControl.GetTileXY (localPosition.X + selectionRectangle.Width, localPosition.Y + selectionRectangle.Height, out endTileX, out endTileY);

         Rectangle tileRect = new Rectangle (startTileX, startTileY, endTileX - startTileX, endTileY - startTileY);
         if (tileRect.Width < 0)
         {
            tileRect.X += tileRect.Width;
            tileRect.Width = -tileRect.Width;
         }
         if (tileRect.Height < 0)
         {
            tileRect.Y += tileRect.Height;
            tileRect.Height = -tileRect.Height;
         }
         tileRect.Width += 1;
         tileRect.Height += 1;

         // TODO: Handle mixed owners, handle only non-player owners

         bool mayHaveToFilterOutEntities = false;
         List<Entity> selectionCandidates = new List<Entity> ();
         Entity[] humanPlayerEntities = MapControl.CurrentMap.HumanPlayer.Entities.ToArray ();
         for (int i = 0; i < humanPlayerEntities.Length; i++)
         {
            Entity ent = humanPlayerEntities [i];

            Rectangle entRect = new Rectangle (ent.TileX, ent.TileY, ent.TileSizeX, ent.TileSizeY);

            if (tileRect.Contains(entRect) ||
               tileRect.Intersects(entRect))
            {
               if (ent.AllowsMultiSelection == false)
                  mayHaveToFilterOutEntities = true;

               selectionCandidates.Add (ent);
            }
         }

         if (mayHaveToFilterOutEntities && selectionCandidates.Count > 1)
         {
            // Remove all entities which do not allow multi selection
            for (int i = selectionCandidates.Count - 1; i >= 0; i--)
            {
               if (selectionCandidates [i].AllowsMultiSelection == false)
                  selectionCandidates.RemoveAt (i);
            }
         }

         MapControl.CurrentMap.SelectEntities (selectionCandidates.ToArray ());
      }

      protected bool ShowMagnifierAt(Microsoft.Xna.Framework.Vector2 position)
      {
         Vector2 localPosition = new Vector2 (position.X - MapControl.X, position.Y - MapControl.Y);
         int tileX = 0;
         int tileY = 0;
         MapControl.GetTileXY (localPosition.X, localPosition.Y, out tileX, out tileY);

         if (MapControl.CurrentMap != null) 
         {
            if (MapControl.CurrentMap.GetDiscoverStateAtTile (tileX, tileY) != MapDiscover.Visible)
               return false;

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
