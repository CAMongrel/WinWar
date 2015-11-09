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
#if !NETFX_CORE
using RectangleF = System.Drawing.RectangleF;
#else
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#endif
using Matrix = Microsoft.Xna.Framework.Matrix;

#endregion
namespace WinWarCS.Graphics
{
   internal class WWTexture : IDisposable
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

      private static WWTexture singleWhite;

      #endregion

      #region Static properties
      public static WWTexture SingleWhite
      {
         get
         {
            if (singleWhite != null)
               return singleWhite;

            singleWhite = new WWTexture (1, 1);
            singleWhite.DXTexture.SetData<byte> (new byte[] { 255, 255, 255, 255 });
            return singleWhite;
         }
      }
      #endregion

      #region Constructor

      private WWTexture (int width, int height)
      {
         Width = width;
         Height = height;

         if (RequiresPowerOfTwo ()) 
         {
            width = (int)NextPowerOfTwo ((uint)width);
            height = (int)NextPowerOfTwo ((uint)height);
         }

         DXTexture = new Texture2D (MainGame.Device, width, height, false, SurfaceFormat.Color);
      }

      #endregion

      #region IDisposable implementation

      public void Dispose ()
      {
         if (DXTexture != null)
         {
            DXTexture.Dispose ();
            DXTexture = null;
         }
      }

      #endregion

      #region Utility
      private uint NextPowerOfTwo(uint val)
      {
         // Taken from http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
         val--;
         val |= val >> 1;
         val |= val >> 2;
         val |= val >> 4;
         val |= val >> 8;
         val |= val >> 16;
         val++;
         return val;
      }

      private bool RequiresPowerOfTwo()
      {
         #if IOS
         return true;
         #else
         return false;
         #endif
      }
      #endregion

      #region FromImageResource

      internal static WWTexture FromImageResource (string name)
      {
         int idx = WarFile.KnowledgeBase.IndexByName (name);
         if (idx == -1)
            return null;

         ImageResource ir = WarFile.GetImageResource (idx);
         return FromImageResource (ir);
      }

      internal static WWTexture FromImageResource (ImageResource res)
      {
         return FromRawData (res.width, res.height, res.image_data);
      }

      internal static WWTexture FromCursorResource (CursorResource res)
      {
         return FromRawData (res.width, res.height, res.image_data);
      }

      internal static WWTexture FromRawData (int width, int height, byte[] data)
      {
         WWTexture tex = new WWTexture (width, height);
         tex.SetData (data);
         return tex;
      }

      #endregion

      #region SetData
      public void SetData(byte[] data)
      {
         if (data == null)
         {
            return;
         }

         if (RequiresPowerOfTwo () == false) 
         {
            DXTexture.SetData<byte> (data);
         } else 
         {
            DXTexture.SetData<byte> (0, new Rectangle(0, 0, this.Width, this.Height), data, 0, data.Length);
         }
      }

      public void SetData(Color[] data)
      {
         if (data == null)
         {
            return;
         }

         if (RequiresPowerOfTwo () == false) 
         {
            DXTexture.SetData<Color> (data);
         } else 
         {
            DXTexture.SetData<Color> (0, new Rectangle(0, 0, this.Width, this.Height), data, 0, data.Length);
         }
      }
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
         RenderOnScreen (display_rect, false, false);
      }

      internal void RenderOnScreen (RectangleF display_rect, bool flipX, bool flipY)
      {
         RectangleF sourceRect = new RectangleF (
                                    flipX ? (float)this.Width : 0, 
                                    flipY ? (float)this.Height : 0, 
                                    flipX ? -(float)this.Width : (float)this.Width, 
                                    flipY ? -(float)this.Height : (float)this.Height);

         RenderOnScreen (sourceRect,
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
         Vector2 position = new Vector2 (MainGame.ScaledOffsetX + destRect.X * MainGame.ScaleX,
                               MainGame.ScaledOffsetY + destRect.Y * MainGame.ScaleY);
         Vector2 scale = new Vector2 (MainGame.ScaleX, MainGame.ScaleY);

         if (RenderManager.IsBatching == false)
            MainGame.SpriteBatch.Begin (SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
         
         MainGame.SpriteBatch.Draw (DXTexture, position, srcRect, col, 0, Vector2.Zero, 
            scale, SpriteEffects.None, 1.0f);
         
         if (RenderManager.IsBatching == false)
            MainGame.SpriteBatch.End ();
      }

      internal static void RenderRectangle (RectangleF destRect, Color col, int BorderWidth = 1)
      {
         destRect = new RectangleF (MainGame.ScaledOffsetX + (int)(destRect.X * MainGame.ScaleX), MainGame.ScaledOffsetY + (int)(destRect.Y * MainGame.ScaleY),
            (int)(destRect.Width * MainGame.ScaleX), (int)(destRect.Height * MainGame.ScaleY));

         Rectangle singleRect = new Rectangle (0, 0, 1, 1);
         Rectangle leftRect = new Rectangle((int)destRect.X, (int)destRect.Y, BorderWidth, (int)destRect.Height);
         Rectangle topRect = new Rectangle((int)destRect.X, (int)destRect.Y, (int)destRect.Width, BorderWidth);
         Rectangle rightRect = new Rectangle((int)destRect.X + (int)destRect.Width - BorderWidth, (int)destRect.Y, BorderWidth, (int)destRect.Height);
         Rectangle bottomRect = new Rectangle((int)destRect.X, (int)destRect.Y + (int)destRect.Height - BorderWidth, (int)destRect.Width, BorderWidth);

         if (RenderManager.IsBatching == false)
            MainGame.SpriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
         MainGame.SpriteBatch.Draw (WWTexture.SingleWhite.DXTexture, leftRect, singleRect, col);
         MainGame.SpriteBatch.Draw (WWTexture.SingleWhite.DXTexture, topRect, singleRect, col);
         MainGame.SpriteBatch.Draw (WWTexture.SingleWhite.DXTexture, rightRect, singleRect, col);
         MainGame.SpriteBatch.Draw (WWTexture.SingleWhite.DXTexture, bottomRect, singleRect, col);
         if (RenderManager.IsBatching == false)
            MainGame.SpriteBatch.End ();
      }

      #endregion

      #region WriteToFile
#if !NETFX_CORE && !IOS
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
#endif
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
