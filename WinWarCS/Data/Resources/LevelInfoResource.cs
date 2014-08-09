#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data.Game;

#endregion

namespace WinWarCS.Data.Resources
{
   #region enum RoadType

   internal enum RoadType
   {
      EndPieceLeft,
      EndPieceTop,
      EndPieceRight,
      EndPieceBottom,
      CornerRightTop,
      CornerLeftTop,
      CornerRightBottom,
      CornerLeftBottom,
      TPieceRight,
      TPieceTop,
      TPieceLeft,
      TPieceBottom,
      QuadPiece,
      MiddlePieceTopBottom,
      MiddlePieceLeftRight,
   }

   #endregion

   #region struct Road

   internal class Road
   {
      internal byte x, y;
      internal RoadType type;
   }

   #endregion

   #region enum LevelObjectType

   internal enum LevelObjectType
   {
      //Units:
      Warrior,
      // 0x00
      Grunt,
      Peasant,
      Peon,
      Ballista,
      Catapult,
      Knight,
      Rider,
      Bowman,
      Spearman,
      Conjurer,
      // 0x0A
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
      Water_Elemental,
      // 0x1F

      //Buildings:
      Human_Farm,
      // 0x20
      Orc_Farm,
      Human_Barracks,
      Orc_Barracks,
      Human_Church,
      Orc_Temple,
      Human_Tower,
      Orc_Tower,
      Human_HQ,
      Orc_HQ,
      Human_Mill,
      // 0x2A
      Orc_Mill,
      Human_Stables,
      Orc_Kennel,
      Human_Blacksmith,
      Orc_Blacksmith,
      Stormwind,
      Black_Rock,
      Goldmine,
      // 0x32

      //Other:
      Orc_corpse,
      // 0x33
   }

   #endregion

   #region Struct LevelObject

   internal class LevelObject
   {
      internal byte x, y;
      internal LevelObjectType type;
      internal byte player, value1, value2;
   };

   #endregion
   internal class LevelInfoResource : BasicResource
   {
      #region Variables

      internal byte[] Magic;
      internal byte[] Header;
      internal ushort[] SubHeader;

      internal int StartCameraX { get; private set; }
      internal int StartCameraY { get; private set; }

      internal int StartGold { get; private set; }
      internal int StartLumber { get; private set; }

      /// <summary>
      /// Resource index of LevelInfoResource for next level
      /// </summary>
      /// <value>The index of the next level resource.</value>
      internal ushort NextLevelResourceIndex { get; private set; }

      /// <summary>
      /// Resource index of LevelPassableResource for this level in DATA.WAR
      /// </summary>
      /// <value>The index of the passable resource.</value>
      internal ushort PassableResourceIndex { get; private set; }
      /// <summary>
      /// Resource index of LevelVisualResource for this level in DATA.WAR
      /// </summary>
      /// <value>The index of the visual resource.</value>
      internal ushort VisualResourceIndex { get; private set; }

      /// <summary>
      /// Resource index of the Tileset resource for this level in DATA.WAR
      /// </summary>
      /// <value>The index of the tileset resource.</value>
      internal ushort TilesetResourceIndex { get; private set; }
      /// <summary>
      /// Resource index of the Tiles resource for this level in DATA.WAR
      /// </summary>
      /// <value>The index of the tileset resource.</value>
      internal ushort TilesResourceIndex { get; private set; }
      /// <summary>
      /// Resource index of the Tiles palette resource for this level in DATA.WAR
      /// </summary>
      /// <value>The index of the tileset resource.</value>
      internal ushort TilesPaletteResourceIndex { get; private set; }

      /// <summary>
      /// Race of the human player (Orcs/Humans)
      /// </summary>
      /// <value>The human player race.</value>
      internal Race HumanPlayerRace { get; private set; }

      internal string MissionText { get; private set; }

      internal Road[] StartRoads { get; private set; }

      internal LevelObject[] StartObjects { get; private set; }

      #endregion

      #region Constructor

      internal LevelInfoResource(WarResource data)
      {
         Type = ContentFileType.FileLevelInfo;

         Init(data);
      }

      #endregion

      #region Init

      private void Init(WarResource data)
      {
         LoadData(data);
      }

      private void LoadData(WarResource data)
      {
         this.Resource = data;

         unsafe
         {
            fixed (byte* org_ptr = &data.data[0])
            {
               byte* ptr = org_ptr;

               // 54 (0x36) bytes header
               Magic = new byte[4];
               Magic[0] = data.data[0]; Magic[1] = data.data[1]; Magic[2] = data.data[2]; Magic[3] = data.data[3];
               Header = new byte[50];
               Array.Copy(data.data, 4, Header, 0, 50);
               // 4 bytes FF FF FF FF
               // 32 (0x20) bytes follow
               SubHeader = new ushort[16];
               int offset = 0x3A;
               for (int i = 0; i < SubHeader.Length; i++)
               {
                  SubHeader[i] = (ushort)(data.data[offset] + (data.data[offset + 1] << 8));
                  offset += 2;
               }
               // 2 bytes FF FF

               // 0x5C => Starting amount of lumber (uint) Player 1
               StartLumber = *(int*)(&ptr[0x5C]);
               // 0x60 => Starting amount of lumber (uint) Player 2
               // 0x64 => Starting amount of lumber (uint) Player 3
               // 0x68 => Starting amount of lumber (uint) Player 4
               // 0x6C => Starting amount of lumber (uint) Player 5?

               // 0x70=> Starting amount of gold (uint) Player 1
               StartGold = *(int*)(&ptr[0x70]);
               // 0x74=> Starting amount of gold (uint) Player 2
               // 0x78=> Starting amount of gold (uint) Player 3
               // 0x7C=> Starting amount of gold (uint) Player 4
               // 0x80=> Starting amount of gold (uint) Player 5?

               // 0xCC, 0xCE => Starting position of camera (divide by 2) (ushort)
               StartCameraX = (*(ushort*)(&ptr[0xCC])) / 2;
               StartCameraY = (*(ushort*)(&ptr[0xCE])) / 2;

               // 0x86 => if 1, human player is "Humans"
               // 0x84 => if 1, human player is "Orcs"
               if ((*(ushort*)(&ptr[0x86])) > 0)
                  HumanPlayerRace = Race.Humans;
               else if ((*(ushort*)(&ptr[0x84])) > 0)
                  HumanPlayerRace = Race.Orcs;

               // 0x94 => Offset to mission text (ushort)
               uint missionTextOffset = *(uint*)(&ptr[0x94]);
               MissionText = string.Empty;
               if (missionTextOffset > 0)    // 0 => No MissionText
               {
                  StringBuilder sb = new StringBuilder();

                  byte* b_ptr = &ptr[missionTextOffset];
                  // Nullterminated string
                  while (*b_ptr != 0x00)
                  {
                     sb.Append((char)*b_ptr);
                     b_ptr++;
                  }

                  MissionText = sb.ToString();
               }

               // 0xCA => -2 to get next map (ushort)
               NextLevelResourceIndex = (ushort)((*(ushort*)(&ptr[0xCA])) - 2);
               // 0xD0, 0xD2 => -2 to get level visual and level passable (ushort)
               VisualResourceIndex = (ushort)((*(ushort*)(&ptr[0xD0])) - 2);
               PassableResourceIndex = (ushort)((*(ushort*)(&ptr[0xD2])) - 2);
               // 0xD4, 0xD6, 0xD8 => -2 to get map tileset, etc.. (ushort)
               TilesetResourceIndex = (ushort)((*(ushort*)(&ptr[0xD4])) - 2);
               TilesResourceIndex = (ushort)((*(ushort*)(&ptr[0xD6])) - 2);
               TilesPaletteResourceIndex = (ushort)((*(ushort*)(&ptr[0xD8])) - 2);

               // 0xE3 start of dynamic data
               // TODO: Needs figuring out how this is actually stored and what the data means
               // For now, just search for the next chunk
               ushort* uptr = (ushort*)&ptr[0xE3];
               while (*uptr != 0xFFFF)
                  uptr++;

               StartObjects = new LevelObject[0];

               // FF FF
               // Roads => x/y - x2/y2 - owner
               StartRoads = new Road[0];
               // FF FF
               // Walls => x/y - x2/y2 - owner
               // FF FF
            }
         }
      }

      private void OldLoadData(WarResource data, int offset)
      {
         this.Resource = data;
         int _offset = offset;

         unsafe
         {
            fixed (byte* org_ptr = &data.data[0])
            {
               byte* ptr = org_ptr;

               StartLumber = *(int*)(&ptr[0x5C]);

               StartGold = *(int*)(&ptr[0x70]);

               StartCameraX = (*(ushort*)(&ptr[0xCC])) / 2;

               StartCameraY = (*(ushort*)(&ptr[0xCE])) / 2;

               _offset = (*(ushort*)(&ptr[_offset]));
               int len = data.data.Length;
               int off = 0;
               byte x, y;

               List<LevelObject> _objects = new List<LevelObject>();
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

               StartObjects = _objects.ToArray();

               _offset = _offset + off;

               // Get the text position
               off = *(int*)(&ptr[0x94]);

               // Are we at the position of the text?
               if (off != _offset)
               {
                  // Should be roads
                  List<Road> _roads = new List<Road>();

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
                     if (dx < 0)
                     {		// Road that goes to the left
                        while (dx <= 0)
                        {
                           road = new Road();
                           road.x = (byte)((x - dx) / 2);
                           road.y = (byte)(y / 2);
                           _roads.Add(road);

                           dx++;
                        }
                     }
                     else if (dx > 0)
                     {		// Road that goes to the right
                        while (dx >= 0)
                        {
                           road = new Road();
                           road.x = (byte)((x + dx) / 2);
                           road.y = (byte)(y / 2);
                           _roads.Add(road);

                           dx--;
                        }
                     }
                     else if (dy < 0)
                     {		// Road that goes to the top
                        while (dy <= 0)
                        {
                           road = new Road();
                           road.x = (byte)(x / 2);
                           road.y = (byte)((y - dy) / 2);
                           _roads.Add(road);

                           dy++;
                        }
                     }
                     else if (dy > 0)
                     {		// Road that goes to the bottom
                        while (dy >= 0)
                        {
                           road = new Road();
                           road.x = (byte)(x / 2);
                           road.y = (byte)((y + dy) / 2);
                           _roads.Add(road);

                           dy--;
                        }
                     }

                     off++;
                  } while(_offset + off < len);

                  StartRoads = _roads.ToArray();
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

               MissionText = sb.ToString();
            }
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
