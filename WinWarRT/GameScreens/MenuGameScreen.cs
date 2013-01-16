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

        private MenuBackgroundWindow backgroundWindow;

        private UIWindow currentWindow;
        private UIWindow nextWindow;

        public MenuGameScreen()
        {
            Menu = this;

            backgroundWindow = new MenuBackgroundWindow();
            currentWindow = new MainMenuWindow(); 
            nextWindow = null;
        }

        public void SetCurrentWidow(UIWindow setWindow)
        {
            nextWindow = setWindow;
        }

        public override void Update(GameTime gameTime)
        {
            if (nextWindow != null)
            {
                currentWindow = nextWindow;
                nextWindow = null;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            backgroundWindow.Render();

            if (currentWindow != null)
            {
                currentWindow.Render();
            }
        }

        public override void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (currentWindow != null)
            {
                currentWindow.MouseDown(position);
            }
        }

        public override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            if (currentWindow != null)
            {
                currentWindow.MouseUp(position);
            }
        }
    }
}
