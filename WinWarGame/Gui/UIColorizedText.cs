using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WinWarCS.Gui.Rendering;

namespace WinWarCS.Gui
{
   enum TextAlignHorizontal
   {
      Center,
      Left,
      Right,
   }

   internal class UIColorizedText
   {
      private string text1;
      private string hotkeyText;
      private string text2;

      internal char HotKey;
      internal string Text
      {
         get
         {
            if (string.IsNullOrEmpty(hotkeyText))
               return text1;

            return text1 + "@1" + hotkeyText + "@2" + text2;
         }
         set
         {
            ParseText(value);
         }
      }

      internal UIColorizedText(string setRawText)
      {
         ParseText(setRawText);
      }

      private void ParseText(string rawText)
      {
         HotKey = (char)0x00;

         int idx = rawText.IndexOf ("@1");
         if (idx != -1)
         {
            int idx2 = rawText.IndexOf("@2", idx + 1);
            if (idx2 != -1)
            {
               text1 = rawText.Substring(0, idx);
               hotkeyText = rawText[idx + 2].ToString();
               text2 = rawText.Substring(idx2 + 2);
            }
            else
            {
               text1 = rawText;
               hotkeyText = string.Empty;
               text2 = string.Empty;
            }
         }
         else
         {
            text1 = rawText;
            hotkeyText = string.Empty;
            text2 = string.Empty;
         }
      }

      internal Vector2 GetSize(SpriteFont font)
      {
         return font.MeasureString(text1 + hotkeyText + text2);
      }

      internal void Render(float x, float y, float width, float height, SpriteFont font, TextAlignHorizontal align, Color textColor, Color hotKeyColor)
      {
         Vector2 fullSize = font.MeasureString(text1 + hotkeyText + text2);

         Vector2 size1 = font.MeasureString(text1);
         Vector2 size2 = font.MeasureString(hotkeyText);

         Vector2 position = new Vector2(0, y - 3);
         if (height > 0)
            position.Y = y + ((float)height / 2.0f - fullSize.Y / 2.0f);

         switch (align) 
         {
            case TextAlignHorizontal.Center:
               position.X = x + ((float)width / 2.0f - fullSize.X / 2.0f);
               break;

            case TextAlignHorizontal.Left:
               position.X = x;
               break;

            case TextAlignHorizontal.Right:
               position.X = x + ((float)width - fullSize.X);
               break;
         }

         MainGame.SpriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

         FontRenderer.DrawString (font, text1, position.X, position.Y, textColor);

         if (string.IsNullOrEmpty(hotkeyText) == false)
         {
            FontRenderer.DrawString (font, hotkeyText, (position.X + size1.X), position.Y, hotKeyColor);
         }

         if (string.IsNullOrEmpty(text2) == false)
         {
            FontRenderer.DrawString (font, text2, (position.X + size1.X + size2.X), position.Y, textColor);
         }

         MainGame.SpriteBatch.End ();
      }
   }
}

