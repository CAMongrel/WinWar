using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.GameScreens
{
    public abstract class BaseGameScreen
    {
        public abstract void InitUI();

        public abstract void Close();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void PointerDown(Microsoft.Xna.Framework.Vector2 position);

        public abstract void PointerUp(Microsoft.Xna.Framework.Vector2 position);
    }
}
