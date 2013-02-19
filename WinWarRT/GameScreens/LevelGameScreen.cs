﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data.Game;
using WinWarRT.GameScreens.Windows;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens
{
    class LevelGameScreen : BaseGameScreen
    {
        internal static LevelGameScreen Game { get; private set; }

        internal Player HumanPlayer { get; private set; }

        private GameBackgroundWindow backgroundWindow;

        internal LevelGameScreen(Player setHumanPlayer)
        {
            Game = this;

            HumanPlayer = setHumanPlayer;
        }

        internal override void InitUI()
        {
            backgroundWindow = new GameBackgroundWindow();

            backgroundWindow.MapControl.LoadCampaignLevel(HumanPlayer.Race + " 1");
        }

        internal override void Close()
        {
            UIWindowManager.Clear();
        }

        internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UIWindowManager.Render();
        }

        internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerDown(position);
        }

        internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerUp(position);
        }

        internal override void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerMoved(position);
        }
    }
}
