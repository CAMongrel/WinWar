﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Gui.Rendering;
using WinWarGame.Graphics;

namespace WinWarGame.Gui
{
    class UILabel : UIBaseComponent
    {
        #region Variables
        private UIColorizedText colText;
        internal string Text
        {
            get { return colText.Text; }
            set
            {
                colText.Text = value;
            }
        }
        private Font font;

        #endregion

        #region Properties
        internal TextAlignHorizontal TextAlign { get; set; }

        internal bool IsUnformattedText { get; set; } = false;
        #endregion

        #region Constructor
        internal UILabel(string setText)
        {
            colText = new UIColorizedText(setText);
            font = MainGame.DefaultFont;
            TextAlign = TextAlignHorizontal.Center;
        }
        #endregion

        #region Render
        internal override void Draw()
        {
            base.Draw();

            Vector2 screenPos = ScreenPosition;

            Color col = Color.FromNonPremultiplied(new Vector4(Vector3.One, CompositeAlpha));
            Color hotKeyCol = Color.Yellow;

            if (IsUnformattedText)
            {
                colText.RenderMultiLineUnformatted(screenPos.X, screenPos.Y, (float)Width, (float)Height, font, TextAlign, col);
            }
            else
            {
                colText.Render(screenPos.X, screenPos.Y, (float)Width, (float)Height, font, TextAlign, col, hotKeyCol);
            }
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
