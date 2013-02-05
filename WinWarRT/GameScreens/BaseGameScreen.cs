using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens
{
    public class BaseGameScreen
    {
        private Color standardBackgroundColor;

        public BaseGameScreen()
        {
            standardBackgroundColor = new Color(0x7F, 0x00, 0x00);
        }

        public virtual Color BackgroundColor
        {
            get
            {
                return standardBackgroundColor;
            }
        }

        public virtual void InitUI()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            UIWindowManager.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
        }

        public virtual void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
        }

        public virtual void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
        }
    }
}
