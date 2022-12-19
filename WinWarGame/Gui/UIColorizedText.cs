using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WinWarCS.Gui.Rendering;
using System.Collections.Generic;

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
                {
                    return text1;
                }

                return text1 + "@1" + hotkeyText + "@2" + text2;
            }
            set
            {
                ParseText(value);
            }
        }

        internal string UnformattedText
        {
            get
            {
                return text1 + hotkeyText + text2;
            }
        }

        internal UIColorizedText(string setRawText)
        {
            ParseText(setRawText);
        }

        private void ParseText(string rawText)
        {
            HotKey = (char)0x00;

            int idx = rawText.IndexOf("@1");
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

        internal string[] WrapLines(SpriteFont font, string fullText, float maxWidth)
        {
            List<string> result = new List<string>();

            string[] paragraphs = fullText.Split("\r\n");

            foreach (string p in paragraphs)
            {
                string[] words = p.Split(' ');
                string curLine = "";
                float lineWidth = 0.0f;
                float spaceWidth = font.MeasureString(" ").X;

                for (int i = 0; i < words.Length; i++)
                {
                    Vector2 size = font.MeasureString(words[i]);
                    if (lineWidth > 0.0f)
                    {
                        if (lineWidth + size.X > maxWidth)
                        {
                            result.Add(curLine);
                            lineWidth = 0.0f;
                            curLine = "";
                        }
                    }

                    lineWidth += size.X + spaceWidth;
                    curLine += words[i] + " ";
                }

                if (string.IsNullOrWhiteSpace(curLine) == false)
                {
                    result.Add(curLine);
                }
            }

            return result.ToArray();
        }

        internal void RenderMultiLineUnformatted(float x, float y, float width, float height,
            SpriteFont font, TextAlignHorizontal align, Color textColor)
        {
            string[] lines = WrapLines(font, UnformattedText, width);
            if (lines.Length == 0)
            {
                return;
            }

            float lineHeight = font.MeasureString(lines[0]).Y;
            float totalHeight = lineHeight * lines.Length;

            float startY = (height * 0.5f) - (totalHeight * 0.5f);
            float curY = startY;

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap,
                DepthStencilState.None, RasterizerState.CullCounterClockwise);

            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 position = new Vector2(0, curY);
                Vector2 fullSize = font.MeasureString(lines[i]);

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

                FontRenderer.DrawString(font, lines[i], position.X, position.Y, textColor);

                curY += lineHeight;
            }

            MainGame.SpriteBatch.End();
        }

        internal void Render(float x, float y, float width, float height, SpriteFont font,
            TextAlignHorizontal align, Color textColor, Color hotKeyColor)
        {
            Vector2 fullSize = GetSize(font);

            Vector2 size1 = font.MeasureString(text1);
            Vector2 size2 = font.MeasureString(hotkeyText);

            Vector2 position = new Vector2(0, y - 3);
            if (height > 0)
            {
                position.Y = y + ((float)height / 2.0f - fullSize.Y / 2.0f);
            }

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

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap,
                DepthStencilState.None, RasterizerState.CullCounterClockwise);

            FontRenderer.DrawString(font, text1, position.X, position.Y, textColor);

            if (string.IsNullOrEmpty(hotkeyText) == false)
            {
                FontRenderer.DrawString(font, hotkeyText, (position.X + size1.X), position.Y, hotKeyColor);
            }

            if (string.IsNullOrEmpty(text2) == false)
            {
                FontRenderer.DrawString(font, text2, (position.X + size1.X + size2.X), position.Y, textColor);
            }

            MainGame.SpriteBatch.End();
        }
    }
}

