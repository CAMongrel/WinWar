#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Graphics;
#endregion

namespace WinWarRT.Gui
{
	public class UIButton : UIBaseComponent
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

        #region Events
        public event OnPointerDownInside OnMouseDownInside;
        public event OnPointerUpInside OnMouseUpInside;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return text + hotkey + text2;
            }
        }
        #endregion

        #region Constructor
        public UIButton(string setText, ButtonType setType)
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

            Vector2 screenPos = ScreenPosition;

            background.RenderOnScreen(screenPos.X, screenPos.Y, Width, Height);

            Microsoft.Xna.Framework.Vector2 size = MainGame.SpriteFont.MeasureString(text);
            Microsoft.Xna.Framework.Vector2 size2 = Microsoft.Xna.Framework.Vector2.Zero;
            if (hotkey != (char)0x00)
                size2 = MainGame.SpriteFont.MeasureString(hotkey.ToString());
            Microsoft.Xna.Framework.Vector2 size3 = Microsoft.Xna.Framework.Vector2.Zero; 
            if (text2 != null)
                size3 = MainGame.SpriteFont.MeasureString(text2);

            Microsoft.Xna.Framework.Vector2 totalSize = size + size2 + size3;
            totalSize.Y = Math.Max(Math.Max(size.Y, size2.Y), size3.Y);

            Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(
                screenPos.X + ((float)Width / 2.0f - totalSize.X / 2.0f),
                screenPos.Y + ((float)Height / 2.0f - totalSize.Y / 2.0f));

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, text, new Microsoft.Xna.Framework.Vector2(position.X * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0, 
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

            if (hotkey != (char)0x00)
            {
                MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, hotkey.ToString(), new Microsoft.Xna.Framework.Vector2((position.X + size.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0,
                    Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
            }

            if (text2 != null)
            {
                MainGame.SpriteBatch.DrawString(MainGame.SpriteFont, text2, new Microsoft.Xna.Framework.Vector2((position.X + size.X + size2.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), Microsoft.Xna.Framework.Color.White, 0,
                    Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
            }

            MainGame.SpriteBatch.End();
		}
		#endregion

        #region MouseDown
        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (!base.PointerDown(position))
                return false;

            isActive = true;

            if (OnMouseDownInside != null)
                OnMouseDownInside(position);

            return true;
        }
        #endregion

        #region MouseUp
        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
		{
            if (!base.PointerUp(position))
				return false;

            isActive = false;

            if (OnMouseUpInside != null)
                OnMouseUpInside(position);

			return true;
		}
		#endregion

        #region ToString
        public override string ToString()
        {
            return Text;
        }
        #endregion
    }
}
