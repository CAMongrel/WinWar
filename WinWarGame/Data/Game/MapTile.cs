﻿// Author: Henning
// Project: WinWarCS
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWar\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 22:25

using WinWarGame.Graphics;
using WinWarGame.Util;

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if NETFX_CORE
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#else
using RectangleF = System.Drawing.RectangleF;
#endif
#endregion

namespace WinWarGame.Data.Game
{
   /// <summary>
   /// Tile
   /// </summary>
   class MapTile
   {
      /// <summary>
      /// Texture
      /// </summary>
      WWTexture texture;

      /// <summary>
      /// Average color of this tile
      /// </summary>
      Color averageColor;
      /// <summary>
      /// Average color of this tile
      /// </summary>
      internal Color AverageColor
      {
         get
         {
            return averageColor;
         }
      }

      /// <summary>
      /// Create tile
      /// </summary>
      internal MapTile(byte[] data)
      {
         if (data.Length != 256 * 4)
            throw new InvalidDataException("Wrong length of tile data array, must be 1024 bytes.");

         averageColor = Color.Black;

         texture = WWTexture.FromRawData(16, 16, data);

         int avgR = 0;
         int avgG = 0;
         int avgB = 0;

         // Calc average color
         for (int i = 0; i < 256; i += 4)
         {
            avgR += data[i + 0];
            avgG += data[i + 1];
            avgB += data[i + 2];
         }

         averageColor.R = (byte)(avgR / 64);
         averageColor.G = (byte)(avgG / 64);
         averageColor.B = (byte)(avgB / 64);
         averageColor.A = 255;
      } // Tile(data)

      /// <summary>
      /// Render
      /// </summary>
      internal void Render(float x, float y, float scale)
      {
         texture.RenderOnScreen(x * scale, y * scale, 16.0f * scale, 16.0f * scale);
         if (DebugOptions.ShowTiles)
            WWTexture.RenderRectangle(new RectangleF(x * scale, y * scale, 16.0f * scale, 16.0f * scale), Color.Red);
      } // Render(x, y)
   } // class Tile
} // namespace WinWarCS.Game
