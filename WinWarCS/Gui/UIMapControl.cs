using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;
using WinWarCS.Data.Game;
using WinWarCS.Data.Resources;
using WinWarCS.Gui.Input;

namespace WinWarCS.Gui
{
   class UIMapControl : UIBaseComponent
   {
      private float mapOffsetX;
      private float mapOffsetY;

      internal int CameraTileX { get { return ((int)mapOffsetX / TileWidth); } }

      internal int CameraTileY { get { return ((int)mapOffsetY / TileHeight); } }

      internal int MapWidth {
         get { return (CurrentMap != null ? CurrentMap.MapWidth : 1); }
      }

      internal int MapHeight {
         get { return (CurrentMap != null ? CurrentMap.MapHeight : 1); }
      }

      internal int TileWidth {
         get { return (CurrentMap != null ? CurrentMap.TileWidth : 1); }
      }

      internal int TileHeight {
         get { return (CurrentMap != null ? CurrentMap.TileHeight : 1); }
      }

      internal int WidthInTiles {
         get { return (CurrentMap != null ? Width / TileWidth : 1); }
      }

      internal int HeightInTiles {
         get { return (CurrentMap != null ? Height / TileHeight : 1); }
      }

      internal Map CurrentMap { get; private set; }

      internal UIMapControlInputHandler InputHandler { get; private set; }

      internal UIMapControl ()
      {
         CurrentMap = null;

         SetInputMode (InputMode.EnhancedMouse);
      }

      internal void SetInputMode(InputMode setMode)
      {
         switch (setMode) 
         {
         case InputMode.Classic:
            InputHandler = new UIMapControlInputHandlerClassic (this);
            break;

         case InputMode.EnhancedMouse:
            InputHandler = new UIMapControlInputHandlerEnhancedMouse (this);
            break;

         case InputMode.EnhancedTouch:
            InputHandler = new UIMapControlInputHandlerEnhancedTouch (this);
            break;
         }

         InputHandler.OnMapDidScroll += inputHandler_OnMapDidScroll;
      }

      internal void GetTileXY(float x, float y, out int tileX, out int tileY)
      {
         tileX = (int)((x + mapOffsetX) / (float)TileWidth);
         tileY = (int)((y + mapOffsetY) / (float)TileHeight);
      }

      void inputHandler_OnMapDidScroll (float setMapOffsetX, float setMapOffsetY)
      {
         SetCameraOffset (setMapOffsetX, setMapOffsetY);
      }

      internal void SetCameraOffset (float setCamOffsetX, float setCamOffsetY)
      {
         mapOffsetX = setCamOffsetX;
         mapOffsetY = setCamOffsetY;

         if (mapOffsetX < 0)
            mapOffsetX = 0;
         if (mapOffsetY < 0)
            mapOffsetY = 0;

         float maxX = (this.MapWidth * this.TileWidth) - this.Width;
         if (mapOffsetX > maxX)
            mapOffsetX = maxX;
         float maxY = (this.MapHeight * this.TileHeight) - this.Height;
         if (mapOffsetY > maxY)
            mapOffsetY = maxY;
      }

      internal void LoadCampaignLevel (string basename)
      {
         LevelInfoResource levelInfo = new LevelInfoResource (basename);
         LevelPassableResource levelPassable = new LevelPassableResource (basename + " (Passable)");
         LevelVisualResource levelVisual = new LevelVisualResource (basename + " (Visual)");

         CurrentMap = new Map (levelInfo, levelVisual, levelPassable);
         SetCameraOffset (levelInfo.StartCameraX * CurrentMap.TileWidth, levelInfo.StartCameraY * CurrentMap.TileHeight);
      }

      internal void LoadCustomLevel (string basename)
      {
         LevelPassableResource levelPassable = new LevelPassableResource (basename + " (Passable)");
         LevelVisualResource levelVisual = new LevelVisualResource (basename + " (Visual)");

         CurrentMap = new Map (null, levelVisual, levelPassable);
      }

      internal override void Update (GameTime gameTime)
      {
         base.Update (gameTime);

         if (CurrentMap != null) 
         {
            CurrentMap.Update (gameTime);
         }
      }

      internal override void Render ()
      {
         base.Render ();

         if (CurrentMap != null) 
         {
            CurrentMap.Render (this.X, this.Y, this.Width, this.Height, mapOffsetX, mapOffsetY);
         }
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return InputHandler.PointerDown (position);
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return InputHandler.PointerUp (position);
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         return InputHandler.PointerMoved (position);
      }
   }
}
