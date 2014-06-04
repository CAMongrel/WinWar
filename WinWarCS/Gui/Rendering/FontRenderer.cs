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
      /// </summary>
      internal static void DrawStringDirect(SpriteFont font, string text, float x, float y, Color color)
      {
         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

			MainGame.SpriteBatch.DrawString(font, text, new Microsoft.Xna.Framework.Vector2(MainGame.ScaledOffsetX + (x * MainGame.ScaleX), MainGame.ScaledOffsetY + (y * MainGame.ScaleY)), color, 0,
             Microsoft.Xna.Framework.Vector2.Zero, new Microsoft.Xna.Framework.Vector2(MainGame.ScaleX, MainGame.ScaleY), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);

         MainGame.SpriteBatch.End();
      }
   }
}
