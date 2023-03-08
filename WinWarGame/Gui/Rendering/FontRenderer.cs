using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Graphics;

namespace WinWarCS.Gui.Rendering
{
    internal static class FontRenderer
    {
        /// <summary>
        /// Draws a text at the specified position without a begin/end. Suitable for multiple draw events.
        /// </summary>
        internal static void DrawString(Font font, string text, float x, float y, Color color)
        {
            font.DrawIntoSpriteBatch(MainGame.SpriteBatch, text,
                new Vector2(MainGame.ScaledOffsetX + (x * MainGame.ScaleX), MainGame.ScaledOffsetY + (y * MainGame.ScaleY)), 
                color, 0, Vector2.Zero, 
                new Vector2(MainGame.ScaleX, MainGame.ScaleY), SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Draws a text at the specified position nested in a begin/end. Suitable for quick single line drawing.
        /// Uses default scaling
        /// </summary>
        internal static void DrawStringDirect(Font font, string text, float x, float y, Color color)
        {
            DrawStringDirect(font, text, x, y, color, 1.0f);
        }

        /// <summary>
        /// Draws a text at the specified position nested in a begin/end. Suitable for quick single line drawing.
        /// </summary>
        internal static void DrawStringDirect(Font font, string text, float x, float y, Color color, float scale)
        {
            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            font.DrawIntoSpriteBatch(MainGame.SpriteBatch,
                text,
                new Vector2(MainGame.ScaledOffsetX + (x * MainGame.ScaleX), MainGame.ScaledOffsetY + (y * MainGame.ScaleY)),
                color,
                0,
                Vector2.Zero,
                new Vector2(MainGame.ScaleX * scale, MainGame.ScaleY * scale),
                SpriteEffects.None,
                1.0f);

            MainGame.SpriteBatch.End();
        }

        /// <summary>
        /// Draws a text at the specified position nested in a begin/end. Suitable for quick text drawing.
        /// </summary>
        internal static void DrawStringDirect(Font font, string text, float x, float y, float width, float height, Color color)
        {
            string[] lines = text.Split('\n');

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            float yGap = -2;
            float yPos = y;
            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 lineSize = font.MeasureString(lines[i]);
                float xPos = x + (width / 2 - lineSize.X / 2);

                font.DrawIntoSpriteBatch(MainGame.SpriteBatch,
                   lines[i],
                   new Vector2(MainGame.ScaledOffsetX + (xPos * MainGame.ScaleX), MainGame.ScaledOffsetY + (yPos * MainGame.ScaleY)),
                   color,
                   0,
                   Vector2.Zero,
                   new Vector2(MainGame.ScaleX, MainGame.ScaleY),
                   SpriteEffects.None,
                   1.0f);

                yPos += font.LineSpacing + yGap;
            }

            MainGame.SpriteBatch.End();
        }
    }
}
