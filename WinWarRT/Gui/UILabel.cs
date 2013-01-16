using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui
{
    class UILabel : UIBaseComponent
    {
        #region Variables
        private string text;
        #endregion

        #region Constructor
        public UILabel(string setText)
		{
            text = setText;
        }
        #endregion

        #region Render
        public override void Render()
        {
            base.Render();

            Microsoft.Xna.Framework.Vector2 size = MainGame.SpriteFont.MeasureString(text);

            Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(
                X + ((float)Width / 2.0f - size.X / 2.0f),
                Y + ((float)Height / 2.0f - size.Y / 2.0f));

            MainGame.SpriteBatch.Begin();

            MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, text, new Microsoft.Xna.Framework.Vector2(position.X * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0,
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

            MainGame.SpriteBatch.End();
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return text;
        }
        #endregion
    }
}
