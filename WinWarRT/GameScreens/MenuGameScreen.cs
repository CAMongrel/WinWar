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

        public Window CurrentWindow;

        public MenuGameScreen()
        {
            Menu = this;

            backgroundWindow = new MenuBackgroundWindow();
            CurrentWindow = Window.FromTextResource("Main Menu Text");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            backgroundWindow.Render();

            if (CurrentWindow != null)
            {
                CurrentWindow.Render();
            }
        }

        public override void MouseDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (CurrentWindow != null)
            {
                CurrentWindow.MouseDown(position);
            }
        }

        public override void MouseUp(Microsoft.Xna.Framework.Vector2 position)
        {
            if (CurrentWindow != null)
            {
                CurrentWindow.MouseUp(position);
            }
        }
    }
}
