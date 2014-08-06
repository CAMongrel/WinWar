// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Data\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:04
using WinWarCS.Util;
using WinWarCS.Graphics;

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
   enum MapDiscover
   {
      Unknown,
      Fog,
      Visible
   }
      
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

      private MapDiscover[] mapDiscoverState;

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

      private List<Entity> entities;

      internal AStar2D Pathfinder;

      internal Random Rnd { get; private set; }

      internal List<Entity> SelectedEntities { get; private set; }

      #region ctor

      /// <summary>
      /// Create map
      /// </summary>
      internal Map (LevelInfoResource setLevelInfo, 
               LevelVisualResource setLevelVisual,
               LevelPassableResource setLevelPassable)
      {
         SelectedEntities = new List<Entity>();

         TileWidth = 16;
         TileHeight = 16;

         MapWidth = 64;
         MapHeight = 64;

         levelInfo = setLevelInfo;
         levelVisual = setLevelVisual;
         levelPassable = setLevelPassable;
			
         mapDiscoverState = new MapDiscover[MapWidth * MapHeight];

         tileSet = MapTileset.GetTileset((int)levelInfo.TilesetResourceIndex);

         Players = new List<BasePlayer> ();

         Rnd = new Random ();

         Pathfinder = new AStar2D ();
      }
      // Map(setLevelInfo, setLevelVisual, setLevelPassable)

      #endregion

      internal void Start(List<BasePlayer> allPlayers)
      {
         Players.AddRange (allPlayers);
         levelPassable.FillAStar (Pathfinder);

         entities = new List<Entity> ();

         HideMap ();
         BuildInitialRoads ();
         PopulateInitialEntities ();
         DiscoverMap ();

         // Set initial resources
         HumanPlayer.Gold = levelInfo.StartGold;
         HumanPlayer.Lumber = levelInfo.StartLumber;
         // TODO: Set resource for other players
      }

      #region Update

      internal void Update(GameTime gameTime)
      {
         for (int i = 0; i < entities.Count; i++) 
         {
            entities [i].Update (gameTime);

            // TODO: Implement a shared view flag for allied forces?

            if (entities[i].Owner == HumanPlayer)
               DiscoverMapByEntity (entities [i]);
         }
      }

      #endregion

      #region Map discovery
      internal MapDiscover GetDiscoverStateAtTile(int tileX, int tileY)
      {
         if (tileX < 0 || tileX >= MapWidth || tileY < 0 || tileY >= MapHeight)
            return MapDiscover.Unknown;

         return mapDiscoverState[tileX + tileY * MapWidth];
      }

      private void HideMap ()
      {
         for (int i = 0; i < mapDiscoverState.Length; i++)
            mapDiscoverState [i] = MapDiscover.Unknown;
      }

      private void ShowMap ()
      {
         for (int i = 0; i < mapDiscoverState.Length; i++)
            mapDiscoverState [i] = MapDiscover.Visible;
      }

      private void DiscoverMapAt(int centerTileX, int centerTileY, double visibleRange)
      {
         double sqrDiscoverRange = visibleRange * visibleRange;

         for (int y = -(int)visibleRange; y <= (int)visibleRange; y++) 
         {
            double actualY = (double)y - 0.5;
            int tileY = centerTileY + (int)actualY;

            if (tileY < 0 || tileY >= MapHeight)
               continue;

            for (int x = -(int)visibleRange; x <= (int)visibleRange; x++) 
            {
               double actualX = (double)x - 0.5;
               int tileX = centerTileX + (int)actualX;

               if (tileX < 0 || tileX >= MapWidth)
                  continue;

               double sqrDist = actualX * actualX + actualY * actualY;
               if (sqrDiscoverRange >= sqrDist)
                  mapDiscoverState [tileX + tileY * MapWidth] = MapDiscover.Visible;
            }
         }
      }

      private void DiscoverMapByEntity (Entity ent)
      {
         for (int y = 0; y < ent.TileSizeY; y++) 
         {
            for (int x = 0; x < ent.TileSizeX; x++) 
            {
               DiscoverMapAt (ent.TileX + x, ent.TileY + y, ent.VisibleRange);
            }
         }
      }

      private void DiscoverMap ()
      {
         // TODO: Implement shared view for allied forces?

         Entity[] ownEntities = HumanPlayer.Entities.ToArray ();

         for (int i = 0; i < ownEntities.Length; i++) 
         {
            DiscoverMapByEntity (ownEntities [i]);
         }
      }
      #endregion

      private BasePlayer getPlayer(byte playerIndex)
      {
         if (playerIndex >= 0 && playerIndex < Players.Count)
            return Players [playerIndex];

         return null;
      }

      #region Entities
      private void PopulateInitialEntities ()
      {
         if (levelInfo == null)
            return;

         for (int i = 0; i < levelInfo.StartObjects.Length; i++)
         {
            LevelObject lo = levelInfo.StartObjects [i];
            BasePlayer newOwner = getPlayer (lo.player);
            CreateEntity (lo.x, lo.y, lo.type, newOwner);
         }
      }

      internal void CreateEntity(int x, int y, LevelObjectType entityType, BasePlayer owner)
      {
         Entity newEnt = Entity.CreateEntityFromType (entityType, this);
         newEnt.SetPosition (x, y);
         entities.Add (newEnt);

         if (owner != null)
            // Neutral entities may not have an owner
            owner.ClaimeOwnership (newEnt);

         newEnt.DidSpawn ();

         // TODO: Add to Pathfinder
      }

      internal void RemoveEntity(Entity ent)
      {
         if (ent == null)
            return;

         if (ent.Owner != null)
            ent.Owner.RemoveOwnership (ent);

         for (int i = 0; i < entities.Count; i++) 
         {
            entities [i].HateList.RemoveEntity (ent);
         }

         entities.Remove (ent);

         if (SelectedEntities.Contains(ent))
            SelectedEntities.Remove(ent);

         // TODO: Remove from Pathfinder
      }

      internal Entity GetEntityAt(int tileX, int tileY)
      {
         for (int i = 0; i < entities.Count; i++) 
         {
            Entity ent = entities [i];

            if (tileX >= ent.TileX &&
                tileY >= ent.TileY &&
                tileX < ent.TileX + ent.TileSizeX &&
                tileY < ent.TileY + ent.TileSizeY) 
            {
               return ent;
            }
         }

         return null;
      }

      private void InternalDeselectAllEntities()
      {
         for (int i = SelectedEntities.Count - 1; i >= 0; i--)
         {
            Entity selEnt = SelectedEntities [i];
            if (selEnt.WillDeselect () == false)
               continue;

            SelectedEntities.Remove (selEnt);
            selEnt.DidDeselect ();
         }
      }
      private void InternalSelectAllEntities(Entity[] entities)
      {
         for (int i = 0; i < entities.Length; i++)
         {
            Entity selEnt = entities [i];
            if (selEnt == null || selEnt.WillSelect () == false)
               continue;

            SelectedEntities.Add (selEnt);
            selEnt.DidSelect ();
         }
      }

      internal void SelectEntities(params Entity[] entities)
      {
         // Try to deselect all entities
         InternalDeselectAllEntities ();

         // If there is still at least one entity selected, the deselection
         // process was rejected, so we can't select a new one
         if (SelectedEntities.Count > 0)
            return;

         // If we passed null, then we want to deselect
         if (entities == null || entities.Length == 0)
            return;

         InternalSelectAllEntities (entities);
      }
      #endregion

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

               tileSet.DrawTile (index, setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY, 1.0f);

               if (DebugOptions.ShowBlockedTiles) 
               {
                  bool isBlocked = levelPassable.passableData[x + startTileX, y + startTileY] > 0;
                  if (isBlocked) 
                  {
                     WWTexture.SingleWhite.RenderOnScreen (setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY,
                        TileWidth, TileHeight, new Color (0.0f, 1.0f, 0.0f, 0.5f));
                  }
               }
            }
         }

         // Render Roads
         for (int i = 0; i < Roads.Count; i++) 
         {
            bool isVisible = true;

            Road road = Roads [i];

            if (mapDiscoverState [road.x + road.y * MapWidth] == MapDiscover.Unknown)
               isVisible = false;

            int x = road.x - startTileX;
            int y = road.y - startTileY;
            if (isVisible)
               tileSet.DrawRoadTile(road.type, setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY, 1.0f);
         }

         // Render entities
         for (int i = 0; i < entities.Count; i++) 
         {
            bool isVisible = true;

            Entity ent = entities [i];

            if (isVisible)
               ent.Render (setX, setY, tileOffsetX, tileOffsetY);
         }

         // Render selected entities
         for (int i = 0; i < SelectedEntities.Count; i++)
         {
            Entity selEnt = SelectedEntities [i];
            WWTexture.RenderRectangle (selEnt.GetTileRectangle (setX, setY, tileOffsetX, tileOffsetY), new Color(0, 255, 0), 3);
         }

         // Overlay undiscored places + fog of war
         for (int y = 0; y < tilesToDrawY; y++)
         {
            for (int x = 0; x < tilesToDrawX; x++)
            {
               int pos = (x + startTileX) + ((y + startTileY) * MapWidth);

               if (mapDiscoverState [pos] == MapDiscover.Fog) 
               {
                  WWTexture.SingleWhite.RenderOnScreen (setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY,
                     TileWidth, TileHeight, new Color (0.0f, 0.0f, 0.0f, 0.5f));
               } else if (mapDiscoverState [pos] == MapDiscover.Unknown) 
               {
                  WWTexture.SingleWhite.RenderOnScreen (setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY,
                     TileWidth, TileHeight, new Color (0.0f, 0.0f, 0.0f, 1.0f));
               } 
            }
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

         // Overlay units and buildings
         for (int i = 0; i < entities.Count; i++) 
         {
            Color col = Color.LightGray;
            Entity ent = entities [i];
            if (ent.Owner != null) 
            {
               // Allied and self
               if (ent.Owner.IsFriendlyTowards (HumanPlayer))
                  col = new Color(0, 255, 0);
               // Enemies
               if (ent.Owner.IsHostileTowards (HumanPlayer))
                  col = Color.Red;
            }
            if (ent.Type == LevelObjectType.Human_HQ ||
               ent.Type == LevelObjectType.Orc_HQ) 
            {
               col = Color.Yellow;
            }

            if (ent.TileSizeX == 1 && ent.TileSizeY == 1) 
            {
               result [ent.TileX + ent.TileY * MapWidth] = col;
            } 
            else 
            {
               for (int y = 0; y < ent.TileSizeY; y++)
               {
                  for (int x = 0; x < ent.TileSizeX; x++)
                  {
                     int tileX = ent.TileX + x;
                     int tileY = ent.TileY + y;
                     result [tileX + tileY * MapWidth] = col;
                  }
               }
            }
         }

         // Overlay undiscovered areas.
         // This is not effective (could also do this conditionally in each of the loops above),
         // but the most secure way.
         for (int y = 0; y < MapHeight; y++)
         {
            for (int x = 0; x < MapWidth; x++)
            {
               int pos = x + y * MapWidth;
               if (mapDiscoverState[pos] == MapDiscover.Unknown)
                  result [pos] = Color.Black;
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

      #region Pathfinding
      internal MapPath CalcPath(int startX, int startY, int endX, int endY)
      {
         Log.Status("Map: Calculating path from " + startX + "," + 
            startY + " to " + endX + "," + endY + "...");

         MapPath path = Pathfinder.FindPath (startX, startY, endX, endY);
         if (path != null)
         {
            Log.Status("... success (" + path.Count + " Nodes)!");
            return path;
         }

         Log.Status("... failed!");

         return null;
      }
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
