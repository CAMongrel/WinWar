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
using WinWarCS.Data;
using WinWarCS.Data.Resources;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = System.Drawing.RectangleF;
using Matrix = Microsoft.Xna.Framework.Matrix;

#endregion
namespace WinWarCS.Graphics
{
   internal class WWTexture
   {
      #region Constants

      /// <summary>
      /// Create a matrix that transforms the screen space position (0..1)
      /// into device space (-1..+1)
      /// </summary>
      static readonly Matrix DeviceTransformMatrix =
         Matrix.CreateScale (2, 2, 0) *
         Matrix.CreateTranslation (-1, -1, 0) *
         Matrix.CreateScale (1, -1, 1);

      #endregion

      #region Variables

      private Texture2D DXTexture = null;
      internal int Width;
      internal int Height;

      #endregion

      #region Constructor

      private WWTexture (int width, int height)
      {
         Width = width;
         Height = height;

         DXTexture = new Texture2D (MainGame.Device, width, height, false, SurfaceFormat.Color);
      }

      #endregion

      #region FromImageResource

      internal static WWTexture FromImageResource (string name)
      {
         int idx = KnowledgeBase.IndexByName (name);
         if (idx == -1)
            return null;

         ImageResource ir = WarFile.GetImageResource (idx);
         return FromImageResource (ir);
      }

      internal static WWTexture FromImageResource (ImageResource res)
      {
         WWTexture tex = null;
         tex = new WWTexture (res.width, res.height);
         tex.DXTexture.SetData<byte> (res.image_data);

         return tex;
      }

      /// <summary>
      /// From DirectX texture
      /// </summary>
      internal static WWTexture FromDXTexture (Texture2D tex)
      {
         WWTexture res = new WWTexture (tex.Width, tex.Height);
         res.DXTexture = tex;
         return res;
      }
      // FromDXTexture(tex)

      #endregion

      #region RenderOnScreen

      internal void RenderOnScreen (float x, float y)
      {
         RenderOnScreen (x, y, Color.White);
      }

      internal void RenderOnScreen (float x, float y, Color color)
      {
         RenderOnScreen (new RectangleF (0, 0, (float)this.Width, (float)this.Height),
			#if ABSOLUTE_COORDS
				new RectangleF(x, y, x + (float)this.Width, y + (float)this.Height), color);
			#else
            new RectangleF (x, y, (float)this.Width, (float)this.Height), color);
         #endif
      }

      internal void RenderOnScreen (RectangleF display_rect)
      {
         RenderOnScreen (new RectangleF (0, 0, (float)this.Width, (float)this.Height),
            new RectangleF (display_rect.X, display_rect.Y, display_rect.Width, display_rect.Height),
            Color.White);
      }

      internal void RenderOnScreen (float x, float y, float width, float height)
      {
         RenderOnScreen (x, y, width, height, Color.White);
      }

      internal void RenderOnScreen (float x, float y, float width, float height, Color color)
      {
         RenderOnScreen (new RectangleF (0, 0, (float)this.Width, (float)this.Height),
			#if ABSOLUTE_COORDS
				new RectangleF(x, y, x + width, y + height), color);
			#else
            new RectangleF (x, y, width, height), color);
         #endif
      }

      internal void RenderOnScreen (RectangleF sourceRect, RectangleF destRect)
      {
         RenderOnScreen (sourceRect, destRect, Color.White);
      }

      internal void RenderOnScreen (RectangleF sourceRect, RectangleF destRect, Color col)
      {
         Rectangle srcRect = new Rectangle ((int)sourceRect.X, (int)sourceRect.Y, (int)sourceRect.Width, (int)sourceRect.Height);
         Rectangle dstRect = new Rectangle (MainGame.ScaledOffsetX + (int)(destRect.X * MainGame.ScaleX), MainGame.ScaledOffsetY + (int)(destRect.Y * MainGame.ScaleY),
                          (int)(destRect.Width * MainGame.ScaleX), (int)(destRect.Height * MainGame.ScaleY));

         MainGame.SpriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
         MainGame.SpriteBatch.Draw (DXTexture, dstRect, srcRect, col);
         MainGame.SpriteBatch.End ();
      }

      #endregion

      #region WriteToFile
      internal void WriteToFile(string fullFilename)
      {
         byte[] outputData = new byte[Width * Height * 4];
         DXTexture.GetData<byte> (outputData);

         int scale = 1;

         System.Drawing.Bitmap bm = new System.Drawing.Bitmap (Width * scale, Height * scale, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
         for (int y = 0; y < Height; y++)
         {
            for (int x = 0; x < Width; x++)
            {
               int pixelIndex = (x + y * Width) * 4;

               System.Drawing.Color col = 
                  System.Drawing.Color.FromArgb (0xFF, outputData [pixelIndex + 0], outputData [pixelIndex + 1], outputData [pixelIndex + 2]);

               for (int y2 = 0; y2 < scale; y2++)
                  for (int x2 = 0; x2 < scale; x2++)
                     bm.SetPixel (x * scale + x2, y * scale + y2, col);
            }
         }
         bm.Save (fullFilename, System.Drawing.Imaging.ImageFormat.Png);
      }
      #endregion

      #region Unit testing

      internal static void TestLoadAndRender ()
      {
         throw new NotImplementedException ();
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
