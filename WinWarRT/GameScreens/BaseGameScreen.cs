using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens
{
    internal class BaseGameScreen
    {
        private Color standardBackgroundColor;

        internal BaseGameScreen()
        {
            standardBackgroundColor = new Color(0x7F, 0x00, 0x00);
        }

        internal virtual Color BackgroundColor
        {
            get
            {
                return standardBackgroundColor;
            }
        }

        internal virtual void InitUI()
        {
        }

        internal virtual void Close()
        {
        }

        internal virtual void Update(GameTime gameTime)
        {
            UIWindowManager.Update(gameTime);
        }

        internal virtual void Draw(GameTime gameTime)
        {
        }

        internal virtual void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
        }

        internal virtual void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
        }

        internal virtual void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
        }
    }
}
