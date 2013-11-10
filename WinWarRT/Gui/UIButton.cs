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
   internal class UIButton : UIBaseComponent
   {
      #region enum ButtonType
      internal enum ButtonType
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
      private SpriteFont font;
      #endregion

      #region Events
      internal event OnPointerDownInside OnMouseDownInside;
      internal event OnPointerUpInside OnMouseUpInside;
      #endregion

      #region Properties
      internal string Text
      {
         get
         {
            return text + hotkey + text2;
         }
      }
      #endregion

      #region Constructor
      internal UIButton(string setText, ButtonType setType)
      {
         type = setType;
         font = MainGame.DefaultFont;

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
      internal override void Render()
      {
         base.Render();

         WWTexture background = null;
         if (isActive)
            background = backgroundClicked;
         else
            background = backgroundNotClicked;

         Vector2 screenPos = ScreenPosition;

         Color col = Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha));
         background.RenderOnScreen(screenPos.X, screenPos.Y, Width, Height, col);

         Microsoft.Xna.Framework.Vector2 size = font.MeasureString(text);
         Microsoft.Xna.Framework.Vector2 size2 = Microsoft.Xna.Framework.Vector2.Zero;
         if (hotkey != (char)0x00)
            size2 = font.MeasureString(hotkey.ToString());
         Microsoft.Xna.Framework.Vector2 size3 = Microsoft.Xna.Framework.Vector2.Zero;
         if (text2 != null)
            size3 = font.MeasureString(text2);

         Microsoft.Xna.Framework.Vector2 totalSize = size + size2 + size3;
         totalSize.Y = Math.Max(Math.Max(size.Y, size2.Y), size3.Y);

         Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(
             screenPos.X + ((float)Width / 2.0f - totalSize.X / 2.0f),
             screenPos.Y + ((float)Height / 2.0f - totalSize.Y / 2.0f));

         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

         MainGame.SpriteBatch.DrawString(font, text, new Microsoft.Xna.Framework.Vector2(position.X * MainGame.ScaleX, position.Y * MainGame.ScaleY), col, 0,
             Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

         if (hotkey != (char)0x00)
         {
            MainGame.SpriteBatch.DrawString(font, hotkey.ToString(), new Microsoft.Xna.Framework.Vector2((position.X + size.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), col, 0,
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
         }

         if (text2 != null)
         {
            MainGame.SpriteBatch.DrawString(font, text2, new Microsoft.Xna.Framework.Vector2((position.X + size.X + size2.X) * MainGame.ScaleX, position.Y * MainGame.ScaleY), col, 0,
                Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
         }

         MainGame.SpriteBatch.End();
      }
      #endregion

      #region MouseDown
      internal override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
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
      internal override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
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
