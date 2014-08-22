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
      internal List<Construct> Roads { get; private set; }

      internal List<Construct> Walls { get; private set; }

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
         Performance.Push("Start Map");
         Players.AddRange(allPlayers);

         Performance.Push("FillAStar");
         levelPassable.FillAStar(Pathfinder);
         Performance.Pop();

         entities = new List<Entity> ();

         Performance.Push("HideMap");
         HideMap();
         Performance.Pop();

         Performance.Push("BuildInitialConstructs");
         BuildInitialConstructs();
         Performance.Pop();

         Performance.Push("PopulateInitialEntities");
         PopulateInitialEntities();
         Performance.Pop();

         Performance.Push("DiscoverMap");
         DiscoverMap();
         Performance.Pop();

         // Test
         if (DebugOptions.ShowFullMapOnLoad)
            ShowMap();

         // Set initial resources
         HumanPlayer.Gold = levelInfo.PlayerInfos[0].StartGold;
         HumanPlayer.Lumber = levelInfo.PlayerInfos[0].StartLumber;
         // TODO: Set resource for other players
         Performance.Pop();
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
            BasePlayer newOwner = getPlayer (lo.Player);
            if (CreateEntity(lo.X, lo.Y, lo.Type, newOwner) == false)
               Log.Write(LogType.Generic, LogSeverity.Status, "Failed to place entity of type '" + lo.Type + "' at " + lo.X + "," + lo.Y + ".");
         }
      }

      internal bool CreateEntity(int x, int y, LevelObjectType entityType, BasePlayer owner)
      {
         Log.Write(LogType.Generic, LogSeverity.Debug, "Pathfinder at [" + x + "," + y + "]: " + Pathfinder[x, y]);
         Log.Write(LogType.Generic, LogSeverity.Debug, "levelPassable at [" + x + "," + y + "]: " + levelPassable.passableData[x, y]);
         if (Pathfinder[x, y] != 0)
            return false;

         Entity newEnt = Entity.CreateEntityFromType (entityType, this);
         newEnt.SetPosition (x, y);
         entities.Add (newEnt);

         if (owner != null)
            // Neutral entities may not have an owner
            owner.ClaimeOwnership (newEnt);

         newEnt.DidSpawn ();

         // Add to Pathfinder
         Pathfinder.SetFieldsBlocked(x, y, newEnt.TileSizeX, newEnt.TileSizeY);

         return true;
      }

      internal void RemoveEntity(Entity ent)
      {
         if (ent == null)
            return;

         if (ent.Owner != null)
            ent.Owner.RemoveOwnership (ent);

         for (int i = 0; i < entities.Count; i++) 
         {
            entities[i].HateList.RemoveEntity (ent);
         }

         entities.Remove (ent);

         if (SelectedEntities.Contains(ent))
            SelectedEntities.Remove(ent);

         // Remove from Pathfinder
         Pathfinder.SetFieldsFree(ent.TileX, ent.TileY, ent.TileSizeX, ent.TileSizeY);
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
                  short passableValue = levelPassable.passableData[x + startTileX, y + startTileY];
                  bool isBlocked = passableValue > 0;
                  if (isBlocked) 
                  {
                     Color col = new Color(0.0f, 1.0f, 0.0f, 0.5f);
                     if (passableValue == 128)
                        col = new Color(0.2f, 0.0f, 0.8f, 0.5f);

                     WWTexture.SingleWhite.RenderOnScreen (setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY,
                        TileWidth, TileHeight, col);
                  }
               }
            }
         }

         // Render Roads
         for (int i = 0; i < Roads.Count; i++) 
         {
            bool isVisible = true;

            Construct road = Roads [i];

            if (mapDiscoverState [road.X + road.Y * MapWidth] == MapDiscover.Unknown)
               isVisible = false;

            int x = road.X - startTileX;
            int y = road.Y - startTileY;
            if (isVisible)
               tileSet.DrawRoadTile(road.Config, setX + x * TileWidth - innerTileOffsetX, setY + y * TileHeight - innerTileOffsetY, 1.0f);
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
         Construct road = new Construct(ConstructType.Road);
         road.X = (byte)x;
         road.Y = (byte)y;
         road.Config = ConstructConfig.EndPieceBottom;

         Roads.Add (road);

         // TODO: Only check adjacent roads
         DetermineConstructConfigForAllConstructs ();
      }

      #region BuildRoadTypes

      private void DetermineConstructConfig(Construct constr, int index, List<Construct> constructs)
      {
         int x = constr.X;
         int y = constr.Y;

         bool topNeighbour = false;
         bool bottomNeighbour = false;
         bool leftNeighbour = false;
         bool rightNeighbour = false;

         for (int j = 0; j < constructs.Count; j++) 
         {
            if (index == j)
               continue;

            if (topNeighbour == false)
               topNeighbour = (constructs [j].X == x && constructs [j].Y == y - 1);
            if (bottomNeighbour == false)
               bottomNeighbour = (constructs [j].X == x && constructs [j].Y == y + 1);
            if (leftNeighbour == false)
               leftNeighbour = (constructs [j].X == x - 1 && constructs [j].Y == y);
            if (rightNeighbour == false)
               rightNeighbour = (constructs [j].X == x + 1 && constructs [j].Y == y);
         }

         // Endpieces
         if (topNeighbour && !bottomNeighbour && !leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.EndPieceBottom;
         if (!topNeighbour && bottomNeighbour && !leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.EndPieceTop;
         if (!topNeighbour && !bottomNeighbour && !leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.EndPieceLeft;
         if (!topNeighbour && !bottomNeighbour && leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.EndPieceRight;

         // Corner pieces
         if (topNeighbour && !bottomNeighbour && leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.CornerLeftTop;
         if (!topNeighbour && bottomNeighbour && leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.CornerLeftBottom;
         if (topNeighbour && !bottomNeighbour && !leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.CornerRightTop;
         if (!topNeighbour && bottomNeighbour && !leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.CornerRightBottom;

         // Middle pieces
         if (!topNeighbour && !bottomNeighbour && leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.MiddlePieceLeftRight;
         if (topNeighbour && bottomNeighbour && !leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.MiddlePieceTopBottom;

         // Quad piece
         if (topNeighbour && bottomNeighbour && leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.QuadPiece;

         // T-Corners
         if (topNeighbour && bottomNeighbour && leftNeighbour && !rightNeighbour)
            constr.Config = ConstructConfig.TPieceLeft;
         if (topNeighbour && bottomNeighbour && !leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.TPieceRight;
         if (!topNeighbour && bottomNeighbour && leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.TPieceBottom;
         if (topNeighbour && !bottomNeighbour && leftNeighbour && rightNeighbour)
            constr.Config = ConstructConfig.TPieceTop;
      }

      private void BuildInitialConstructs ()
      {
         Roads = new List<Construct> (levelInfo.StartRoads);
         Walls = new List<Construct> (levelInfo.StartWalls);

         DetermineConstructConfigForAllConstructs ();
      }

      private void DetermineConstructConfigForAllConstructs ()
      {
         for (int i = 0; i < Roads.Count; i++) 
         {
            // Check the neighbouring road pieces
            Construct road = Roads [i];

            DetermineConstructConfig (road, i, Roads);
         }

         for (int i = 0; i < Walls.Count; i++) 
         {
            // Check the neighbouring road pieces
            Construct wall = Walls [i];

            DetermineConstructConfig (wall, i, Walls);
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
