#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data.Game;

#endregion

namespace WinWarCS.Data.Resources
{
   #region enum PieceType

   internal enum ConstructType
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

   #region struct Construct

   internal class Construct
   {
      internal byte X;
      internal byte Y;
      internal ConstructType Type;
      internal byte Owner;
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
      internal byte X;
      internal byte Y;
      internal LevelObjectType Type;
      internal byte Player;
      internal byte Value1;
      internal byte Value2;
   };

   internal class PlayerInfo
   {
      internal int StartGold { get; set; }
      internal int StartLumber { get; set; }
      internal Race Race { get; set; }
   }

   #endregion
   internal class LevelInfoResource : BasicResource
   {
      private enum LevelInfoType
      {
         Unknown,
         Type1,      // 0x83 0x09 0x20 0x00
         Type2,      // 0xD3 0x9B 0x20 0x00
         Type3,      // 0xFF 0xFF 0x7F 0x00
      }

      #region Variables

      internal byte[] Magic;
      internal byte[] Header;
      internal ushort[] SubHeader;

      private LevelInfoType levelInfoType;

      internal int StartCameraX { get; private set; }
      internal int StartCameraY { get; private set; }

      internal PlayerInfo[] PlayerInfos { get; private set; }

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
      /// PlayerInfo of the human player (Orcs/Humans)
      /// </summary>
      internal PlayerInfo HumanPlayerInfo 
      { 
         get { return PlayerInfos[0]; }
      }

      internal string MissionText { get; private set; }

      internal Construct[] StartRoads { get; private set; }
      internal Construct[] StartWalls { get; private set; }

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

      private LevelInfoType DetermineType(byte[] magic)
      {
         if (magic[0] == 0x83 && magic[1] == 0x09 && magic[2] == 0x20 && magic[3] == 0x00)
            return LevelInfoType.Type1;
         if (magic[0] == 0xD3 && magic[1] == 0x9B && magic[2] == 0x20 && magic[3] == 0x00)
            return LevelInfoType.Type2;
         if (magic[0] == 0xFF && magic[1] == 0xFF && magic[2] == 0x7F && magic[3] == 0x00)
            return LevelInfoType.Type3;

         return LevelInfoType.Unknown;
      }

      private LevelObject[] ReadStartObjects(int offset, WarResource res)
      {
         List<LevelObject> result = new List<LevelObject>();

         while (offset < res.data.Length)
         {
            uint val = ReadUShort(offset, res.data);
            if (val == 0xFFFF)
               break;

            LevelObject lo = new LevelObject();
            lo.X = (byte)(res.data[offset + 0] / 2);
            lo.Y = (byte)(res.data[offset + 1] / 2);
            lo.Type = (LevelObjectType)res.data[offset + 2];
            lo.Player = res.data[offset + 3];

            offset += 4;

            if (lo.Type == LevelObjectType.Goldmine)
            {
               lo.Value1 = res.data[offset + 0];
               lo.Value2 = res.data[offset + 1];

               offset += 2;
            }

            result.Add(lo);
         }

         return result.ToArray();
      }

      private void ReadHeaders(WarResource res)
      {
         // 54 (0x36) bytes header
         Magic = ReadBytes(0, 4, res.data);
         levelInfoType = DetermineType(Magic);
         Header = ReadBytes(4, 50, res.data);

         // 4 bytes FF FF FF FF
         // 32 (0x20) bytes follow
         SubHeader = new ushort[16];
         int offset = 0x3A;
         for (int i = 0; i < SubHeader.Length; i++)
         {
            SubHeader[i] = (ushort)(res.data[offset] + (res.data[offset + 1] << 8));
            offset += 2;
         }
         // 2 bytes FF FF
      }

      private unsafe void ReadStartingResources(byte* ptr)
      {
         // 0x5C => Starting amount of lumber (uint) Player 1
         // 0x60 => Starting amount of lumber (uint) Player 2
         // 0x64 => Starting amount of lumber (uint) Player 3
         // 0x68 => Starting amount of lumber (uint) Player 4
         // 0x6C => Starting amount of lumber (uint) Player 5?
         for (int i = 0; i < 5; i++)
         {
            int startLumber = *(int*)(&ptr[0x5C + i * 4]);
            if (startLumber > 0)
            {
               if (PlayerInfos[i] == null)
                  PlayerInfos[i] = new PlayerInfo();
               PlayerInfos[i].StartLumber = startLumber;
            }
         }
         // 0x70=> Starting amount of gold (uint) Player 1
         // 0x74=> Starting amount of gold (uint) Player 2
         // 0x78=> Starting amount of gold (uint) Player 3
         // 0x7C=> Starting amount of gold (uint) Player 4
         // 0x80=> Starting amount of gold (uint) Player 5?
         for (int i = 0; i < 5; i++)
         {
            int startGold = *(int*)(&ptr[0x70 + i * 4]);
            if (startGold > 0)
            {
               if (PlayerInfos[i] == null)
                  PlayerInfos[i] = new PlayerInfo();
               PlayerInfos[i].StartGold = startGold;
            }
         }
      }

      private unsafe void ReadPlayerInfo(byte* ptr)
      {
         // 0xCC, 0xCE => Starting position of camera (divide by 2) (ushort)
         StartCameraX = (*(ushort*)(&ptr[0xCC])) / 2;
         StartCameraY = (*(ushort*)(&ptr[0xCE])) / 2;

         // 0x86 => if 1, human player is "Humans"
         // 0x84 => if 1, human player is "Orcs"
         Race humanPlayerRace = Race.Humans;
         if ((*(ushort*)(&ptr[0x86])) > 0)
            humanPlayerRace = Race.Humans;
         else
            if ((*(ushort*)(&ptr[0x84])) > 0)
               humanPlayerRace = Race.Orcs;

         if (PlayerInfos[0] != null)
            PlayerInfos[0].Race = humanPlayerRace;

         for (int i = 1; i < 5; i++)
         {
            if (PlayerInfos[i] != null)
               PlayerInfos[i].Race = (humanPlayerRace == Race.Humans ? Race.Orcs : Race.Humans);
         }
      }

      private unsafe void ReadMissionText(byte* ptr)
      {
         // 0x94 => Offset to mission text (ushort)
         uint missionTextOffset = *(uint*)(&ptr[0x94]);
         MissionText = string.Empty;
         if (missionTextOffset > 0)// 0 => No MissionText
         
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
      }

      private void ReadResourceIndices(WarResource res)
      {
         // 0xCA => -2 to get next map (ushort)
         NextLevelResourceIndex = ReadResourceIndexDirectUShort(0xCA, res);
         // 0xD0, 0xD2 => -2 to get level visual and level passable (ushort)
         VisualResourceIndex = ReadResourceIndexDirectUShort(0xD0, res);
         PassableResourceIndex = ReadResourceIndexDirectUShort(0xD2, res);
         // 0xD4, 0xD6, 0xD8 => -2 to get map tileset, etc.. (ushort)
         TilesetResourceIndex = ReadResourceIndexDirectUShort(0xD4, res);
         TilesResourceIndex = ReadResourceIndexDirectUShort(0xD6, res);
         TilesPaletteResourceIndex = ReadResourceIndexDirectUShort(0xD8, res);
      }

      private void LoadData(WarResource res)
      {
         PlayerInfos = new PlayerInfo[5];

         unsafe
         {
            fixed (byte* org_ptr = &res.data[0])
            {
               byte* ptr = org_ptr;

               ReadHeaders(res);

               ReadStartingResources(ptr);

               ReadPlayerInfo(ptr);

               ReadMissionText(ptr);

               ReadResourceIndices(res);

               // Usually 0x03
               ushort unk = ReadUShort(0xDA, res.data);
               // 7 bytes (always 0x0A 0x72 0x77 0x0A 0x79 0x7E 0x00)
               byte[] unk2 = ReadBytes(0xDC, 7, res.data);

               // 0xE3 start of dynamic data
               // TODO: Needs figuring out how this is actually stored and what the data means
               // For now, just search for the next chunk
               byte* bptr = (byte*)&ptr[0xE3];
               while (*(uint*)bptr != 0xFFFFFFFF)
                  bptr++;
               bptr += 4;
               int startObjOffset = (int)bptr - (int)org_ptr;
               startObjOffset = (int)ReadUShort(startObjOffset, res.data);

               // Read start objects
               StartObjects = ReadStartObjects(startObjOffset, res);

               // FF FF
               // Roads => x/y - x2/y2 - owner
               StartRoads = new Construct[0];
               // FF FF
               // Walls => x/y - x2/y2 - owner
               StartWalls = new Construct[0];
               // FF FF
            }
         }
      }

      private void OldLoadData(WarResource data, int offset)
      {
         int _offset = offset;

         unsafe
         {
            fixed (byte* org_ptr = &data.data[0])
            {
               byte* ptr = org_ptr;

               int StartLumber = *(int*)(&ptr[0x5C]);

               int StartGold = *(int*)(&ptr[0x70]);

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
                  lo.X = (byte)(x / 2);
                  lo.Y = (byte)(y / 2);

                  lo.Type = (LevelObjectType)ptr[_offset + off + 2];
                  lo.Player = ptr[_offset + off + 3];

                  off += 4;
                  // If it's a gold mine, check gold amount
                  if (lo.Type == LevelObjectType.Goldmine)
                  {
                     lo.Value1 = ptr[_offset + off];
                     lo.Value2 = ptr[_offset + off + 1];

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
                  List<Construct> _roads = new List<Construct>();

                  Construct road;
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
                           road = new Construct();
                           road.X = (byte)((x - dx) / 2);
                           road.Y = (byte)(y / 2);
                           _roads.Add(road);

                           dx++;
                        }
                     }
                     else if (dx > 0)
                     {		// Road that goes to the right
                        while (dx >= 0)
                        {
                           road = new Construct();
                           road.X = (byte)((x + dx) / 2);
                           road.Y = (byte)(y / 2);
                           _roads.Add(road);

                           dx--;
                        }
                     }
                     else if (dy < 0)
                     {		// Road that goes to the top
                        while (dy <= 0)
                        {
                           road = new Construct();
                           road.X = (byte)(x / 2);
                           road.Y = (byte)((y - dy) / 2);
                           _roads.Add(road);

                           dy++;
                        }
                     }
                     else if (dy > 0)
                     {		// Road that goes to the bottom
                        while (dy >= 0)
                        {
                           road = new Construct();
                           road.X = (byte)(x / 2);
                           road.Y = (byte)((y + dy) / 2);
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
