// Author: Henning
// Project: WinWarCS
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWar\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 22:25

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion

namespace WinWarRT.Data.Game
{
	/// <summary>
	/// Tile
	/// </summary>
	class Tile
	{
		/// <summary>
		/// Texture
		/// </summary>
		WinWarRT.Graphics.WWTexture texture;
		
		/// <summary>
		/// Average color of this tile
		/// </summary>
		Vector4 averageColor;
		/// <summary>
		/// Average color of this tile
		/// </summary>
		public Vector4 AverageColor
		{
			get
			{
				return averageColor;
			}
		}
		
		/// <summary>
		/// Create tile
		/// </summary>
		public Tile(byte[] data)
		{
			if (data.Length != 256 * 4)
				throw new InvalidDataException("Wrong length of tile data array, " + 
					"must be 1024 bytes.");
		
			averageColor = new Vector4(0, 0, 0, 1);
			
            throw new NotImplementedException();
			/*Texture2D DXTexture = new Texture2D(BaseGame.device, 16, 16, 1, 
				TextureUsage.None, SurfaceFormat.Color);
			DXTexture.SetData<byte>(data);
			
			texture = WinWarRT.Graphics.Texture.FromDXTexture(DXTexture);*/
			
			// Calc average color
			for (int i = 0; i < 256; i+=4)
			{
				averageColor.X += (float)data[i + 0] / 255.0f;
				averageColor.Y += (float)data[i + 1] / 255.0f;
				averageColor.Z += (float)data[i + 2] / 255.0f;
			}
			
			averageColor.X /= 256.0f;
			averageColor.Y /= 256.0f;
			averageColor.Z /= 256.0f;
			averageColor.W = 1.0f;
		} // Tile(data)
		
		/// <summary>
		/// Render
		/// </summary>
		public void Render(float x, float y, float scale)
		{
            texture.RenderOnScreen(x * scale, y * scale, 16.0f * scale, 16.0f * scale);
		} // Render(x, y)
	} // class Tile
} // namespace WinWarCS.Game
