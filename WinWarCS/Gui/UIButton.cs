#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Graphics;
using WinWarCS.Gui.Rendering;
using WinWarCS.Data;

#endregion
namespace WinWarCS.Gui
{
   internal class UIButton : UIBaseComponent
   {
      #region Variables
      private WWTexture backgroundNotClicked;
      private WWTexture backgroundClicked;
      private SpriteFont font;
      private UIColorizedText colText;
      protected bool isPressed;
      #endregion

      #region Events
      internal event OnPointerDownInside OnMouseDownInside;
      internal event OnPointerUpInside OnMouseUpInside;
      #endregion

      #region Properties
      internal string Text 
      { 
         get { return colText.Text; }
         set { colText.Text = value; } 
      }
      #endregion

      #region Constructor
      internal UIButton (string setText, int releaseButtonResourceIndex, int pressedButtonResourceIndex)
      {
         colText = new UIColorizedText(setText);
         font = MainGame.DefaultFont;

         isPressed = false;

         AutoSetButtonImage (releaseButtonResourceIndex, pressedButtonResourceIndex);
      }
      #endregion

      #region AutoSetButtonImage
      private void AutoSetButtonImage (int releaseButtonResourceIndex, int pressedButtonResourceIndex)
      {
         backgroundNotClicked = WWTexture.FromImageResource(WarFile.GetImageResource(releaseButtonResourceIndex));
         backgroundClicked = WWTexture.FromImageResource(WarFile.GetImageResource(pressedButtonResourceIndex));

         Width = (int)(backgroundNotClicked.Width);
         Height = (int)(backgroundNotClicked.Height);
      }
      #endregion

      #region Render
      internal override void Draw()
      {
         base.Draw();

         WWTexture background = null;
         if (isPressed)
            background = backgroundClicked;
         else
            background = backgroundNotClicked;

         Vector2 screenPos = ScreenPosition;

         Color col = Color.FromNonPremultiplied (new Vector4 (Vector3.One, CompositeAlpha));
         background.RenderOnScreen (screenPos.X, screenPos.Y, Width, Height, col);

         colText.Render(screenPos.X, screenPos.Y, (float)Width, (float)Height, font, TextAlignHorizontal.Center, col, Color.Yellow);
      }
      #endregion

      #region MouseDown
      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (!base.PointerDown (position, pointerType))
            return false;

         isPressed = true;

         if (OnMouseDownInside != null)
            OnMouseDownInside (position);

         return true;
      }
      #endregion

      #region MouseUp
      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (!base.PointerUp (position, pointerType))
            return false;

         isPressed = false;

         if (OnMouseUpInside != null)
            OnMouseUpInside (position);

         return true;
      }
      #endregion

      #region ToString
      public override string ToString ()
      {
         return Text;
      }
      #endregion

   }
}
