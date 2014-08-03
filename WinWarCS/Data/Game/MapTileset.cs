// Author: Henning
// Project: WinWarCS
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWar\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:01
using WinWarCS.Data.Resources;

#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data;

#endregion

namespace WinWarCS.Data.Game
{
   internal enum Tileset : int
   {
      Summer,
      Swamp,
      Dungeon
   }

   /// <summary>
   /// Map tileset
   /// </summary>
   class MapTileset
   {
      private const byte lightup = 10;
	
      /// <summary>
      /// Tileset type
      /// </summary>
      private Tileset tilesetType;
      /// <summary>
      /// Tileset
      /// </summary>
      private WarResource tileset;
      /// <summary>
      /// Tiles
      /// </summary>
      private WarResource tiles;
      /// <summary>
      /// Tileset pal
      /// </summary>
      private WarResource tilesetPal;
		
      /// <summary>
      /// Known tilesets
      /// </summary>
      private static List<MapTileset> knownTilesets;
		
      /// <summary>
      /// 
      /// </summary>
      private MapTile[] tilesList;
		
      /// <summary>
      /// 
      /// </summary>
      private byte[] palette;

      public int[] RoadIndices;

      internal int Count
      {
         get
         {
            if (tilesList == null)
               return 0;

            return tilesList.Length;
         }
      }

      #region static ctor

      /// <summary>
      /// Create map tileset
      /// </summary>
      static MapTileset ()
      {
         knownTilesets = new List<MapTileset> ();
      }
      // MapTileset()

      #endregion

      #region ctor

      /// <summary>
      /// Create map tileset
      /// </summary>
      internal MapTileset (Tileset setTilesetType,
                      WarResource setTileset, WarResource setTiles,
                      WarResource setTilesetPal)
      {
         tiles = setTiles;
         tileset = setTileset;
         tilesetPal = setTilesetPal;
         tilesetType = setTilesetType;
			
         CreateTiles ();
         CreateRoadTypes ();
      }
      // MapTileset(setTilesetType, setTileset, setTiles)

      #endregion

      #region GetTileset

      /// <summary>
      /// Get tileset
      /// </summary>
      internal static MapTileset GetTileset (Tileset tileSetType)
      {
         for (int i = 0; i < knownTilesets.Count; i++) 
         {
            if (knownTilesets [i].tilesetType == tileSetType)
               return knownTilesets [i];
         } // for
			
         return null;
      }
      // GetTileset(tileSetType)

      #endregion

      #region CreateRoadTypes
      void CreateRoadTypes ()
      {
         // TODO: Refactor me

         int offset = 0;
         switch (tilesetType) 
         {
         case Tileset.Swamp:
            RoadIndices = new int[15];
            offset = 57;
            RoadIndices [(int)RoadType.EndPieceLeft] = offset + 0;
            RoadIndices [(int)RoadType.EndPieceTop] = offset + 1;
            RoadIndices [(int)RoadType.EndPieceRight] = offset + 2;
            RoadIndices [(int)RoadType.EndPieceBottom] = offset + 3;
            RoadIndices [(int)RoadType.CornerRightTop] = offset + 4;
            RoadIndices [(int)RoadType.MiddlePieceTopBottom] = offset + 5;
            RoadIndices [(int)RoadType.CornerLeftTop] = offset + 6;
            RoadIndices [(int)RoadType.TPieceRight] = offset + 7;
            RoadIndices [(int)RoadType.TPieceTop] = offset + 8;
            RoadIndices [(int)RoadType.TPieceLeft] = offset + 9;
            RoadIndices [(int)RoadType.QuadPiece] = offset + 10;
            RoadIndices [(int)RoadType.CornerRightBottom] = offset + 11;
            RoadIndices [(int)RoadType.MiddlePieceLeftRight] = offset + 12;
            RoadIndices [(int)RoadType.TPieceBottom] = offset + 13;
            RoadIndices [(int)RoadType.CornerLeftBottom] = offset + 14;
            break;

         case Tileset.Summer:
            RoadIndices = new int[15];
            offset = 56;
            RoadIndices [(int)RoadType.EndPieceLeft] = offset + 0;
            RoadIndices [(int)RoadType.EndPieceTop] = offset + 1;
            RoadIndices [(int)RoadType.EndPieceRight] = offset + 2;
            RoadIndices [(int)RoadType.EndPieceBottom] = offset + 3;
            RoadIndices [(int)RoadType.CornerRightTop] = offset + 4;
            RoadIndices [(int)RoadType.MiddlePieceTopBottom] = offset + 5;
            RoadIndices [(int)RoadType.CornerLeftTop] = offset + 6;
            RoadIndices [(int)RoadType.TPieceRight] = offset + 7;
            RoadIndices [(int)RoadType.TPieceTop] = offset + 8;
            RoadIndices [(int)RoadType.TPieceLeft] = offset + 9;
            RoadIndices [(int)RoadType.QuadPiece] = offset + 10;
            RoadIndices [(int)RoadType.CornerRightBottom] = offset + 11;
            RoadIndices [(int)RoadType.MiddlePieceLeftRight] = offset + 12;
            RoadIndices [(int)RoadType.TPieceBottom] = offset + 13;
            RoadIndices [(int)RoadType.CornerLeftBottom] = offset + 14;
            break;

         case Tileset.Dungeon:
            break;
         }
      }
      #endregion

      #region LoadAddTilesets

      /// <summary>
      /// Load all tilesets
      /// </summary>
      internal static void LoadAllTilesets ()
      {
         WarResource tileset = WarFile.GetResourceByName ("Barrens 1");
         WarResource tiles = WarFile.GetResourceByName ("Barrens 2");
         WarResource tilesPAL = WarFile.GetResourceByName ("Barrens 3");
         MapTileset swamp = new MapTileset (Tileset.Swamp, tileset, tiles, tilesPAL);
         knownTilesets.Add (swamp);

         tileset = WarFile.GetResourceByName ("Summer 1");
         tiles = WarFile.GetResourceByName ("Summer 2");
         tilesPAL = WarFile.GetResourceByName ("Summer 3");
         MapTileset summer = new MapTileset (Tileset.Summer, tileset, tiles, tilesPAL);
         knownTilesets.Add (summer);

         if (WarFile.IsDemo == false)
         {
            tileset = WarFile.GetResourceByName ("Dungeon 1");
            tiles = WarFile.GetResourceByName ("Dungeon 2");
            tilesPAL = WarFile.GetResourceByName ("Dungeon 3");
            MapTileset dungeon = new MapTileset (Tileset.Dungeon, tileset, tiles, tilesPAL);
            knownTilesets.Add (dungeon);
         }
      }
      // LoadAllTilesets()

      #endregion

      #region CreateTiles

      /// <summary>
      /// Create tiles
      /// </summary>
      unsafe void CreateTiles ()
      {
         ushort tile1, tile2, tile3, tile4;
         bool tile1_flip_x, tile2_flip_x, tile3_flip_x, tile4_flip_x;
         bool tile1_flip_y, tile2_flip_y, tile3_flip_y, tile4_flip_y;
		
         // Create palette
         palette = new byte[768];
         Array.Copy (tilesetPal.data, 0, palette, 0, 384);
         for (int i = 0; i < 384; i++) {
            palette [i] = (byte)(palette [i] * 3);
         }
         //Array.Copy (KnowledgeBase.hardcoded_pal, 0, palette, 384, 384);

         //Array.Copy (KnowledgeBase.hardcoded_pal, 0, palette, 0, KnowledgeBase.hardcoded_pal.Length);
			
         // Create tiles
         int numTiles = tileset.data.Length / 8;
         tilesList = new MapTile[numTiles];

         fixed (byte* org_ptr = &tileset.data[0]) {
            ushort* ptr = (ushort*)org_ptr;
			
            for (int i = 0; i < numTiles; i++) {
               tile1 = *ptr;
               tile1_flip_y = ((tile1 & 0x01) >= 1);
               tile1_flip_x = ((tile1 & 0x02) >= 1);
               tile1 = (ushort)((tile1 & 0xFFFC) * 2);
               ptr++;

               tile2 = *ptr;
               tile2_flip_y = ((tile2 & 0x01) >= 1);
               tile2_flip_x = ((tile2 & 0x02) >= 1);
               tile2 = (ushort)((tile2 & 0xFFFC) * 2);
               ptr++;

               tile3 = *ptr;
               tile3_flip_y = ((tile3 & 0x01) >= 1);
               tile3_flip_x = ((tile3 & 0x02) >= 1);
               tile3 = (ushort)((tile3 & 0xFFFC) * 2);
               ptr++;

               tile4 = *ptr;
               tile4_flip_y = ((tile4 & 0x01) >= 1);
               tile4_flip_x = ((tile4 & 0x02) >= 1);
               tile4 = (ushort)((tile4 & 0xFFFC) * 2);
               ptr++;

               tilesList [i] = CreateTile (tile1, tile1_flip_x, tile1_flip_y,
                  tile2, tile2_flip_x, tile2_flip_y,
                  tile3, tile3_flip_x, tile3_flip_y,
                  tile4, tile4_flip_x, tile4_flip_y);
            } // for
         } // fixed
      }
      // CreateTiles()

      #endregion

      #region CreateTile

      private unsafe void FillTileData(ushort offset, bool flipX, bool flipY, byte[] data, int baseX, int baseY)
      {
         int x, y, pos;

         fixed (byte* org_ptr = &tiles.data[offset]) 
         {
            byte* b_ptr = org_ptr;

            for (y = baseY; y < (8 + baseY); y++) 
            {
               for (x = baseX; x < (8 + baseX); x++) 
               {
                  int pal_index = *b_ptr;

                  int xPos = (flipX ? baseX + ((7 + baseX) - x) : x);
                  int yPos = (flipY ? baseY + ((7 + baseY) - y) : y);

                  pos = (xPos + yPos * 16) * 4;
                  data [pos + 0] = palette [pal_index * 3 + 0];
                  data [pos + 1] = palette [pal_index * 3 + 1];
                  data [pos + 2] = palette [pal_index * 3 + 2];
                  data [pos + 3] = 255;

                  if (data [pos + 0] < 255 - lightup)
                     data [pos + 0] += lightup;
                  else
                     data [pos + 0] = 255;
                  if (data [pos + 1] < 255 - lightup)
                     data [pos + 1] += lightup;
                  else
                     data [pos + 1] = 255;
                  if (data [pos + 2] < 255 - lightup)
                     data [pos + 2] += lightup;
                  else
                     data [pos + 2] = 255;

                  b_ptr++;
               } // for
            } // for
         } // fixed
      }

      /// <summary>
      /// Create tile
      /// </summary>
      private MapTile CreateTile (ushort tile1, bool tile1_flip_x, bool tile1_flip_y,
                         ushort tile2, bool tile2_flip_x, bool tile2_flip_y,
                         ushort tile3, bool tile3_flip_x, bool tile3_flip_y,
                         ushort tile4, bool tile4_flip_x, bool tile4_flip_y)
      {
         byte[] data = new byte[16 * 16 * 4];

         FillTileData (tile1, tile1_flip_x, tile1_flip_y, data, 0, 0);
         FillTileData (tile2, tile2_flip_x, tile2_flip_y, data, 8, 0);
         FillTileData (tile3, tile3_flip_x, tile3_flip_y, data, 0, 8);
         FillTileData (tile4, tile4_flip_x, tile4_flip_y, data, 8, 8);

         return new MapTile (data);
      }
      // CreateTile()

      #endregion

      #region DrawTile

      /// <summary>
      /// Draw tile
      /// </summary>
      internal void DrawTile (int index, float x, float y, float scale)
      {
         if (index < 0 || index >= tilesList.Length)
            return;

         tilesList [index].Render (x, y, scale);
      }
      // DrawTile(x, y)

      internal void DrawRoadTile(RoadType type, float x, float y, float scale)
      {
         if (RoadIndices == null)
            // TileSet has no roads
            return;

         int index = RoadIndices [(int)type];
         DrawTile (index, x, y, scale);
      }

      #endregion

      #region GetTileAverageColor

      /// <summary>
      /// Get tile average color
      /// </summary>
      internal Color GetTileAverageColor (int index)
      {
         if (index < 0 || index >= tilesList.Length)
            return Color.Purple;

         return tilesList [index].AverageColor;
      }
      // GetTileAverageColor(index)

      #endregion

      #region DrawTiles

      /// <summary>
      /// Draw tiles
      /// </summary>
      internal void DrawTiles (int index = 0)
      {
         for (int i = 0; i < (tilesList.Length - index); i++) 
         {
            DrawTile (i + index, (i % 16) * 16, (i / 16) * 16, 1.0f);
         }
      }
      // DrawTiles()

      #endregion

      #region Unit-testing

      /// <summary>
      /// Test tileset
      /// </summary>
      internal static void TestTileset ()
      {
         throw new NotImplementedException ();
         /*TestGame.Start("TestTileset",
				delegate
				{
					MapTileset.LoadAllTilesets();
				},
				delegate
				{
					MapTileset maptiles = MapTileset.GetTileset(Tileset.Swamp);
				
					maptiles.DrawTiles();
				});
            */
      }
      // TestTileset()

      #endregion
   }
   // class MapTileset
}
 // namespace WinWarCS.Game
