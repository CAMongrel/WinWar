// Author: Henning
// Project: WinWarCS
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWar\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:01

#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Data;
#endregion

namespace WinWarRT.Data.Game
{
	/// <summary>
	/// Map tileset
	/// </summary>
	class MapTileset
	{
		const byte lightup = 10;
	
		/// <summary>
		/// Tileset type
		/// </summary>
		Tileset tilesetType;
		/// <summary>
		/// Tileset
		/// </summary>
		WarResource tileset;
		/// <summary>
		/// Tiles
		/// </summary>
		WarResource tiles;
		/// <summary>
		/// Tileset pal
		/// </summary>
		WarResource tilesetPal;
		
		/// <summary>
		/// Known tilesets
		/// </summary>
		static List<MapTileset> knownTilesets;
		
		/// <summary>
		/// 
		/// </summary>
		Tile[] tilesList;
		
		/// <summary>
		/// 
		/// </summary>
		byte[] palette;
		
		#region static ctor
		/// <summary>
		/// Create map tileset
		/// </summary>
		static MapTileset()
		{
			knownTilesets = new List<MapTileset>();
		} // MapTileset()
		#endregion
	
		#region ctor
		/// <summary>
		/// Create map tileset
		/// </summary>
		public MapTileset(Tileset setTilesetType,
			WarResource setTileset, WarResource setTiles,
			WarResource setTilesetPal)
		{
			tiles = setTiles;
			tileset = setTileset;
			tilesetPal = setTilesetPal;
			tilesetType = setTilesetType;
			
			knownTilesets.Add(this);
			
			CreateTiles();
		} // MapTileset(setTilesetType, setTileset, setTiles)
		#endregion
		
		#region GetTileset
		/// <summary>
		/// Get tileset
		/// </summary>
		public static MapTileset GetTileset(Tileset tileSetType)
		{
			for (int i = 0; i < knownTilesets.Count; i++)
			{
				if (knownTilesets[i].tilesetType == tileSetType)
					return knownTilesets[i];
			} // for
			
			return null;
		} // GetTileset(tileSetType)
		#endregion
		
		#region LoadAddTilesets
		/// <summary>
		/// Load all tilesets
		/// </summary>
		public static void LoadAllTilesets()
		{
			WarResource tileset = WarFile.GetResourceByName("Barrens 1");
			WarResource tiles = WarFile.GetResourceByName("Barrens 2");
			WarResource tilesPAL = WarFile.GetResourceByName("Barrens 3");
			new MapTileset(Tileset.Swamp, tileset, tiles, tilesPAL);

			tileset = WarFile.GetResourceByName("Summer 1");
			tiles = WarFile.GetResourceByName("Summer 2");
			tilesPAL = WarFile.GetResourceByName("Summer 3");
			new MapTileset(Tileset.Summer, tileset, tiles, tilesPAL);

			tileset = WarFile.GetResourceByName("Dungeon 1");
			tiles = WarFile.GetResourceByName("Dungeon 2");
			tilesPAL = WarFile.GetResourceByName("Dungeon 3");
			new MapTileset(Tileset.Dungeon, tileset, tiles, tilesPAL);
		} // LoadAllTilesets()
		#endregion
		
		#region CreateTiles
		/// <summary>
		/// Create tiles
		/// </summary>
		unsafe void CreateTiles()
		{
			ushort tile1, tile2, tile3, tile4;
			bool tile1_flip_x, tile2_flip_x, tile3_flip_x, tile4_flip_x;
			bool tile1_flip_y, tile2_flip_y, tile3_flip_y, tile4_flip_y;
		
			// Create palette
			palette = new byte[768];
			Array.Copy(tilesetPal.data, 0, palette, 0, 384);
			for (int i = 0; i < 384; i++)
			{
				palette[i] = (byte)(palette[i] * 3);
			}
			Array.Copy(KnowledgeBase.hardcoded_pal, 0, palette, 384, 384);
			
			// Create tiles
			int numTiles = tileset.data.Length / 8;
			tilesList = new Tile[numTiles];

			fixed (byte* org_ptr = &tileset.data[0])
			{
				ushort* ptr = (ushort*)org_ptr;
			
				for (int i = 0; i < numTiles; i++)
				{
					tile1 = *ptr;
					tile1_flip_y = ((tile1 & 0x01) == 1);
					tile1_flip_x = ((tile1 & 0x02) == 1);
					tile1 = (ushort)((tile1 & 0xFFFC) * 2);
					ptr++;

					tile2 = *ptr;
					tile2_flip_y = ((tile2 & 0x01) == 1);
					tile2_flip_x = ((tile2 & 0x02) == 1);
					tile2 = (ushort)((tile2 & 0xFFFC) * 2);
					ptr++;

					tile3 = *ptr;
					tile3_flip_y = ((tile3 & 0x01) == 1);
					tile3_flip_x = ((tile3 & 0x02) == 1);
					tile3 = (ushort)((tile3 & 0xFFFC) * 2);
					ptr++;

					tile4 = *ptr;
					tile4_flip_y = ((tile4 & 0x01) == 1);
					tile4_flip_x = ((tile4 & 0x02) == 1);
					tile4 = (ushort)((tile4 & 0xFFFC) * 2);
					ptr++;

					tilesList[i] = CreateTile(tile1, tile1_flip_x, tile1_flip_y,
											tile2, tile2_flip_x, tile2_flip_y,
											tile3, tile3_flip_x, tile3_flip_y,
											tile4, tile4_flip_x, tile4_flip_y);
				} // for
			} // fixed
		} // CreateTiles()
		#endregion
		
		#region CreateTile
		/// <summary>
		/// Create tile
		/// </summary>
		unsafe Tile CreateTile(ushort tile1, bool tile1_flip_x, bool tile1_flip_y,
							   ushort tile2, bool tile2_flip_x, bool tile2_flip_y,
							   ushort tile3, bool tile3_flip_x, bool tile3_flip_y,
							   ushort tile4, bool tile4_flip_x, bool tile4_flip_y)
		{
			int x, y, pos;
			byte[] data = new byte[16 * 16 * 4];

			// Tile 1
			fixed (byte* org_ptr = &tiles.data[0])
			{
				byte* b_ptr = org_ptr;
				b_ptr += tile1;
				for (y = 0; y < 8; y++)
				{
					for (x = 0; x < 8; x++)
					{
						pos = ((tile1_flip_x ? 7 - x : x) + (tile1_flip_y ? 7 - y : y) * 16) * 4;
						data[pos + 0] = palette[*b_ptr * 3 + 2];
						data[pos + 1] = palette[*b_ptr * 3 + 1];
						data[pos + 2] = palette[*b_ptr * 3 + 0];
						data[pos + 3] = 255;

						if (data[pos + 0] < 255 - lightup)
							data[pos + 0] += lightup;
						else
							data[pos + 0] = 255;
						if (data[pos + 1] < 255 - lightup)
							data[pos + 1] += lightup;
						else
							data[pos + 1] = 255;
						if (data[pos + 2] < 255 - lightup)
							data[pos + 2] += lightup;
						else
							data[pos + 2] = 255;

						b_ptr++;
					} // for
				} // for
			} // fixed

			// Tile 2
			fixed (byte* org_ptr = &tiles.data[0])
			{
				byte* b_ptr = org_ptr;
				b_ptr += tile2;
				for (y = 0; y < 8; y++)
				{
					for (x = 8; x < 16; x++)
					{
						pos = ((tile2_flip_x ? 8 + (15 - x) : x) + (tile2_flip_y ? 7 - y : y) * 16) * 4;
						data[pos + 0] = palette[*b_ptr * 3 + 2];
						data[pos + 1] = palette[*b_ptr * 3 + 1];
						data[pos + 2] = palette[*b_ptr * 3 + 0];
						data[pos + 3] = 255;

						if (data[pos + 0] < 255 - lightup)
							data[pos + 0] += lightup;
						else
							data[pos + 0] = 255;
						if (data[pos + 1] < 255 - lightup)
							data[pos + 1] += lightup;
						else
							data[pos + 1] = 255;
						if (data[pos + 2] < 255 - lightup)
							data[pos + 2] += lightup;
						else
							data[pos + 2] = 255;

						b_ptr++;
					} // for
				} // for
			} // fixed

			// Tile 3
			fixed (byte* org_ptr = &tiles.data[0])
			{
				byte* b_ptr = org_ptr;
				b_ptr += tile3;
				for (y = 8; y < 16; y++)
				{
					for (x = 0; x < 8; x++)
					{
						pos = ((tile3_flip_x ? 7 - x : x) + (tile3_flip_y ? 8 + (15 - y) : y) * 16) * 4;
						data[pos + 0] = palette[*b_ptr * 3 + 2];
						data[pos + 1] = palette[*b_ptr * 3 + 1];
						data[pos + 2] = palette[*b_ptr * 3 + 0];
						data[pos + 3] = 255;

						if (data[pos + 0] < 255 - lightup)
							data[pos + 0] += lightup;
						else
							data[pos + 0] = 255;
						if (data[pos + 1] < 255 - lightup)
							data[pos + 1] += lightup;
						else
							data[pos + 1] = 255;
						if (data[pos + 2] < 255 - lightup)
							data[pos + 2] += lightup;
						else
							data[pos + 2] = 255;

						b_ptr++;
					} // for
				} // for
			} // fixed

			// Tile 4
			fixed (byte* org_ptr = &tiles.data[0])
			{
				byte* b_ptr = org_ptr;
				b_ptr += tile4;
				for (y = 8; y < 16; y++)
				{
					for (x = 8; x < 16; x++)
					{
						pos = ((tile4_flip_x ? 8 + (15 - x) : x) + (tile4_flip_y ? 8 + (15 - y) : y) * 16) * 4;
						data[pos + 0] = palette[*b_ptr * 3 + 2];
						data[pos + 1] = palette[*b_ptr * 3 + 1];
						data[pos + 2] = palette[*b_ptr * 3 + 0];
						data[pos + 3] = 255;

						if (data[pos + 0] < 255 - lightup)
							data[pos + 0] += lightup;
						else
							data[pos + 0] = 255;
						if (data[pos + 1] < 255 - lightup)
							data[pos + 1] += lightup;
						else
							data[pos + 1] = 255;
						if (data[pos + 2] < 255 - lightup)
							data[pos + 2] += lightup;
						else
							data[pos + 2] = 255;

						b_ptr++;
					} // for
				} // for
			} // fixed

			Tile res = new Tile(data);
			return res;
		} // CreateTile()
		#endregion
		
		#region DrawTile
		/// <summary>
		/// Draw tile
		/// </summary>
		public void DrawTile(int index, float x, float y, float scale)
		{
			tilesList[index].Render(x, y, scale);
		} // DrawTile(x, y)
		#endregion
		
		#region GetTileAverageColor
		/// <summary>
		/// Get tile average color
		/// </summary>
		public Vector4 GetTileAverageColor(int index)
		{
			return tilesList[index].AverageColor;
		} // GetTileAverageColor(index)
		#endregion
		
		#region DrawTiles
		/// <summary>
		/// Draw tiles
		/// </summary>
		public void DrawTiles()
		{
			for (int i = 0; i < tilesList.Length; i++)
            {
				DrawTile(i, (i % 16) * 16, (i / 16) * 16, 2.0f);
            }
		} // DrawTiles()
		#endregion
		
		#region Unit-testing
		/// <summary>
		/// Test tileset
		/// </summary>
		public static void TestTileset()
		{
            throw new NotImplementedException();
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
		} // TestTileset()
		#endregion
	} // class MapTileset
} // namespace WinWarCS.Game
