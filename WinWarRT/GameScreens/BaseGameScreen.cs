using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.GameScreens
{
    abstract class BaseGameScreen
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void MouseDown(Microsoft.Xna.Framework.Vector2 position);

        public abstract void MouseUp(Microsoft.Xna.Framework.Vector2 position);
    }
}
