// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Data\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:04

#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

#endregion
namespace WinWarCS.Data.Game
{
   /// <summary>
   /// The Map class is the center point of a playable level (or map) in WinWar
   /// The map handles all spawned entities, updates states, etc... and creates its own
   /// update thread to update the game logic.
   /// </summary>
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

      /// <summary>
      /// All placed roads
      /// </summary>
      /// <value>The roads.</value>
      internal List<Road> Roads { get; private set; }

      internal List<BasePlayer> Players { get; private set; }

      internal BasePlayer HumanPlayer
      {
         get
         {
            return (from pl in Players
                            where pl.PlayerType == PlayerType.Human
                            select pl).FirstOrDefault ();
         }
      }

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

         Players = new List<BasePlayer> ();

         BuildInitialRoads ();
         PopulateInitialEntities ();
      }
      // Map(setLevelInfo, setLevelVisual, setLevelPassable)

      #endregion

      internal void Start()
      {

      }

      #region Update

      internal void Update(GameTime gameTime)
      {

      }

      #endregion

      void PopulateInitialEntities ()
      {
         if (levelInfo == null)
            return;

         for (int i = 0; i < levelInfo.StartObjects.Length; i++)
         {
            //
         }
      }

      #region Render

      /// <summary>
      /// Render
      /// </summary>
      internal void Render (int setX, int setY, int setWidth, int setHeight, float tileOffsetX, float tileOffsetY)
      {
         // Render tiles
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

         //int count = 170;
         for (int y = 0; y < tilesToDrawY; y++)
         {
            for (int x = 0; x < tilesToDrawX; x++)
            {
               int index = levelVisual.visualData [(x + startTileX) + ((y + startTileY) * MapWidth)];
               //index = count++;

               tileSet.DrawTile (index, setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY, 1.0f);
            }
         }

         // Render Roads
         for (int i = 0; i < Roads.Count; i++) 
         {
            bool isVisible = true;

            Road road = Roads [i];

            int x = road.x - startTileX;
            int y = road.y - startTileY;
            tileSet.DrawRoadTile(road.type, x * TileWidth - innerTileOffsetX, y * TileHeight - innerTileOffsetY, 1.0f);
         }
      }
      // Render()

      #endregion

      #region GetMinimap
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
      #endregion

      #region Roads
      public void PlaceRoad(int x, int y)
      {
         Road road = new Road ();
         road.x = (byte)x;
         road.y = (byte)y;
         road.type = RoadType.EndPieceBottom;

         Roads.Add (road);

         // TODO: Only check adjacent roads
         DetermineRoadTypeForAllRoads ();
      }

      #region BuildRoadTypes

      private void DetermineRoadType(Road road, int index)
      {
         int x = road.x;
         int y = road.y;

         bool topNeighbour = false;
         bool bottomNeighbour = false;
         bool leftNeighbour = false;
         bool rightNeighbour = false;

         for (int j = 0; j < Roads.Count; j++) 
         {
            if (index == j)
               continue;

            if (topNeighbour == false)
               topNeighbour = (Roads [j].x == x && Roads [j].y == y - 1);
            if (bottomNeighbour == false)
               bottomNeighbour = (Roads [j].x == x && Roads [j].y == y + 1);
            if (leftNeighbour == false)
               leftNeighbour = (Roads [j].x == x - 1 && Roads [j].y == y);
            if (rightNeighbour == false)
               rightNeighbour = (Roads [j].x == x + 1 && Roads [j].y == y);
         }

         // Endpieces
         if (topNeighbour && !bottomNeighbour && !leftNeighbour && !rightNeighbour)
            road.type = RoadType.EndPieceBottom;
         if (!topNeighbour && bottomNeighbour && !leftNeighbour && !rightNeighbour)
            road.type = RoadType.EndPieceTop;
         if (!topNeighbour && !bottomNeighbour && !leftNeighbour && rightNeighbour)
            road.type = RoadType.EndPieceLeft;
         if (!topNeighbour && !bottomNeighbour && leftNeighbour && !rightNeighbour)
            road.type = RoadType.EndPieceRight;

         // Corner pieces
         if (topNeighbour && !bottomNeighbour && leftNeighbour && !rightNeighbour)
            road.type = RoadType.CornerLeftTop;
         if (!topNeighbour && bottomNeighbour && leftNeighbour && !rightNeighbour)
            road.type = RoadType.CornerLeftBottom;
         if (topNeighbour && !bottomNeighbour && !leftNeighbour && rightNeighbour)
            road.type = RoadType.CornerRightTop;
         if (!topNeighbour && bottomNeighbour && !leftNeighbour && rightNeighbour)
            road.type = RoadType.CornerRightBottom;

         // Middle pieces
         if (!topNeighbour && !bottomNeighbour && leftNeighbour && rightNeighbour)
            road.type = RoadType.MiddlePieceLeftRight;
         if (topNeighbour && bottomNeighbour && !leftNeighbour && !rightNeighbour)
            road.type = RoadType.MiddlePieceTopBottom;

         // Quad piece
         if (topNeighbour && bottomNeighbour && leftNeighbour && rightNeighbour)
            road.type = RoadType.QuadPiece;

         // T-Corners
         if (topNeighbour && bottomNeighbour && leftNeighbour && !rightNeighbour)
            road.type = RoadType.TPieceLeft;
         if (topNeighbour && bottomNeighbour && !leftNeighbour && rightNeighbour)
            road.type = RoadType.TPieceRight;
         if (!topNeighbour && bottomNeighbour && leftNeighbour && rightNeighbour)
            road.type = RoadType.TPieceBottom;
         if (topNeighbour && !bottomNeighbour && leftNeighbour && rightNeighbour)
            road.type = RoadType.TPieceTop;
      }

      private void BuildInitialRoads ()
      {
         Roads = new List<Road> (levelInfo.StartRoads);

         DetermineRoadTypeForAllRoads ();
      }

      private void DetermineRoadTypeForAllRoads ()
      {
         for (int i = 0; i < Roads.Count; i++) 
         {
            // Check the neighbouring road pieces
            Road road = Roads [i];

            DetermineRoadType (road, i);
         }
      }

      #endregion
      #endregion

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
