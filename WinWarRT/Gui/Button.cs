#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Graphics;
#endregion

namespace WinWarRT.Gui
{
	public class Button : BaseComponent
	{
		#region enum ButtonType
		public enum ButtonType
		{
			SmallButton,
			MediumButton,
			LargeButton,
		}
		#endregion

		#region Variables
        private string text;
        private string text2;
        private char hotkey;
        private ButtonType type;
        private WWTexture backgroundNotClicked;
        private WWTexture backgroundClicked;
        private bool isActive;
		#endregion

		#region Constructor
		public Button(string setText, ButtonType setType)
		{
			type = setType;

            isActive = false;

			switch (type)
			{
				case ButtonType.SmallButton:
					backgroundNotClicked = WWTexture.FromImageResource("Small Button");
					backgroundClicked = WWTexture.FromImageResource("Small Button (Clicked)");
					break;
				case ButtonType.MediumButton:
					backgroundNotClicked = WWTexture.FromImageResource("Medium Button");
					backgroundClicked = WWTexture.FromImageResource("Medium Button (Clicked)");
					break;
				case ButtonType.LargeButton:
					backgroundNotClicked = WWTexture.FromImageResource("Large Button");
					backgroundClicked = WWTexture.FromImageResource("Large Button (Clicked)");
					break;
			}

			Width = (int)(backgroundNotClicked.Width);
            Height = (int)(backgroundNotClicked.Height);

			hotkey = (char)0x00;

			int idx = setText.IndexOf("@1");
			if (idx != -1)
			{
				int idx2 = setText.IndexOf("@2", idx + 1);
				if (idx2 != -1)
				{
					text = setText.Substring(0, idx);
					hotkey = setText[idx + 2];
					text2 = setText.Substring(idx2 + 2);

					return;
				}
			}
			
			text = setText;
		}
		#endregion

		#region Render
		public override void Render()
		{
			base.Render();

            WWTexture background = null;
            if (isActive)
                background = backgroundClicked;
            else
                background = backgroundNotClicked;

            background.RenderOnScreen(X, Y, Width, Height);

            Microsoft.Xna.Framework.Vector2 size = MainGame.SpriteFont.MeasureString(text);
            Microsoft.Xna.Framework.Vector2 size2 = MainGame.SpriteFont.MeasureString(hotkey.ToString());
            Microsoft.Xna.Framework.Vector2 size3 = MainGame.SpriteFont.MeasureString(text2);

            Microsoft.Xna.Framework.Vector2 totalSize = size + size2 + size3;
            totalSize.Y = Math.Max(Math.Max(size.Y, size2.Y), size3.Y);

            Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(
                X + ((float)Width / 2.0f - totalSize.X / 2.0f), 
                Y + ((float)Height / 2.0f - totalSize.Y / 2.0f));

            MainGame.SpriteBatch.Begin();

            MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, text, new Microsoft.Xna.Framework.Vector2(position.X * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0, 
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

            MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, hotkey.ToString(), new Microsoft.Xna.Framework.Vector2((position.X + size.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0,
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

            MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, text2, new Microsoft.Xna.Framework.Vector2((position.X + size.X + size2.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0,
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

            MainGame.SpriteBatch.End();
		}
		#endregion

        #region MouseDown
        public override bool MouseDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (!base.MouseDown(position))
                return false;

            isActive = true;

            return true;
        }
        #endregion

        #region MouseUp
        public override bool MouseUp(Microsoft.Xna.Framework.Vector2 position)
		{
            if (!base.MouseUp(position))
				return false;

            isActive = false;

			return true;
		}
		#endregion
	}
}
