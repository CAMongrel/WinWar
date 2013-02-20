// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Graphics
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 22:25

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Data;
using WinWarRT.Data.Resources;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = SharpDX.RectangleF;
using Matrix = Microsoft.Xna.Framework.Matrix;
#endregion

namespace WinWarRT.Graphics
{
   internal class WWTexture
   {
      #region Constants
      /// <summary>
      /// Create a matrix that transforms the screen space position (0..1)
      /// into device space (-1..+1)
      /// </summary>
      static readonly Matrix DeviceTransformMatrix =
         Matrix.CreateScale(2, 2, 0) *
            Matrix.CreateTranslation(-1, -1, 0) *
            Matrix.CreateScale(1, -1, 1);
      #endregion

      #region Variables
      private Texture2D DXTexture = null;

      internal int Width;
      internal int Height;
      #endregion

      #region Constructor
      private WWTexture(int width, int height)
      {
         Width = width;
         Height = height;

         DXTexture = new Texture2D(MainGame.Device, width, height, false, SurfaceFormat.Color);
      }
      #endregion

      #region FromImageResource
      internal static WWTexture FromImageResource(string name)
      {
         int idx = KnowledgeBase.IndexByName(name);
         if (idx == -1)
            return null;

         ImageResource ir = WarFile.GetImageResource(idx);
         return FromImageResource(ir);
      }

      internal static WWTexture FromImageResource(ImageResource res)
      {
         WWTexture tex = null;
         tex = new WWTexture(res.width, res.height);
         tex.DXTexture.SetData<byte>(res.image_data);

         return tex;
      }

      /// <summary>
      /// From DirectX texture
      /// </summary>
      internal static WWTexture FromDXTexture(Texture2D tex)
      {
         WWTexture res = new WWTexture(tex.Width, tex.Height);
         res.DXTexture = tex;
         return res;
      } // FromDXTexture(tex)
      #endregion

      #region RenderOnScreen
      internal void RenderOnScreen(float x, float y)
      {
         RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
            new RectangleF(x, y, x + (float)this.Width, y + (float)this.Height), Color.White);
      }

      internal void RenderOnScreen(float x, float y, Color color)
      {
         RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
            new RectangleF(x, y, x + (float)this.Width, y + (float)this.Height), color);
      }

      internal void RenderOnScreen(RectangleF display_rect)
      {
         RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
            new RectangleF(display_rect.X, display_rect.Y, display_rect.Width, display_rect.Height),
            Color.White);
      }

      internal void RenderOnScreen(float x, float y, float width, float height)
      {
         RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
            new RectangleF(x, y, x + width, y + height), Color.White);
      }

      internal void RenderOnScreen(float x, float y, float width, float height, Color color)
      {
         RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
            new RectangleF(x, y, x + width, y + height), color);
      }

      internal void RenderOnScreen(RectangleF sourceRect, RectangleF destRect)
      {
         RenderOnScreen(sourceRect, destRect, Color.White);
      }

      internal void RenderOnScreen(RectangleF sourceRect, RectangleF destRect, Color col)
      {
         Rectangle srcRect = new Rectangle((int)sourceRect.X, (int)sourceRect.Y, (int)sourceRect.Width, (int)sourceRect.Height);
         Rectangle dstRect = new Rectangle((int)(destRect.X * MainGame.ScaleX), (int)(destRect.Y * MainGame.ScaleY),
             (int)(destRect.Width * MainGame.ScaleX), (int)(destRect.Height * MainGame.ScaleY));

         MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
         MainGame.SpriteBatch.Draw(DXTexture, dstRect, srcRect, col);
         MainGame.SpriteBatch.End();
      }
      #endregion

      #region Unit testing
      internal static void TestLoadAndRender()
      {
         throw new NotImplementedException();
         /*WWTexture tex = null;
			
         TestGame.Start("TestLoadAndRender",
            delegate
            {
               WarResource res = WarFile.GetResource(243);
               WarResource pal = WarFile.GetResource(260);
               ImageResource img = new ImageResource(res, pal);
               tex = Texture.FromImageResource(img);					
            },
            delegate
            {
               tex.RenderOnScreen(0, 0, tex.Width, tex.Height);
            });
            */
      }
      #endregion
   }
}
