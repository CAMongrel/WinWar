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
        internal static MenuGameScreen Menu { get; private set; }

        internal MenuGameScreen()
        {
            Menu = this;
        }

        internal override void InitUI()
        {
            new MenuBackgroundWindow();
            new MainMenuWindow();
        }

        internal override void Close()
        {
            UIWindowManager.Clear();
        }

        internal override void Draw(GameTime gameTime)
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

        internal override void PointerMoved(Vector2 position)
        {
            UIWindowManager.PointerMoved(position);
        }
    }
}
