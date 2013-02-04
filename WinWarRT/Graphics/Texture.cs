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
	public class WWTexture
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

		public int Width;
		public int Height;
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
		public static WWTexture FromImageResource(string name)
		{
			int idx = KnowledgeBase.IndexByName(name);
			if (idx == -1)
				return null;

			ImageResource ir = WarFile.GetImageResource(idx);
			return FromImageResource(ir);
		}

		public static WWTexture FromImageResource(ImageResource res)
		{
			WWTexture tex = null;
			tex = new WWTexture(res.width, res.height);
			tex.DXTexture.SetData<byte>(res.image_data);

			return tex;
		}
		
		/// <summary>
		/// From DirectX texture
		/// </summary>
		public static WWTexture FromDXTexture(Texture2D tex)
		{
			WWTexture res = new WWTexture(tex.Width, tex.Height);
			res.DXTexture = tex;
			return res;
		} // FromDXTexture(tex)
		#endregion

		#region RenderOnScreen
		public void RenderOnScreen(float x, float y)
		{
			RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
				new RectangleF(x, y, x + (float)this.Width, y + (float)this.Height), Color.White);
		}

		public void RenderOnScreen(RectangleF display_rect)
		{
			RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
				new RectangleF(display_rect.X, display_rect.Y, display_rect.Width, display_rect.Height), 
				Color.White);
		}

		public void RenderOnScreen(float x, float y, float width, float height)
		{
			RenderOnScreen(new RectangleF(0, 0, (float)this.Width, (float)this.Height),
				new RectangleF(x, y, x + width, y + height), Color.White);
		}

        public void RenderOnScreen(RectangleF sourceRect, RectangleF destRect)
		{
			RenderOnScreen(sourceRect, destRect, Color.White);
		}

        public void RenderOnScreen(RectangleF sourceRect, RectangleF destRect, Color col)
		{
            Rectangle srcRect = new Rectangle((int)sourceRect.X, (int)sourceRect.Y, (int)sourceRect.Width, (int)sourceRect.Height);
            Rectangle dstRect = new Rectangle((int)(destRect.X * MainGame.ScaleX), (int)(destRect.Y * MainGame.ScaleY), 
                (int)(destRect.Width * MainGame.ScaleX), (int)(destRect.Height * MainGame.ScaleY));

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            MainGame.SpriteBatch.Draw(DXTexture, dstRect, srcRect, col);
            MainGame.SpriteBatch.End();
            /*return;

			if (vertexBuffer == null)
				return;

			// Convert to UV (0..1)
			sourceRect.Left /= this.Width;
			sourceRect.Top /= this.Height;
			sourceRect.Right /= this.Width;
			sourceRect.Bottom /= this.Height;

			// Convert to normalized screen space (0..1)
            destRect.Left /= MainGame.ResolutionRect.Width;
            destRect.Top /= MainGame.ResolutionRect.Height;
            destRect.Right /= MainGame.ResolutionRect.Width;
            destRect.Bottom /= MainGame.ResolutionRect.Height;

			// Create the position matrix that scales and translates the 
			// vertex positions to the correct offsets and dimensions.
			Matrix posMatrix =
				// Apply scaling to correct width and height
				Matrix.CreateScale(destRect.Width, destRect.Height, 0) *
				// Now move to the final position on screen.
				Matrix.CreateTranslation(destRect.X, destRect.Y, 0);

			// Create the tex coord matrix that scales and translates the 
			// UV coordinates of the vertices to the correct offsets and 
			// dimensions.
			Matrix texMatrix =
				Matrix.CreateScale(sourceRect.Width, sourceRect.Height, 0) *
				Matrix.CreateTranslation(sourceRect.X, sourceRect.Y, 0);

			// Set the common screen space -> device space matrix
			transformMatrix.SetValue(DeviceTransformMatrix);
			// Set matrices
			positionMatrix.SetValue(posMatrix);
			texcoordMatrix.SetValue(texMatrix);

			// Set color and texture
			color.SetValue(new Vector4(1, 1, 1, 1));
			diffuseTexture.SetValue(DXTexture);*/

            //throw new NotImplementedException();
            /*
			BaseGame.device.Vertices[0].SetSource(
				vertexBuffer, 0, ScreenVertex.SizeOf);
			BaseGame.device.VertexDeclaration = ScreenVertex.VertexDecl;

			primitiveEffect.Begin();
			// Start the pass
			primitiveEffect.CurrentTechnique.Passes[0].Begin();

			// Render quad
			BaseGame.device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

			// End pass
			primitiveEffect.CurrentTechnique.Passes[0].End();
			primitiveEffect.End();
             * */
		}
		#endregion

		#region Unit testing
		public static void TestLoadAndRender()
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
