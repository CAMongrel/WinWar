using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.GameScreens.Windows;
using WinWarRT.Graphics;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens
{
    class MenuGameScreen : BaseGameScreen
    {
        public static MenuGameScreen Menu { get; private set; }

        public MenuGameScreen()
        {
            Menu = this;
        }

        public override void InitUI()
        {
            new MenuBackgroundWindow();
            new MainMenuWindow(); 
        }

        public override void Close()
        {
            UIWindowManager.Clear();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
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
