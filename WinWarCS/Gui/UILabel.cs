using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Gui.Rendering;

namespace WinWarCS.Gui
{
   class UILabel : UIBaseComponent
   {

      #region Variables

      private string text;
      private SpriteFont font;

      #endregion

      #region Constructor

      internal UILabel (string setText)
      {
         text = setText;
         font = MainGame.DefaultFont;
      }

      #endregion

      #region Render

      internal override void Render ()
      {
         Vector2 screenPos = ScreenPosition;

         Color col = Color.FromNonPremultiplied (new Vector4 (Vector3.One, CompositeAlpha));
         Microsoft.Xna.Framework.Vector2 size = font.MeasureString (text);

         Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2 (
                                                       screenPos.X + ((float)Width / 2.0f - size.X / 2.0f),
                                                       screenPos.Y + ((float)Height / 2.0f - size.Y / 2.0f));

         FontRenderer.DrawStringDirect (font, text, position.X, position.Y, col);

         base.Render ();
      }

      #endregion

      #region ToString

      public override string ToString ()
      {
         return text;
      }

      #endregion

   }
}
