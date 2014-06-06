#region Using directives
using System;
using System.Collections.Generic;
using System.Text;

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

   internal class LevelInfoResource : BasicResource
   {
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

      internal struct LevelObject
      {
         internal byte x, y;
         internal LevelObjectType type;
         internal byte player, value1, value2;
      };

      #endregion

      #region Variables

      int _offset;

      public int StartGold { get; private set; }
      public int StartLumber { get; private set; }

      public string MissionText { get; private set; }

      private List<LevelObject> _objects;

      public int StartCameraX { get; private set; }
      public int StartCameraY { get; private set; }

      public Road[] startRoads { get; private set; }

      #endregion

      #region Constructor

      internal LevelInfoResource (WarResource data, int offset)
      {
         Init (data, offset);
      }

      internal LevelInfoResource (string name)
      {
         KnowledgeEntry ke = KnowledgeBase.KEByName (name);

         WarResource res = WarFile.GetResource (ke.id);
         if (res == null)
            throw new ArgumentNullException ("res");

         Init (res, ke.param);
      }

      #endregion

      #region Init

      private void Init (WarResource data, int offset)
      {
         LoadData (data, offset);
      }

      private void LoadData (WarResource data, int offset)
      {
         this.data = data;
         this._offset = offset;

         unsafe {
            fixed (byte* org_ptr = &data.data[0]) {
               byte* ptr = org_ptr;

               StartLumber = *(int*)(&ptr [0x5C]);

               StartGold = *(int*)(&ptr [0x70]);

               StartCameraX = (*(ushort*)(&ptr [0xCC])) / 2;

               StartCameraY = (*(ushort*)(&ptr [0xCE])) / 2;

               _offset = (*(ushort*)(&ptr [_offset]));
               int len = data.data.Length;
               int off = 0;
               byte x, y;

               _objects = new List<LevelObject> ();
               // Add objects
               do {
                  x = ptr [_offset + off];
                  y = ptr [_offset + off + 1];

                  if ((x == 0xFF) && (y == 0xFF)) {
                     off += 2;
                     break;
                  }

                  LevelObject lo = new LevelObject ();
                  lo.x = (byte)(x / 2);
                  lo.y = (byte)(y / 2);

                  lo.type = (LevelObjectType)ptr [_offset + off + 2];
                  lo.player = ptr [_offset + off + 3];

                  off += 4;
                  // If it's a gold mine, check gold amount
                  if (lo.type == LevelObjectType.Goldmine) {
                     lo.value1 = ptr [_offset + off];
                     lo.value2 = ptr [_offset + off + 1];

                     off += 2;
                  }
                  _objects.Add (lo);
               } while (_offset + off < len);

               _offset = _offset + off;

               // Get the text position
               off = *(int*)(&ptr [0x94]);

               // Are we at the position of the text?
               if (off != _offset) {
                  // Should be roads
                  List<Road> _roads = new List<Road> ();

                  Road road;
                  int x2, y2;
                  //int i, j;
                  int dx, dy;
                  off = 0;

                  do {
                     x = ptr [_offset + off];
                     y = ptr [_offset + off + 1];

                     if ((x == 0xFF) && (y == 0xFF))
                        break;

                     off += 2;

                     x2 = ptr [_offset + off];
                     y2 = ptr [_offset + off + 1];

                     off += 2;

                     if (ptr [_offset + off] != 0x00)
                        break;

                     dx = x2 - x;
                     dy = y2 - y;

                     // Shitty code to create roads
                     if (dx < 0) {		// Road that goes to the left
                        while (dx <= 0) {
                           road = new Road ();
                           road.x = (byte)((x - dx) / 2);
                           road.y = (byte)(y / 2);
                           _roads.Add (road);

                           dx++;
                        }
                     } else if (dx > 0) {		// Road that goes to the right
                        while (dx >= 0) {
                           road = new Road ();
                           road.x = (byte)((x + dx) / 2);
                           road.y = (byte)(y / 2);
                           _roads.Add (road);

                           dx--;
                        }
                     } else if (dy < 0) {		// Road that goes to the top
                        while (dy <= 0) {
                           road = new Road ();
                           road.x = (byte)(x / 2);
                           road.y = (byte)((y - dy) / 2);
                           _roads.Add (road);

                           dy++;
                        }
                     } else if (dy > 0) {		// Road that goes to the bottom
                        while (dy >= 0) {
                           road = new Road ();
                           road.x = (byte)(x / 2);
                           road.y = (byte)((y + dy) / 2);
                           _roads.Add (road);

                           dy--;
                        }
                     }

                     off++;
                  } while(_offset + off < len);

                  startRoads = _roads.ToArray ();
               }

               // Get the text position again

               StringBuilder sb = new StringBuilder ();

               off = *(int*)(&ptr [0x94]);

               byte* b_ptr = &ptr [off];
               while (*b_ptr != 0x00) {
                  sb.Append ((char)*b_ptr);
                  b_ptr++;
               }

               MissionText = sb.ToString ();
            }
         }
      }

      #endregion

      #region Unit testing

      internal static void TestLoadLevelInfo ()
      {
         throw new NotImplementedException ();
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
