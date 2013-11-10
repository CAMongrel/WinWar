using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
      private SpriteFont font;
      #endregion

      #region Constructor
      internal UILabel(string setText)
      {
         text = setText;
         font = MainGame.DefaultFont;
      }
      #endregion

      #region Render
      internal override void Render()
      {
         base.Render();

         Vector2 screenPos = ScreenPosition;

         Color col = Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha));
         Microsoft.Xna.Framework.Vector2 size = font.MeasureString(text);

         Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(
             screenPos.X + ((float)Width / 2.0f - size.X / 2.0f),
             screenPos.Y + ((float)Height / 2.0f - size.Y / 2.0f));

         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

         MainGame.SpriteBatch.DrawString(font, text, new Microsoft.Xna.Framework.Vector2(position.X * MainGame.ScaleX, position.Y * MainGame.ScaleY), col, 0,
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
