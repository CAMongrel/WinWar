#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace WinWarRT.Data.Resources
{
	internal class LevelInfoResource : BasicResource
	{
		#region enum RoadType
		internal enum RoadType
		{
			EndPieceLeft,
			EndPieceRight,
			EndPieceTop,
			EndPieceBottom,
			MiddlePieceLeftRight,
			MiddlePieceTopBottom,
			TPieceLeft,
			TPieceRight,
			TPieceTop,
			TPieceBottom,
			CornerLeftBottom,
			CornerRightBottom,
			CornerLeftTop,
			CornerRightTop,
			QuadPiece,
		}
		#endregion

		#region struct Road
		internal struct Road
		{
			internal byte x,
				y;
			internal RoadType type;
		}
		#endregion

		#region enum LevelObjectType
		internal enum LevelObjectType
		{
			//Units:
			Warrior,		// 0x00
			Grunt,
			Peasant,
			Peon,
			Ballista,
			Catapult,
			Knight,
			Rider,
			Bowman,
			Spearman,
			Conjurer,		// 0x0A
			Warlock,
			Cleric,
			Necrolyte,
			Medivh,
			Lothar,
			Wounded,
			Unk1,
			Garona,
			Unk2,
			Ogre,
			Spider,
			Slime,
			Fire_Elemental,
			Scorpion,
			Brigand,
			Skeleton,
			Skeleton2,
			Daemon,
			Dragon_Cyclops_Giant,
			Unk3,
			Water_Elemental,// 0x1F

			//Buildings:
			Human_Farm,		// 0x20
			Orc_Farm,
			Human_Barracks,
			Orc_Barracks,
			Human_Church,
			Orc_Temple,
			Human_Tower,
			Orc_Tower,
			Human_HQ,
			Orc_HQ,
			Human_Mill,		// 0x2A
			Orc_Mill,
			Human_Stables,
			Orc_Kennel,
			Human_Blacksmith,
			Orc_Blacksmith,
			Stormwind,
			Black_Rock,
			Goldmine,		// 0x32

			//Other:
			Orc_corpse,		// 0x33
		}
		#endregion

		#region Struct LevelObject
		internal struct LevelObject
		{
			internal byte x, y;
			internal LevelObjectType type;
			internal byte player, value1, value2;
		};
		#endregion

		#region Variables
		int _offset;

		int _gold;
		int _lumber;
		string _mission_text;

		//int _numObjects;
		List<LevelObject> _objects;

		int _cam_x;
		int _cam_y;

		//int _numRoads;
		List<Road> _roads;
		#endregion

		#region Constructor
		internal LevelInfoResource(WarResource data, int offset)
		{
			Init(data, offset);
		}

		internal LevelInfoResource(string name)
		{
			KnowledgeEntry ke = KnowledgeBase.KEByName(name);

			WarResource res = WarFile.GetResource(ke.id);
			if (res == null)
				throw new ArgumentNullException("res");

			Init(res, ke.param);
		}
		#endregion

		#region Init
		private void Init(WarResource data, int offset)
		{
			this.data = data;
			this._offset = offset;

			unsafe	
			{
				fixed (byte* org_ptr = &data.data[0])
				{
					byte* ptr = org_ptr;

					_lumber = *(int*)(&ptr[0x5C]);

					_gold = *(int*)(&ptr[0x70]);

					_cam_x = (*(ushort*)(&ptr[0xCC])) / 2;

					_cam_y = (*(ushort*)(&ptr[0xCE])) / 2;

					_offset = (*(ushort*)(&ptr[_offset]));
					int len = data.data.Length;
					int off = 0;
					byte x, y;

					_objects = new List<LevelObject>();
					// Add objects
					do 
					{
						x = ptr[_offset + off];
						y = ptr[_offset + off + 1];

						if ((x == 0xFF) && (y == 0xFF))
						{
							off += 2;
							break;
						}

						LevelObject lo = new LevelObject();
						lo.x = (byte)(x / 2);
						lo.y = (byte)(y / 2);

						lo.type = (LevelObjectType)ptr[_offset + off + 2];
						lo.player = ptr[_offset + off + 3];

						off += 4;
						// If it's a gold mine, check gold amount
						if (lo.type == LevelObjectType.Goldmine)
						{
							lo.value1 = ptr[_offset + off];
							lo.value2 = ptr[_offset + off + 1];

							off += 2;
						}
						_objects.Add(lo);
					} while (_offset + off < len);

					_offset = _offset + off;

					// Get the text position
					off = *(int*)(&ptr[0x94]);

					// Are we at the position of the text?
					if (off != _offset)
					{
						// Should be roads
						_roads = new List<Road>();

						Road road;
						int x2, y2;
						//int i, j;
						int dx, dy;
						off = 0;

						do 
						{
							x = ptr[_offset + off];
							y = ptr[_offset + off + 1];

							if ((x == 0xFF) && (y == 0xFF))
								break;

							off += 2;

							x2 = ptr[_offset + off];
							y2 = ptr[_offset + off + 1];

							off += 2;

							if (ptr[_offset + off] != 0x00)
								break;

							dx = x2 - x;
							dy = y2 - y;

							// Shitty code to create roads
							if (dx < 0)		// Road that goes to the left
							{
								while (dx <= 0) 
								{
									road = new Road();
									road.x = (byte)((x - dx) / 2);
									road.y = (byte)(y / 2);
									_roads.Add(road);

									dx++;
								}
							}
							else
							if (dx > 0)		// Road that goes to the right
							{
								while (dx >= 0) 
								{
									road = new Road();
									road.x = (byte)((x + dx) / 2);
									road.y = (byte)(y / 2);
									_roads.Add(road);

									dx--;
								}
							}
							else
							if (dy < 0)		// Road that goes to the top
							{
								while (dy <= 0) 
								{
									road = new Road();
									road.x = (byte)(x / 2);
									road.y = (byte)((y - dy) / 2);
									_roads.Add(road);

									dy++;
								}
							}
							else
							if (dy > 0)		// Road that goes to the bottom
							{
								while (dy >= 0) 
								{
									road = new Road();
									road.x = (byte)(x / 2);
									road.y = (byte)((y + dy)/2);
									_roads.Add(road);

									dy--;
								}
							}

							off++;
						} while(_offset + off < len);
					}

					// Get the text position again

                    StringBuilder sb = new StringBuilder();

					off = *(int*)(&ptr[0x94]);

					byte* b_ptr = &ptr[off];
					while (*b_ptr != 0x00)
					{
						sb.Append((char)*b_ptr);
                        b_ptr++;
					}

                    _mission_text = sb.ToString();
				}
			}
		}
		#endregion

		#region BuildRoadTypes
		private void BuildRoadTypes()
		{
			for (int i = 0; i < _roads.Count; i++)
			{
				// Check the neighbouring road pieces
				Road road = _roads[i];

				int x = _roads[i].x;
				int y = _roads[i].y;

				bool topNeighbour = false;
				bool bottomNeighbour = false;
				bool leftNeighbour = false;
				bool rightNeighbour = false;

				for (int j = 0; j < _roads.Count; j++)
				{
					if (i == j)
						continue;

					topNeighbour = (_roads[j].x == x && _roads[j].y == y + 1);
					bottomNeighbour = (_roads[j].x == x && _roads[j].y == y - 1);
					leftNeighbour = (_roads[j].x == x - 1 && _roads[j].y == y);
					rightNeighbour = (_roads[j].x == x + 1 && _roads[j].y == y);

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

				_roads[i] = road;
			}
		}
		#endregion

		#region Unit testing
		internal static void TestLoadLevelInfo()
		{
            throw new NotImplementedException();
			/*TestGame.Start("TestLoadLevelInfo",
				delegate
				{
					LevelInfoResource res = new LevelInfoResource("Humans 1");
				},
				delegate
				{
				});
            */
		}
		#endregion
	}
}
