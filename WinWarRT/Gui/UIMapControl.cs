using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data;
using WinWarRT.Data.Game;
using WinWarRT.Data.Resources;
using WinWarRT.Gui.Input;

namespace WinWarRT.Gui
{
    class UIMapControl : UIBaseComponent
    {
        private float mapOffsetX;
        private float mapOffsetY;

        public int CameraTileX { get { return ((int)mapOffsetX / TileWidth); } }
        public int CameraTileY { get { return ((int)mapOffsetY / TileHeight); } }

        public int MapWidth
        {
            get { return (CurrentMap != null ? CurrentMap.MapWidth : 0); }
        }
        public int MapHeight
        {
            get { return (CurrentMap != null ? CurrentMap.MapHeight : 0); }
        }
        public int TileWidth
        {
            get { return (CurrentMap != null ? CurrentMap.TileWidth : 0); }
        }
        public int TileHeight
        {
            get { return (CurrentMap != null ? CurrentMap.TileHeight : 0); }
        }
        public int WidthInTiles
        {
            get { return (CurrentMap != null ? Width / TileWidth : 0); }
        }
        public int HeightInTiles
        {
            get { return (CurrentMap != null ? Height / TileHeight : 0); }
        }

        public Map CurrentMap { get; private set; }

        public UIMapControlInputHandler InputHandler { get; private set; }

        public UIMapControl()
        {
            CurrentMap = null;

            InputHandler = new UIMapControlInputHandlerEnhancedMouse(this);
            InputHandler.OnMapDidScroll += inputHandler_OnMapDidScroll;
        }

        void inputHandler_OnMapDidScroll(float setMapOffsetX, float setMapOffsetY)
        {
            SetCameraOffset(setMapOffsetX, setMapOffsetY);
        }

        public void SetCameraOffset(float setCamOffsetX, float setCamOffsetY)
        {
            mapOffsetX = setCamOffsetX;
            mapOffsetY = setCamOffsetY;
        }

        public void LoadCampaignLevel(string basename)
        {
            LevelInfoResource levelInfo = new LevelInfoResource(basename);
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            CurrentMap = new Map(levelInfo, levelVisual, levelPassable);
        }

        public void LoadCustomLevel(string basename)
        {
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            CurrentMap = new Map(null, levelVisual, levelPassable);
        }

        public override void Render()
        {
            base.Render();

            if (CurrentMap != null)
            {
                CurrentMap.Render(this.X, this.Y, this.Width, this.Height, mapOffsetX, mapOffsetY);
            }
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerDown(position);
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerUp(position);
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerMoved(position);
        }
    }
}
