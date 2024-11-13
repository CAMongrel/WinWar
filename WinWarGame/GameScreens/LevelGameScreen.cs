using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Game;
using WinWarGame.GameScreens.Windows;
using WinWarGame.Gui;

namespace WinWarGame.GameScreens
{
    class LevelGameScreen : BaseGameScreen
    {
        internal static LevelGameScreen Game { get; private set; }

        /*internal Campaign Campaign { get; private set; }

        internal bool IsCampaignLevel => Campaign != null;*/

        private Map currentMap;

        private GameBackgroundWindow backgroundWindow;

        internal Race InterfaceRace => currentMap?.HumanPlayer?.Race ?? Race.Humans;

        internal bool GamePaused
        {
            get { return backgroundWindow.GamePaused; }
            set { backgroundWindow.GamePaused = value; }
        }

        internal LevelGameScreen(Map setMap)//Campaign setCampaign)
        {
            Game = this;
            currentMap = setMap;
        }

        internal override void InitUI()
        {
            MouseCursor.State = MouseCursorState.Pointer;

            backgroundWindow = new GameBackgroundWindow(this);

            backgroundWindow.MapControl.SetCurrentMap(currentMap);
            backgroundWindow.MapControl.CurrentMap.Start();
        }

        internal void SetMapUnitOrder(MapUnitOrder setMapUnitOrder)
        {
            if (backgroundWindow != null &&
                backgroundWindow.MapControl != null &&
                backgroundWindow.MapControl.InputHandler != null)
            {
                backgroundWindow.MapControl.InputHandler.SetMapUnitOrder(setMapUnitOrder);
            }
        }

        internal override void Close()
        {
            UIWindowManager.Clear();
        }

        internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UIWindowManager.Render();
        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerDown(position, pointerType);
        }

        internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerUp(position, pointerType);
        }

        internal override void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerMoved(position);
        }
    }
}