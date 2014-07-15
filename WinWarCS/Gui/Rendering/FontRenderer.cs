using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Gui.Rendering
{
   internal static class FontRenderer
   {
      /// <summary>
      /// Draws a text at the specified position without a begin/end. Suitable for multiple draw events.
      /// </summary>
      internal static void DrawString(SpriteFont font, string text, float x, float y, Color color)
      {
			MainGame.SpriteBatch.DrawString(font, text, new Microsoft.Xna.Framework.Vector2(MainGame.ScaledOffsetX + (x * MainGame.ScaleX), MainGame.ScaledOffsetY + (y * MainGame.ScaleY)), color, 0,
             Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
      }

      /// <summary>
      /// Draws a text at the specified position nested in a begin/end. Suitable for quick single line drawing.
      /// Uses default scaling
      /// </summary>
      internal static void DrawStringDirect(SpriteFont font, string text, float x, float y, Color color)
      {
         DrawStringDirect (font, text, x, y, color, 1.0f);
      }

      /// <summary>
      /// Draws a text at the specified position nested in a begin/end. Suitable for quick single line drawing.
      /// </summary>
      internal static void DrawStringDirect(SpriteFont font, string text, float x, float y, Color color, float scale)
      {
         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

			MainGame.SpriteBatch.DrawString(font, 
            text, 
            new Microsoft.Xna.Framework.Vector2(MainGame.ScaledOffsetX + (x * MainGame.ScaleX), MainGame.ScaledOffsetY + (y * MainGame.ScaleY)), 
            color, 
            0,
            Microsoft.Xna.Framework.Vector2.Zero, 
            new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX * scale, MainGame.ScaleY * scale), 
            Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 
            1.0f);

         MainGame.SpriteBatch.End();
      }

      /// <summary>
      /// Draws a text at the specified position nested in a begin/end. Suitable for quick text drawing.
      /// </summary>
      internal static void DrawStringDirect(SpriteFont font, string text, float x, float y, float width, float height, Color color)
      {
         string[] lines = text.Split ('\n');

         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

         float yGap = -2;
         float yPos = y;
         for (int i = 0; i < lines.Length; i++)
         {
            Vector2 lineSize = font.MeasureString (lines [i]);
            float xPos = x + (width / 2 - lineSize.X / 2);

            MainGame.SpriteBatch.DrawString(font, 
               lines [i], 
               new Microsoft.Xna.Framework.Vector2(MainGame.ScaledOffsetX + (xPos * MainGame.ScaleX), MainGame.ScaledOffsetY + (yPos * MainGame.ScaleY)), 
               color,
               0,
               Microsoft.Xna.Framework.Vector2.Zero, 
               new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), 
               Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 
               1.0f);

            yPos += font.LineSpacing + yGap;
         }

         MainGame.SpriteBatch.End();
      }
   }
}
