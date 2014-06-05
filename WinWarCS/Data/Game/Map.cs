// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Data\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:04

#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

#endregion
namespace WinWarCS.Data.Game
{
   class Map
   {
      private LevelInfoResource levelInfo;
      private LevelVisualResource levelVisual;
      private LevelPassableResource levelPassable;
      private MapTileset tileSet;

      internal int TileWidth { get; private set; }

      internal int TileHeight { get; private set; }

      internal int MapWidth { get; private set; }

      internal int MapHeight { get; private set; }

      #region ctor

      /// <summary>
      /// Create map
      /// </summary>
      internal Map (LevelInfoResource setLevelInfo, 
               LevelVisualResource setLevelVisual,
               LevelPassableResource setLevelPassable)
      {
         TileWidth = 16;
         TileHeight = 16;

         MapWidth = 64;
         MapHeight = 64;

         levelInfo = setLevelInfo;
         levelVisual = setLevelVisual;
         levelPassable = setLevelPassable;
			
         tileSet = MapTileset.GetTileset (levelVisual.Tileset);
      }
      // Map(setLevelInfo, setLevelVisual, setLevelPassable)

      #endregion

      #region Render

      /// <summary>
      /// Render
      /// </summary>
      internal void Render (int setX, int setY, int setWidth, int setHeight, float tileOffsetX, float tileOffsetY)
      {
         if (tileOffsetX < 0.0f)
            tileOffsetX = 0.0f;
         if (tileOffsetY < 0.0f)
            tileOffsetY = 0.0f;

         int tilesToDrawX = (setWidth / TileWidth) + 1;
         int tilesToDrawY = (setHeight / TileHeight) + 1;

         int startTileX = ((int)tileOffsetX / TileWidth);
         int startTileY = ((int)tileOffsetY / TileHeight);

         if (tilesToDrawX + startTileX > MapWidth)
            tilesToDrawX = MapWidth - startTileX;
         if (tilesToDrawY + startTileY > MapHeight)
            tilesToDrawY = MapHeight - startTileY;

         //float innerTileOffsetX = (float)((int)(tileOffsetX * 100) % (TileWidth * 100)) / 100.0f;
         //float innerTileOffsetY = (float)((int)(tileOffsetY * 100) % (TileHeight * 100)) / 100.0f;
         int innerTileOffsetX = ((int)tileOffsetX % TileWidth);
         int innerTileOffsetY = ((int)tileOffsetY % TileHeight);

         for (int y = 0; y < tilesToDrawY; y++)
         {
            for (int x = 0; x < tilesToDrawX; x++)
            {
               tileSet.DrawTile (levelVisual.visualData [(x + startTileX) + ((y + startTileY) * MapWidth)], 
                  setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY, 1.0f);
            }
         }
      }
      // Render()

      #endregion

      internal Color[] GetMinimap ()
      {
         Color[] result = new Color[MapWidth * MapHeight];
         for (int y = 0; y < MapHeight; y++)
         {
            for (int x = 0; x < MapWidth; x++)
            {
               result [x + y * MapWidth] = tileSet.GetTileAverageColor (levelVisual.visualData [x + (y * MapWidth)]);
            }
         }
         return result;
      }

      #region Unit-testing

      /// <summary>
      /// Test map
      /// </summary>
      internal static void TestMap ()
      {
         throw new NotImplementedException ();
         /*Map map = null;
		
			TestGame.Start("TestMap",
				delegate
				{
					LevelInfoResource lInfo = 
						new LevelInfoResource("Humans 1");
							
					LevelVisualResource lVisual = 
						new LevelVisualResource("Humans 1 (Visual)");
					
					LevelPassableResource lPassable = 
						new LevelPassableResource(
							WarFile.GetResourceByName("Humans 1 (Passable)"));
				
					map = new Map(lInfo, lVisual, lPassable);
				},
				delegate
				{
					map.Render();
				});
            */
      }
      // TestMap()

      #endregion

   }
}
