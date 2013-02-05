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

        public int MapWidth
        {
            get { return (curMap != null ? curMap.MapWidth : 0); }
        }
        public int MapHeight
        {
            get { return (curMap != null ? curMap.MapHeight : 0); }
        }
        public int TileWidth
        {
            get { return (curMap != null ? curMap.TileWidth : 0); }
        }
        public int TileHeight
        {
            get { return (curMap != null ? curMap.TileHeight : 0); }
        }

        private Map curMap;

        private UIMapControlInputHandler inputHandler;

        public UIMapControl()
        {
            curMap = null;

            inputHandler = new UIMapControlInputHandlerEnhancedTouch(this);
            inputHandler.OnMapDidScroll += inputHandler_OnMapDidScroll;
        }

        void inputHandler_OnMapDidScroll(float setMapOffsetX, float setMapOffsetY)
        {
            mapOffsetX = setMapOffsetX;
            mapOffsetY = setMapOffsetY;
        }

        public void LoadCampaignLevel(string basename)
        {
            LevelInfoResource levelInfo = new LevelInfoResource(basename);
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            curMap = new Map(levelInfo, levelVisual, levelPassable);
        }

        public void LoadCustomLevel(string basename)
        {
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            curMap = new Map(null, levelVisual, levelPassable);
        }

        public override void Render()
        {
            base.Render();

            if (curMap != null)
            {
                curMap.Render(this.X, this.Y, this.Width, this.Height, mapOffsetX, mapOffsetY);
            }
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return inputHandler.PointerDown(position);
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return inputHandler.PointerUp(position);
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return inputHandler.PointerMoved(position);
        }
    }
}
