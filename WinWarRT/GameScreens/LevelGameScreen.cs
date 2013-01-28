using System;
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
        public static LevelGameScreen Game { get; private set; }

        public Player HumanPlayer { get; private set; }

        private GameBackgroundWindow backgroundWindow;

        public LevelGameScreen(Player setHumanPlayer)
        {
            Game = this;

            HumanPlayer = setHumanPlayer;
        }

        public override void InitUI()
        {
            backgroundWindow = new GameBackgroundWindow();
        }

        public override void Close()
        {
            UIWindowManager.Clear();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //UIWindowManager.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UIWindowManager.Render();
        }

        public override void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerDown(position);
        }

        public override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerUp(position);
        }
    }
}
