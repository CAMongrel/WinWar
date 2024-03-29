using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WinWarGame.Data.Game;

namespace WinWarGame.Data
{
   public enum ContentFileType : int
   {
      FileUnknown,
      FileImage,
      FilePalette,
      FilePaletteShort,
      FileXMID,
      FileCursor,
      FileUI,
      FileText,
      FileLevelInfo,
      FileLevelVisual,
      FileLevelPassable,
      FileSprite,
      FileWave,
      FileVOC,
      FileTileSet,
      FileTiles,
      FileTable,
      FileEmbeddedData,
   }

   public class KnowledgeEntry
   {
      public int id;
      public ContentFileType type;
      public int param;
      public string text;

      internal KnowledgeEntry (int id, ContentFileType type, int param, string text)
      {
         this.id = id;
         this.type = type;
         this.param = param;
         this.text = text;
      }
   }

   // NOTE: Unit stats are in WAR.EXE starting at 0x47A6C
   // TimeToBuild,Gold cost,Lumber cost are val * 10

   public class KnowledgeBase
   {
      #region KnowledgeBase definitions
      private static KnowledgeEntry[] KnowledgeBaseEntries = {
         new KnowledgeEntry (0, ContentFileType.FileXMID, 0, "Music0"),
         new KnowledgeEntry (1, ContentFileType.FileXMID, 0, "Music1"),
         new KnowledgeEntry (2, ContentFileType.FileXMID, 0, "Music2"),
         new KnowledgeEntry (3, ContentFileType.FileXMID, 0, "Music3"),
         new KnowledgeEntry (4, ContentFileType.FileXMID, 0, "Music4"),
         new KnowledgeEntry (5, ContentFileType.FileXMID, 0, "Music5"),
         new KnowledgeEntry (6, ContentFileType.FileXMID, 0, "Music6"),
         new KnowledgeEntry (7, ContentFileType.FileXMID, 0, "Music7"),
         new KnowledgeEntry (8, ContentFileType.FileXMID, 0, "Music8"),
         new KnowledgeEntry (9, ContentFileType.FileXMID, 0, "Music9"),
         new KnowledgeEntry (10, ContentFileType.FileXMID, 0, "Music10"),
         new KnowledgeEntry (11, ContentFileType.FileXMID, 0, "Music11"),
         new KnowledgeEntry (12, ContentFileType.FileXMID, 0, "Music12"),
         new KnowledgeEntry (13, ContentFileType.FileXMID, 0, "Music13"),
         new KnowledgeEntry (14, ContentFileType.FileXMID, 0, "Music14"),
         new KnowledgeEntry (15, ContentFileType.FileXMID, 0, "Music15"),
         new KnowledgeEntry (16, ContentFileType.FileXMID, 0, "Music16"),
         new KnowledgeEntry (17, ContentFileType.FileXMID, 0, "Music17"),
         new KnowledgeEntry (18, ContentFileType.FileXMID, 0, "Music18"),
         new KnowledgeEntry (19, ContentFileType.FileXMID, 0, "Music19"),
         new KnowledgeEntry (20, ContentFileType.FileXMID, 0, "Music20"),
         new KnowledgeEntry (21, ContentFileType.FileXMID, 0, "Music21"),
         new KnowledgeEntry (22, ContentFileType.FileXMID, 0, "Music22"),
         new KnowledgeEntry (23, ContentFileType.FileXMID, 0, "Music23"),
         new KnowledgeEntry (24, ContentFileType.FileXMID, 0, "Music24"),
         new KnowledgeEntry (25, ContentFileType.FileXMID, 0, "Music25"),
         new KnowledgeEntry (26, ContentFileType.FileXMID, 0, "Music26"),
         new KnowledgeEntry (27, ContentFileType.FileXMID, 0, "Music27"),
         new KnowledgeEntry (28, ContentFileType.FileXMID, 0, "Music28"),
         new KnowledgeEntry (29, ContentFileType.FileXMID, 0, "Music29"),
         new KnowledgeEntry (30, ContentFileType.FileXMID, 0, "Music30"),
         new KnowledgeEntry (31, ContentFileType.FileXMID, 0, "Music31"),
         new KnowledgeEntry (32, ContentFileType.FileXMID, 0, "Music32"),
         new KnowledgeEntry (33, ContentFileType.FileXMID, 0, "Music33"),
         new KnowledgeEntry (34, ContentFileType.FileXMID, 0, "Music34"),
         new KnowledgeEntry (35, ContentFileType.FileXMID, 0, "Music35"),
         new KnowledgeEntry (36, ContentFileType.FileXMID, 0, "Music36"),
         new KnowledgeEntry (37, ContentFileType.FileXMID, 0, "Music37"),
         new KnowledgeEntry (38, ContentFileType.FileXMID, 0, "Music38"),
         new KnowledgeEntry (39, ContentFileType.FileXMID, 0, "Music39"),
         new KnowledgeEntry (40, ContentFileType.FileXMID, 0, "Music40"),
         new KnowledgeEntry (41, ContentFileType.FileXMID, 0, "Music41"),
         new KnowledgeEntry (42, ContentFileType.FileXMID, 0, "Music42"),
         new KnowledgeEntry (43, ContentFileType.FileXMID, 0, "Music43"),
         new KnowledgeEntry (44, ContentFileType.FileXMID, 0, "Music44"),
         new KnowledgeEntry (45, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (46, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (47, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (48, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (49, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (50, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (51, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (52, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (53, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (54, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (55, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (56, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (57, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (58, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (59, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (60, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (61, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (62, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (63, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (64, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (65, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (66, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (67, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (68, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (69, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (70, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (71, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (72, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (73, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (74, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (75, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (76, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (77, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (78, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (79, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (80, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (81, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (82, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (83, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (84, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (85, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (86, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (87, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (88, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (89, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (90, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (91, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (92, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (93, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (94, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (95, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (96, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (97, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (98, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (99, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (100, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (101, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (102, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (103, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (104, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (105, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (106, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (107, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (108, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (109, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (110, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (111, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (112, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (113, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (114, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (115, ContentFileType.FileLevelVisual, 0, null),
         new KnowledgeEntry (116, ContentFileType.FileLevelPassable, 0, null),
         new KnowledgeEntry (117, ContentFileType.FileLevelInfo, 0, "Humans 1"),
         new KnowledgeEntry (118, ContentFileType.FileLevelInfo, 0, "Orcs 1"),
         new KnowledgeEntry (119, ContentFileType.FileLevelInfo, 0, "Humans 2"),
         new KnowledgeEntry (120, ContentFileType.FileLevelInfo, 0, "Orcs 2"),
         new KnowledgeEntry (121, ContentFileType.FileLevelInfo, 0, "Humans 3"),
         new KnowledgeEntry (122, ContentFileType.FileLevelInfo, 0, "Orcs 3"),
         new KnowledgeEntry (123, ContentFileType.FileLevelInfo, 0, "Humans 4"),
         new KnowledgeEntry (124, ContentFileType.FileLevelInfo, 0, "Orcs 4"),
         new KnowledgeEntry (125, ContentFileType.FileLevelInfo, 0, "Humans 5"),
         new KnowledgeEntry (126, ContentFileType.FileLevelInfo, 0, "Orcs 5"),
         new KnowledgeEntry (127, ContentFileType.FileLevelInfo, 0, "Humans 6"),
         new KnowledgeEntry (128, ContentFileType.FileLevelInfo, 0, "Orcs 6"),
         new KnowledgeEntry (129, ContentFileType.FileLevelInfo, 0, "Humans 7"),
         new KnowledgeEntry (130, ContentFileType.FileLevelInfo, 0, "Orcs 7"),
         new KnowledgeEntry (131, ContentFileType.FileLevelInfo, 0, "Humans 8"),
         new KnowledgeEntry (132, ContentFileType.FileLevelInfo, 0, "Orcs 8"),
         new KnowledgeEntry (133, ContentFileType.FileLevelInfo, 0, "Humans 9"),
         new KnowledgeEntry (134, ContentFileType.FileLevelInfo, 0, "Orcs 9"),
         new KnowledgeEntry (135, ContentFileType.FileLevelInfo, 0, "Humans 10"),
         new KnowledgeEntry (136, ContentFileType.FileLevelInfo, 0, "Orcs 10"),
         new KnowledgeEntry (137, ContentFileType.FileLevelInfo, 0, "Humans 11"),
         new KnowledgeEntry (138, ContentFileType.FileLevelInfo, 0, "Orcs 11"),
         new KnowledgeEntry (139, ContentFileType.FileLevelInfo, 0, "Humans 12"),
         new KnowledgeEntry (140, ContentFileType.FileLevelInfo, 0, "Orcs 12"),
         new KnowledgeEntry (141, ContentFileType.FileLevelInfo, 0, "Unknown Level"),  
         new KnowledgeEntry (142, ContentFileType.FileLevelInfo, 0, "Unknown Level"),  
         new KnowledgeEntry (143, ContentFileType.FileLevelInfo, 0, "Unknown Level"),  
         new KnowledgeEntry (144, ContentFileType.FileLevelInfo, 0, "Unknown Level"),  
         new KnowledgeEntry (145, ContentFileType.FileUnknown, 0, "Unknown (broken?) Level"),      // Should be a level, but seems to be broken or a different format
         new KnowledgeEntry (146, ContentFileType.FileUnknown, 0, "Unknown (broken?) Level"),      // Should be a level, but seems to be broken or a different format
         new KnowledgeEntry (147, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (148, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (149, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (150, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (151, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (152, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (153, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (154, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (155, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (156, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (157, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (158, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (159, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (160, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (161, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (162, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (163, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (164, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (165, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (166, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (167, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (168, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (169, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (170, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (171, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (172, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (173, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (174, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (175, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (176, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (177, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (178, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (179, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (180, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (181, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (182, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (183, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (184, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (185, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (186, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (187, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (188, ContentFileType.FileLevelInfo, 0, "Unknown Level"), 
         new KnowledgeEntry (189, ContentFileType.FileTileSet, 0, "Summer 1"),
         new KnowledgeEntry (190, ContentFileType.FileTiles, 0, "Summer 2"),
         new KnowledgeEntry (191, ContentFileType.FilePaletteShort, 0, "Summer 3"),
         new KnowledgeEntry (192, ContentFileType.FileTileSet, 0, "Barrens 1"),
         new KnowledgeEntry (193, ContentFileType.FileTiles, 0, "Barrens 2"),
         new KnowledgeEntry (194, ContentFileType.FilePaletteShort, 0, "Barrens 3"),
         new KnowledgeEntry (195, ContentFileType.FileTileSet, 0, "Dungeon 1"),
         new KnowledgeEntry (196, ContentFileType.FileTiles, 0, "Dungeon 2"),
         new KnowledgeEntry (197, ContentFileType.FilePaletteShort, 0, "Dungeon 3"),
         new KnowledgeEntry (198, ContentFileType.FileTable, 0, "Unknown Table 1"),
         new KnowledgeEntry (199, ContentFileType.FileTable, 0, "Unknown Table 2"),
         new KnowledgeEntry (200, ContentFileType.FileTable, 0, "Unknown Table 3"),
         new KnowledgeEntry (201, ContentFileType.FileTable, 0, "Unknown Table 4"),
         new KnowledgeEntry (202, ContentFileType.FileTable, 0, "Unknown Table 5"),
         new KnowledgeEntry (203, ContentFileType.FileTable, 0, "Unknown Table 6"),
         new KnowledgeEntry (204, ContentFileType.FileTable, 0, "Unknown Table 7"),
         new KnowledgeEntry (205, ContentFileType.FileTable, 0, "Unknown Table 8"),
         new KnowledgeEntry (206, ContentFileType.FileTable, 0, "Unknown Table 9"),
         new KnowledgeEntry (207, ContentFileType.FileTable, 0, "Unknown Table 10"),
         new KnowledgeEntry (208, ContentFileType.FileTable, 0, "Unknown Table 11"),
         new KnowledgeEntry (209, ContentFileType.FileTable, 0, "Unknown Table 12"),
         new KnowledgeEntry (210, ContentFileType.FilePaletteShort, 0, "Unknown"),
         new KnowledgeEntry (211, ContentFileType.FilePaletteShort, 0, "Unknown"),
         new KnowledgeEntry (212, ContentFileType.FileUnknown, 0, "Unknown"),
         new KnowledgeEntry (213, ContentFileType.FileUnknown, 0, "Unknown"),
         new KnowledgeEntry (214, ContentFileType.FileUnknown, 0, "Unknown"),
         new KnowledgeEntry (215, ContentFileType.FileUnknown, 0, "Unknown file (short, offset, 2x data PSQR) (P-splines via QR?)"),
         new KnowledgeEntry (216, ContentFileType.FileImage, 217, "Background 'Blizzard'"),
         new KnowledgeEntry (217, ContentFileType.FilePalette, 0, "For Background"),
         new KnowledgeEntry (218, ContentFileType.FileImage, 255, "Topbar (Humans)"),
         new KnowledgeEntry (219, ContentFileType.FileImage, 217, "Topbar (Orcs)"),
         new KnowledgeEntry (220, ContentFileType.FileImage, 255, "Sidebar Right (Humans)"),
         new KnowledgeEntry (221, ContentFileType.FileImage, 217, "Sidebar Right (Orcs)"),
         new KnowledgeEntry (222, ContentFileType.FileImage, 255, "Lower Bar (Humans)"),
         new KnowledgeEntry (223, ContentFileType.FileImage, 217, "Lower Bar (Orcs)"),
         new KnowledgeEntry (224, ContentFileType.FileImage, 255, "Sidebar Left Minimap Empty (Humans)"),
         new KnowledgeEntry (225, ContentFileType.FileImage, 217, "Sidebar Left Minimap Empty (Orcs)"),
         new KnowledgeEntry (226, ContentFileType.FileImage, 255, "Sidebar Left (Humans)"),
         new KnowledgeEntry (227, ContentFileType.FileImage, 217, "Sidebar Left (Orcs)"),
         new KnowledgeEntry (228, ContentFileType.FileImage, 255, "Sidebar Left Minimap Black (Humans)"),
         new KnowledgeEntry (229, ContentFileType.FileImage, 217, "Sidebar Left Minimap Black (Orcs)"),
         new KnowledgeEntry (230, ContentFileType.FileUI, 0, "Warcraft CD"),
         new KnowledgeEntry (231, ContentFileType.FileUI, 0, "Purchase the Retail Version of Warcraft"),
         new KnowledgeEntry (232, ContentFileType.FileUI, 0, "Purchase the Retail Version of Warcraft"),
         new KnowledgeEntry (233, ContentFileType.FileImage, 255, "Large Box (Humans)"),
         new KnowledgeEntry (234, ContentFileType.FileImage, 217, "Large Box (Orcs)"),
         new KnowledgeEntry (235, ContentFileType.FileImage, 255, "Small Box (Humans)"),
         new KnowledgeEntry (236, ContentFileType.FileImage, 217, "Small Box (Orcs)"),
         new KnowledgeEntry (237, ContentFileType.FileImage, 260, "Large Button"),
         new KnowledgeEntry (238, ContentFileType.FileImage, 260, "Large Button (Clicked)"),
         new KnowledgeEntry (239, ContentFileType.FileImage, 260, "Medium Button"),
         new KnowledgeEntry (240, ContentFileType.FileImage, 260, "Medium Button (Clicked)"),
         new KnowledgeEntry (241, ContentFileType.FileImage, 260, "Small Button"),
         new KnowledgeEntry (242, ContentFileType.FileImage, 260, "Small Button (Clicked)"),
         new KnowledgeEntry (243, ContentFileType.FileImage, 260, "Mainmenu Background Lower Part"),
         new KnowledgeEntry (244, ContentFileType.FileImage, 260, "Button Arrow Left"),
         new KnowledgeEntry (245, ContentFileType.FileImage, 260, "Button Arrow Left, Grey"),
         new KnowledgeEntry (246, ContentFileType.FileImage, 260, "Button Arrow Right"),
         new KnowledgeEntry (247, ContentFileType.FileImage, 260, "Button Arrow Right, Grey"),
         new KnowledgeEntry (248, ContentFileType.FileImage, 260, "Window Border"),
         new KnowledgeEntry (249, ContentFileType.FileImage, 260, "Saving/Loading Screen (Humans)"),
         new KnowledgeEntry (250, ContentFileType.FileImage, 260, "Saving/Loading Screen (Orcs)"),
         new KnowledgeEntry (251, ContentFileType.FilePalette, 0, "Unknown"),
         new KnowledgeEntry (252, ContentFileType.FilePalette, 0, "Unknown"),
         new KnowledgeEntry (253, ContentFileType.FilePalette, 0, "Unknown"),
         new KnowledgeEntry (254, ContentFileType.FileImage, 255, "Hot keys"),
         new KnowledgeEntry (255, ContentFileType.FilePalette, 0, "Humans"),
         new KnowledgeEntry (256, ContentFileType.FileImage, 255, "Button 'ok'"),
         new KnowledgeEntry (257, ContentFileType.FileImage, 255, "Button 'ok' 2"),
         new KnowledgeEntry (258, ContentFileType.FileImage, 260, "Text 'WarCraft'"),
         new KnowledgeEntry (259, ContentFileType.FileUI, 0, "Main Menu Text"),
         new KnowledgeEntry (260, ContentFileType.FilePalette, 0, "UI Palette"),
         new KnowledgeEntry (261, ContentFileType.FileImage, 260, "Mainmenu Background"),
         new KnowledgeEntry (262, ContentFileType.FilePalette, 0, "Pointer Palette"),
         new KnowledgeEntry (263, ContentFileType.FileCursor, 262, "Normal Pointer"),
         new KnowledgeEntry (264, ContentFileType.FileCursor, 262, "Not allowed"),
         new KnowledgeEntry (265, ContentFileType.FileCursor, 262, "Crosshair Orange"),
         new KnowledgeEntry (266, ContentFileType.FileCursor, 262, "Crosshair Red"),
         new KnowledgeEntry (267, ContentFileType.FileCursor, 262, "Crosshair Orange 2"),
         new KnowledgeEntry (268, ContentFileType.FileCursor, 262, "Magnifier"),
         new KnowledgeEntry (269, ContentFileType.FileCursor, 262, "Crosshair Green"),
         new KnowledgeEntry (270, ContentFileType.FileCursor, 262, "Loading..."),
         new KnowledgeEntry (271, ContentFileType.FileCursor, 262, "Scroll Top"),
         new KnowledgeEntry (272, ContentFileType.FileCursor, 262, "Scroll Topright"),
         new KnowledgeEntry (273, ContentFileType.FileCursor, 262, "Scroll Right"),
         new KnowledgeEntry (274, ContentFileType.FileCursor, 262, "Scroll Bottomright"),
         new KnowledgeEntry (275, ContentFileType.FileCursor, 262, "Scroll Bottom"),
         new KnowledgeEntry (276, ContentFileType.FileCursor, 262, "Scroll Bottomleft"),
         new KnowledgeEntry (277, ContentFileType.FileCursor, 262, "Scroll Left"),
         new KnowledgeEntry (278, ContentFileType.FileCursor, 262, "Scroll Topleft"),
         new KnowledgeEntry (279, ContentFileType.FileSprite, 217, "Human Warrior"),
         new KnowledgeEntry (280, ContentFileType.FileSprite, 217, "Orc Grunt"),
         new KnowledgeEntry (281, ContentFileType.FileSprite, 217, "Human Peasant"),
         new KnowledgeEntry (282, ContentFileType.FileSprite, 217, "Orc Peon"),
         new KnowledgeEntry (283, ContentFileType.FileSprite, 217, "Human Catapult"),
         new KnowledgeEntry (284, ContentFileType.FileSprite, 217, "Orc Catapult"),
         new KnowledgeEntry (285, ContentFileType.FileSprite, 217, "Human Knight"),
         new KnowledgeEntry (286, ContentFileType.FileSprite, 217, "Orc Rider"),
         new KnowledgeEntry (287, ContentFileType.FileSprite, 217, "Human Bowman"),
         new KnowledgeEntry (288, ContentFileType.FileSprite, 217, "Orc Axethrower"),
         new KnowledgeEntry (289, ContentFileType.FileSprite, 217, "Human Wizard"),
         new KnowledgeEntry (290, ContentFileType.FileSprite, 217, "Orc Wizard"),
         new KnowledgeEntry (291, ContentFileType.FileSprite, 217, "Human Priest"),
         new KnowledgeEntry (292, ContentFileType.FileSprite, 217, "Orc Necro"),
         new KnowledgeEntry (293, ContentFileType.FileSprite, 217, "Medivh"),
         new KnowledgeEntry (294, ContentFileType.FileSprite, 217, "Lothar"),
         new KnowledgeEntry (295, ContentFileType.FileSprite, 217, "Wounded"),
         new KnowledgeEntry (296, ContentFileType.FileSprite, 217, "Garana"),
         new KnowledgeEntry (297, ContentFileType.FileSprite, 217, "Giant"),
         new KnowledgeEntry (298, ContentFileType.FileSprite, 217, "Spider"),
         new KnowledgeEntry (299, ContentFileType.FileSprite, 217, "Slime"),
         new KnowledgeEntry (300, ContentFileType.FileSprite, 217, "Unit_300"),
         new KnowledgeEntry (301, ContentFileType.FileSprite, 217, "Scorpion"),
         new KnowledgeEntry (302, ContentFileType.FileSprite, 217, "Brigand"),
         new KnowledgeEntry (303, ContentFileType.FileSprite, 217, "Skeleton"),
         new KnowledgeEntry (304, ContentFileType.FileSprite, 217, "Skeleton 2"),
         new KnowledgeEntry (305, ContentFileType.FileSprite, 217, "Demon"),
         new KnowledgeEntry (306, ContentFileType.FileSprite, 217, "Water Elemental"),
         new KnowledgeEntry (307, ContentFileType.FileSprite, 217, "Human Farm"),
         new KnowledgeEntry (308, ContentFileType.FileSprite, 217, "Orc Farm"),
         new KnowledgeEntry (309, ContentFileType.FileSprite, 217, "Human Barracks"),
         new KnowledgeEntry (310, ContentFileType.FileSprite, 217, "Orc Barracks"),
         new KnowledgeEntry (311, ContentFileType.FileSprite, 217, "Human Church"),
         new KnowledgeEntry (312, ContentFileType.FileSprite, 217, "Orc Stone Circle"),
         new KnowledgeEntry (313, ContentFileType.FileSprite, 217, "Human Tower"),
         new KnowledgeEntry (314, ContentFileType.FileSprite, 217, "Orc Skull"),
         new KnowledgeEntry (315, ContentFileType.FileSprite, 217, "Human Base"),
         new KnowledgeEntry (316, ContentFileType.FileSprite, 217, "Orc Base"),
         new KnowledgeEntry (317, ContentFileType.FileSprite, 217, "Human Smith"),
         new KnowledgeEntry (318, ContentFileType.FileSprite, 217, "Orc Smith"),
         new KnowledgeEntry (319, ContentFileType.FileSprite, 217, "Human Stables"),
         new KnowledgeEntry (320, ContentFileType.FileSprite, 217, "Orc Stables"),
         new KnowledgeEntry (321, ContentFileType.FileSprite, 217, "Human Smith"),
         new KnowledgeEntry (322, ContentFileType.FileSprite, 217, "Orc Smith"),
         new KnowledgeEntry (323, ContentFileType.FileSprite, 217, "Stormwind"),
         new KnowledgeEntry (324, ContentFileType.FileSprite, 217, "Black Spire"),
         new KnowledgeEntry (325, ContentFileType.FileSprite, 217, "Goldmine"),
         new KnowledgeEntry (326, ContentFileType.FileSprite, 217, "Corpse"),
         new KnowledgeEntry (327, ContentFileType.FileSprite, 217, "Human Peasant with Lumber"),
         new KnowledgeEntry (328, ContentFileType.FileSprite, 217, "Orc Peon with Lumber"),
         new KnowledgeEntry (329, ContentFileType.FileSprite, 217, "Human Peasant with Gold"),
         new KnowledgeEntry (330, ContentFileType.FileSprite, 217, "Orc Peon with Gold"),
         new KnowledgeEntry (331, ContentFileType.FileSprite, 217, "Humans Under construction"),
         new KnowledgeEntry (332, ContentFileType.FileSprite, 217, "Orcs Under construction"),
         new KnowledgeEntry (333, ContentFileType.FileSprite, 217, "Humans Under construction 2"),
         new KnowledgeEntry (334, ContentFileType.FileSprite, 217, "Orcs Under construction 2"),
         new KnowledgeEntry (335, ContentFileType.FileSprite, 217, "Humans Under construction 3"),
         new KnowledgeEntry (336, ContentFileType.FileSprite, 217, "Orcs Under construction 3"),
         new KnowledgeEntry (337, ContentFileType.FileSprite, 217, "Humans Under construction 4"),
         new KnowledgeEntry (338, ContentFileType.FileSprite, 217, "Orcs Under construction 4"),
         new KnowledgeEntry (339, ContentFileType.FileSprite, 217, "Humans Under construction 5"),
         new KnowledgeEntry (340, ContentFileType.FileSprite, 217, "Orcs Under construction 5"),
         new KnowledgeEntry (341, ContentFileType.FileSprite, 217, "Humans Under construction 6"),
         new KnowledgeEntry (342, ContentFileType.FileSprite, 217, "Orcs Under construction 6"),
         new KnowledgeEntry (343, ContentFileType.FileSprite, 217, "Humans Under construction 7"),
         new KnowledgeEntry (344, ContentFileType.FileSprite, 217, "Orcs Under construction 7"),
         new KnowledgeEntry (345, ContentFileType.FileSprite, 217, "Humans Under construction 8"),
         new KnowledgeEntry (346, ContentFileType.FileSprite, 217, "Orcs Under construction 8"),
         new KnowledgeEntry (347, ContentFileType.FileSprite, 217, "Building explosion"),
         new KnowledgeEntry (348, ContentFileType.FileSprite, 217, "Fireball"),
         new KnowledgeEntry (349, ContentFileType.FileSprite, 217, "Arrow"),
         new KnowledgeEntry (350, ContentFileType.FileSprite, 217, "Unit_350"),
         new KnowledgeEntry (351, ContentFileType.FileSprite, 217, "Unit_351"),
         new KnowledgeEntry (352, ContentFileType.FileSprite, 217, "Tornado"),
         new KnowledgeEntry (353, ContentFileType.FileSprite, 217, "Unit_353"),
         new KnowledgeEntry (354, ContentFileType.FileSprite, 217, "Unit_354"),
         new KnowledgeEntry (355, ContentFileType.FileSprite, 217, "Unit_355"),
         new KnowledgeEntry (356, ContentFileType.FileSprite, 217, "Unit_356"),
         new KnowledgeEntry (357, ContentFileType.FileSprite, 217, "Unit_357"),
         new KnowledgeEntry (358, ContentFileType.FileSprite, 217, "Unit_358"),
         new KnowledgeEntry (359, ContentFileType.FileSprite, 217, "UI Pictures Orcs"),
         new KnowledgeEntry (360, ContentFileType.FileSprite, 255, "UI Pictures Humans"),
         new KnowledgeEntry (361, ContentFileType.FileSprite, 217, "UI Pictures Icons"),
         new KnowledgeEntry (362, ContentFileType.FileImage, 260, "Menu Button"),   
         new KnowledgeEntry (363, ContentFileType.FileImage, 260, "Menu Button (Pressed)"),   
         new KnowledgeEntry (364, ContentFileType.FileImage, 260, "Empty Button"),   
         new KnowledgeEntry (365, ContentFileType.FileImage, 260, "Empty Button (Pressed)"),   
         new KnowledgeEntry (366, ContentFileType.FileUI, 0, "Remote system is not responding"),
         new KnowledgeEntry (367, ContentFileType.FileUI, 0, "Remote system is not responding (+Save)"),
         new KnowledgeEntry (368, ContentFileType.FileUI, 0, "Save Game; Load Game; Options; ..."),
         new KnowledgeEntry (369, ContentFileType.FileUI, 0, "Save Game; Load Game; Options; ..."),
         new KnowledgeEntry (370, ContentFileType.FileUI, 0, "Empty slots ... loading"),
         new KnowledgeEntry (371, ContentFileType.FileUI, 0, "Empty slots ... loading"),
         new KnowledgeEntry (372, ContentFileType.FileUI, 0, "Ok; Cancel"),
         new KnowledgeEntry (373, ContentFileType.FileUI, 0, "Ok; Cancel"),
         new KnowledgeEntry (374, ContentFileType.FileUI, 0, "Ok; Cancel"),
         new KnowledgeEntry (375, ContentFileType.FileUI, 0, "Ok; Cancel"),
         new KnowledgeEntry (376, ContentFileType.FileUI, 0, "Options"),  
         new KnowledgeEntry (377, ContentFileType.FileUI, 0, "Options"),  
         new KnowledgeEntry (378, ContentFileType.FileUI, 0, "Select Game Type"),
         new KnowledgeEntry (379, ContentFileType.FileUI, 0, "Choose Campaign"),
         new KnowledgeEntry (380, ContentFileType.FileUI, 0, "CD needed for Multiplayer"),
         new KnowledgeEntry (381, ContentFileType.FileUI, 0, "Customize your Game"),
         new KnowledgeEntry (382, ContentFileType.FileUI, 0, "Set Map"),
         new KnowledgeEntry (383, ContentFileType.FileUI, 0, "Set Opponent"),
         new KnowledgeEntry (384, ContentFileType.FileUI, 0, "Modem Connection"),  
         new KnowledgeEntry (385, ContentFileType.FileUI, 0, "Modem Setup"),  
         new KnowledgeEntry (386, ContentFileType.FileUI, 0, "Direct connection"),  
         new KnowledgeEntry (387, ContentFileType.FileUI, 0, "Network connection"),  
         new KnowledgeEntry (388, ContentFileType.FileUI, 0, "Customize your game"),  
         new KnowledgeEntry (389, ContentFileType.FileUI, 0, "[empty slot] Save Game"),  
         new KnowledgeEntry (390, ContentFileType.FileUI, 0, "[empty slot] Save Game"),  
         new KnowledgeEntry (391, ContentFileType.FileUI, 0, "Quit to DOS or Main Menu"),  
         new KnowledgeEntry (392, ContentFileType.FileUI, 0, "Quit to DOS or Main Menu"),  
         new KnowledgeEntry (393, ContentFileType.FileUI, 0, "Volume in drive C"),  
         new KnowledgeEntry (394, ContentFileType.FileUI, 0, "Volume in drive C"),  
         new KnowledgeEntry (395, ContentFileType.FileUI, 0, "Connecting ..."),  
         new KnowledgeEntry (396, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (397, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (398, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (399, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (400, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (401, ContentFileType.FileUI, 0, "Set Army"),  
         new KnowledgeEntry (402, ContentFileType.FileUI, 0, "Master has control"),  
         new KnowledgeEntry (403, ContentFileType.FileUI, 0, "Slave has control"),  
         new KnowledgeEntry (404, ContentFileType.FileUI, 0, "Set Player Races"),
         new KnowledgeEntry (405, ContentFileType.FileUI, 0, "Set Player Races"),  
         new KnowledgeEntry (406, ContentFileType.FileImage, 255, "Gold (Humans)"),
         new KnowledgeEntry (407, ContentFileType.FileImage, 255, "Lumber (Humans)"),
         new KnowledgeEntry (408, ContentFileType.FileImage, 217, "Gold (Orcs)"),
         new KnowledgeEntry (409, ContentFileType.FileImage, 217, "Lumber (Orcs)"),
         new KnowledgeEntry (410, ContentFileType.FileImage, 413, "%complete"),
         new KnowledgeEntry (411, ContentFileType.FileImage, 413, "Human point summary"),
         new KnowledgeEntry (412, ContentFileType.FileImage, 413, "Orc point summary"),
         new KnowledgeEntry (413, ContentFileType.FilePalette, 0, "Summary palette"),
         new KnowledgeEntry (414, ContentFileType.FilePalette, 0, "Unknown pal"),
         new KnowledgeEntry (415, ContentFileType.FileImage, 416, "Background Victory"),
         new KnowledgeEntry (416, ContentFileType.FilePalette, 0, "for 415"),
         new KnowledgeEntry (417, ContentFileType.FileImage, 418, "Background Defeat"),
         new KnowledgeEntry (418, ContentFileType.FilePalette, 0, "for 417"),
         new KnowledgeEntry (419, ContentFileType.FileImage, 423, "Victory Text"),
         new KnowledgeEntry (420, ContentFileType.FileImage, 423, "Defeat Text"),
         new KnowledgeEntry (421, ContentFileType.FileImage, 423, "Human Briefing"),
         new KnowledgeEntry (422, ContentFileType.FileImage, 424, "Orc Briefing"),
         new KnowledgeEntry (423, ContentFileType.FilePalette, 0, "Briefing Palette 1"),
         new KnowledgeEntry (424, ContentFileType.FilePalette, 0, "Briefing Palette 2"),
         new KnowledgeEntry (425, ContentFileType.FileSprite, 424, "Burning Fire Anim"),
         new KnowledgeEntry (426, ContentFileType.FileSprite, 424, "Talking Orc Anim"),
         new KnowledgeEntry (427, ContentFileType.FileSprite, 424, "Talking Orc 2 Anim"),
         new KnowledgeEntry (428, ContentFileType.FileSprite, 423, "Eating Human Anim"),
         new KnowledgeEntry (429, ContentFileType.FileSprite, 423, "Human Wizard Anim"),
         new KnowledgeEntry (430, ContentFileType.FileSprite, 423, "Anim 1"),
         new KnowledgeEntry (431, ContentFileType.FileSprite, 423, "Anim 2"),
         new KnowledgeEntry (432, ContentFileType.FileText, 1, "Orcs 1"),
         new KnowledgeEntry (433, ContentFileType.FileText, 2, "Orcs 2"),
         new KnowledgeEntry (434, ContentFileType.FileText, 3, "Orcs 3"),
         new KnowledgeEntry (435, ContentFileType.FileText, 4, "Orcs 4"),
         new KnowledgeEntry (436, ContentFileType.FileText, 5, "Orcs 5"),
         new KnowledgeEntry (437, ContentFileType.FileText, 6, "Orcs 6"),
         new KnowledgeEntry (438, ContentFileType.FileText, 7, "Orcs 7"),
         new KnowledgeEntry (439, ContentFileType.FileText, 8, "Orcs 8"),
         new KnowledgeEntry (440, ContentFileType.FileText, 9, "Orcs 9"),
         new KnowledgeEntry (441, ContentFileType.FileText, 10, "Orcs 10"),
         new KnowledgeEntry (442, ContentFileType.FileText, 11, "Orcs 11"),
         new KnowledgeEntry (443, ContentFileType.FileText, 12, "Orcs 12"),
         new KnowledgeEntry (444, ContentFileType.FileText, 1, "Humans 1"),
         new KnowledgeEntry (445, ContentFileType.FileText, 2, "Humans 2"),
         new KnowledgeEntry (446, ContentFileType.FileText, 3, "Humans 3"),
         new KnowledgeEntry (447, ContentFileType.FileText, 4, "Humans 4"),
         new KnowledgeEntry (448, ContentFileType.FileText, 5, "Humans 5"),
         new KnowledgeEntry (449, ContentFileType.FileText, 6, "Humans 6"),
         new KnowledgeEntry (450, ContentFileType.FileText, 7, "Humans 7"),
         new KnowledgeEntry (451, ContentFileType.FileText, 8, "Humans 8"),
         new KnowledgeEntry (452, ContentFileType.FileText, 9, "Humans 9"),
         new KnowledgeEntry (453, ContentFileType.FileText, 10, "Humans 10"),
         new KnowledgeEntry (454, ContentFileType.FileText, 11, "Humans 11"),
         new KnowledgeEntry (455, ContentFileType.FileText, 12, "Humans 12"),
         new KnowledgeEntry (456, ContentFileType.FileImage, 457, "Human Win"),
         new KnowledgeEntry (457, ContentFileType.FilePalette, 0, "for 456"),
         new KnowledgeEntry (458, ContentFileType.FileImage, 459, "Orc Win"),
         new KnowledgeEntry (459, ContentFileType.FilePalette, 0, "for 458"),
         new KnowledgeEntry (460, ContentFileType.FileEmbeddedData, 0, "Unknown embedded Data file (entry count, offsets, data)"),
         new KnowledgeEntry (461, ContentFileType.FileText, 0, "Endgametext: Humans 1"),
         new KnowledgeEntry (462, ContentFileType.FileText, 0, "Endgametext: Orcs 1"),
         new KnowledgeEntry (463, ContentFileType.FileText, 0, "Endgametext: Humans 2"),
         new KnowledgeEntry (464, ContentFileType.FileText, 0, "Endgametext: Orcs 2"),
         new KnowledgeEntry (465, ContentFileType.FileText, 0, "Credits"),
         new KnowledgeEntry (466, ContentFileType.FileText, 0, "Level won (Humans?)"),
         new KnowledgeEntry (467, ContentFileType.FileText, 0, "Level won (Orcs?)"),
         new KnowledgeEntry (468, ContentFileType.FileText, 0, "Level lost (Humans?)"),
         new KnowledgeEntry (469, ContentFileType.FileText, 0, "Level lost (Orcs?)"),
         new KnowledgeEntry (470, ContentFileType.FileImage, 457, "Human Win/Animation 2"),
         new KnowledgeEntry (471, ContentFileType.FileImage, 459, "Orc Win/Animation 2"),
         new KnowledgeEntry (472, ContentFileType.FileWave, 0, "Blizzard"),
         new KnowledgeEntry (473, ContentFileType.FileWave, 0, "Opening Gate"),
         new KnowledgeEntry (474, ContentFileType.FileVOC, 0, "Construction"),
         new KnowledgeEntry (475, ContentFileType.FileVOC, 0, "Boom"),
         new KnowledgeEntry (476, ContentFileType.FileVOC, 0, "Catapult Fire"),
         new KnowledgeEntry (477, ContentFileType.FileVOC, 0, "Chop1"),
         new KnowledgeEntry (478, ContentFileType.FileVOC, 0, "Chop2"),
         new KnowledgeEntry (479, ContentFileType.FileVOC, 0, "Chop3"),
         new KnowledgeEntry (480, ContentFileType.FileVOC, 0, "Chop4"),
         new KnowledgeEntry (481, ContentFileType.FileVOC, 0, "Destruction1"),
         new KnowledgeEntry (482, ContentFileType.FileVOC, 0, "Destruction2"),
         new KnowledgeEntry (483, ContentFileType.FileVOC, 0, "Destruction3"),
         new KnowledgeEntry (484, ContentFileType.FileVOC, 0, "Bling"),
         new KnowledgeEntry (485, ContentFileType.FileWave, 0, "Clicking noise"),
         new KnowledgeEntry (486, ContentFileType.FileVOC, 0, "Spell1"),
         new KnowledgeEntry (487, ContentFileType.FileVOC, 0, "Slash1"),
         new KnowledgeEntry (488, ContentFileType.FileVOC, 0, "Slash2"),
         new KnowledgeEntry (489, ContentFileType.FileVOC, 0, "Slash3"),
         new KnowledgeEntry (490, ContentFileType.FileVOC, 0, "Weapon1"),
         new KnowledgeEntry (491, ContentFileType.FileVOC, 0, "Weapon2"),
         new KnowledgeEntry (492, ContentFileType.FileVOC, 0, "Spell2"),
         new KnowledgeEntry (493, ContentFileType.FileVOC, 0, "Arrow1"),
         new KnowledgeEntry (494, ContentFileType.FileVOC, 0, "Arrow2"),
         new KnowledgeEntry (495, ContentFileType.FileVOC, (int)Race.Orcs, "The humans draw near"),
         new KnowledgeEntry (496, ContentFileType.FileVOC, (int)Race.Orcs, "The pale dogs approach"),
         new KnowledgeEntry (497, ContentFileType.FileWave, (int)Race.Humans, "The Orcs are approaching"),
         new KnowledgeEntry (498, ContentFileType.FileWave, (int)Race.Humans, "There are enemies nearby"),
         new KnowledgeEntry (499, ContentFileType.FileVOC, (int)Race.Orcs, "Orc death"),
         new KnowledgeEntry (500, ContentFileType.FileVOC, (int)Race.Humans, "Human death"),
         new KnowledgeEntry (501, ContentFileType.FileVOC, (int)Race.Orcs, "Work completed"),
         new KnowledgeEntry (502, ContentFileType.FileWave, (int)Race.Humans, "Work completed"),
         new KnowledgeEntry (503, ContentFileType.FileVOC, (int)Race.Orcs, "We are being attacked"),
         new KnowledgeEntry (504, ContentFileType.FileWave, (int)Race.Orcs, "They're destroying our city"),
         new KnowledgeEntry (505, ContentFileType.FileWave, (int)Race.Humans, "We are under attack"),
         new KnowledgeEntry (506, ContentFileType.FileWave, (int)Race.Humans, "The town is under attack"),
         new KnowledgeEntry (507, ContentFileType.FileVOC, (int)Race.Orcs, "Your command, master"),
         new KnowledgeEntry (508, ContentFileType.FileWave, (int)Race.Humans, "Your command"),
         new KnowledgeEntry (509, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote1"),
         new KnowledgeEntry (510, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote2"),
         new KnowledgeEntry (511, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote3"),
         new KnowledgeEntry (512, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote4"),
         new KnowledgeEntry (513, ContentFileType.FileWave, (int)Race.Humans, "Yes"),
         new KnowledgeEntry (514, ContentFileType.FileWave, (int)Race.Humans, "Yes, mylord"),
         new KnowledgeEntry (515, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote5"),
         new KnowledgeEntry (516, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote6"),
         new KnowledgeEntry (517, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote7"),
         new KnowledgeEntry (518, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote8"),
         new KnowledgeEntry (519, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote9"),
         new KnowledgeEntry (520, ContentFileType.FileWave, (int)Race.Humans, "Yes?"),
         new KnowledgeEntry (521, ContentFileType.FileWave, (int)Race.Humans, "Your will, sire?"),
         new KnowledgeEntry (522, ContentFileType.FileWave, (int)Race.Humans, "Mylord?"),
         new KnowledgeEntry (523, ContentFileType.FileWave, (int)Race.Humans, "My liege?"),
         new KnowledgeEntry (524, ContentFileType.FileWave, (int)Race.Humans, "Your bidding?"),
         new KnowledgeEntry (525, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote10"),
         new KnowledgeEntry (526, ContentFileType.FileVOC, (int)Race.Orcs, "OrcEmote11"),
         new KnowledgeEntry (527, ContentFileType.FileWave, (int)Race.Orcs, "Stop poking me"),
         new KnowledgeEntry (528, ContentFileType.FileWave, (int)Race.Humans, "What?!"),
         new KnowledgeEntry (529, ContentFileType.FileWave, (int)Race.Humans, "What do you want?!"),
         new KnowledgeEntry (530, ContentFileType.FileWave, (int)Race.Humans, "Why do you keep touching me?!"),
         new KnowledgeEntry (531, ContentFileType.FileWave, 0, "Splatter"),
         new KnowledgeEntry (532, ContentFileType.FileWave, 0, "Spell"),
         new KnowledgeEntry (533, ContentFileType.FileWave, 0, "Placement"),
         new KnowledgeEntry (534, ContentFileType.FileWave, (int)Race.Orcs, "Orc Tower"),
         new KnowledgeEntry (535, ContentFileType.FileWave, (int)Race.Humans, "Human Church"),
         new KnowledgeEntry (536, ContentFileType.FileWave, (int)Race.Orcs, "Kennel"),
         new KnowledgeEntry (537, ContentFileType.FileWave, (int)Race.Humans, "Stables"),
         new KnowledgeEntry (538, ContentFileType.FileWave, (int)Race.Humans, "Smith"),
         new KnowledgeEntry (539, ContentFileType.FileWave, 0, "Burning Fire"),
         new KnowledgeEntry (540, ContentFileType.FileWave, 0, "Boom"),
         new KnowledgeEntry (541, ContentFileType.FileWave, 0, "Boom2"),
         new KnowledgeEntry (542, ContentFileType.FileWave, (int)Race.Humans, "Human Ending"),
         new KnowledgeEntry (543, ContentFileType.FileWave, 0, "The duty of"),
         new KnowledgeEntry (544, ContentFileType.FileWave, 0, "With the decemation"),
         new KnowledgeEntry (545, ContentFileType.FileWave, 0, "What new conquests"),
         new KnowledgeEntry (546, ContentFileType.FileWave, 0, "In the age of chaos"),
         new KnowledgeEntry (547, ContentFileType.FileWave, 0, "The kingdom of Azeroth was"),
         new KnowledgeEntry (548, ContentFileType.FileWave, 0, "No one knew where these"),
         new KnowledgeEntry (549, ContentFileType.FileWave, 0, "With an ingenious"),
         new KnowledgeEntry (550, ContentFileType.FileWave, 0, "Welcome to the World of Warcraft"),
         new KnowledgeEntry (551, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans1"),
         new KnowledgeEntry (552, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans2"),
         new KnowledgeEntry (553, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans3"),
         new KnowledgeEntry (554, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans4"),
         new KnowledgeEntry (555, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans5"),
         new KnowledgeEntry (556, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans6"),
         new KnowledgeEntry (557, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans7"),
         new KnowledgeEntry (558, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans8"),
         new KnowledgeEntry (559, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans9"),
         new KnowledgeEntry (560, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans10"),
         new KnowledgeEntry (561, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans11"),
         new KnowledgeEntry (562, ContentFileType.FileWave, (int)Race.Humans, "Briefing Humans12"),
         new KnowledgeEntry (563, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs1"),
         new KnowledgeEntry (564, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs2"),
         new KnowledgeEntry (565, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs3"),
         new KnowledgeEntry (566, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs4"),
         new KnowledgeEntry (567, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs5"),
         new KnowledgeEntry (568, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs6"),
         new KnowledgeEntry (569, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs7"),
         new KnowledgeEntry (570, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs8"),
         new KnowledgeEntry (571, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs9"),
         new KnowledgeEntry (572, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs10"),
         new KnowledgeEntry (573, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs11"),
         new KnowledgeEntry (574, ContentFileType.FileWave, (int)Race.Orcs, "Briefing Orcs12"),
         new KnowledgeEntry (575, ContentFileType.FileWave, (int)Race.Humans, "Your failure in battle"),
         new KnowledgeEntry (576, ContentFileType.FileWave, (int)Race.Orcs, "You pitiful"),
         new KnowledgeEntry (577, ContentFileType.FileWave, (int)Race.Orcs, "The feel of bones"),
         new KnowledgeEntry (578, ContentFileType.FileWave, (int)Race.Orcs, "If only the worthless"),
         new KnowledgeEntry (579, ContentFileType.FileWave, (int)Race.Orcs, "Gaze upon the destruction"),
         new KnowledgeEntry (580, ContentFileType.FileWave, (int)Race.Humans, "The forces of darkness"),
         new KnowledgeEntry (581, ContentFileType.FileWave, (int)Race.Humans, "Even these children"),
         new KnowledgeEntry (582, ContentFileType.FileWave, (int)Race.Humans, "Cheers of victory"),
      };
      #endregion

      private Dictionary<string, int> iconDatabase;

      private KnowledgeEntry[] database;
      private Dictionary<string, int> hashes;

      internal DataWarFileType Type { get; private set; }

      internal KnowledgeBase(DataWarFileType setType)
      {
         Type = setType;

         SetupDatabase ();
      }

      private void SetupDatabase()
      {
         iconDatabase = new Dictionary<string, int>();
         SetupIconDatabase();

         database = new KnowledgeEntry[KnowledgeBaseEntries.Length];
         Array.Copy(KnowledgeBaseEntries, database, KnowledgeBaseEntries.Length);

         hashes = new Dictionary<string, int> (database.Length);
         for (int i = 0; i < database.Length; i++)
         {
            if (database[i] == null || database[i].text == null)
            {
               continue;
            }

            if (hashes.ContainsKey(database[i].text.ToLowerInvariant()))
            {
               continue;
            }

            hashes.Add(database[i].text.ToLowerInvariant(), database[i].id);
         }
      }

      public KnowledgeEntry this[string name]
      {
         get
         {
            name = name.ToLowerInvariant ();
            if (hashes.ContainsKey(name) == false)
               return null;

            return database[hashes[name]];
         }
      }

      public KnowledgeEntry this[int index]
      {
         get
         {
            if (index < 0 || index >= database.Length)
               return null;

            return database[index];
         }
      }

      public int IndexByName (string name)
      {
         name = name.ToLowerInvariant ();

         if (hashes.ContainsKey (name))
         {
            return hashes[name];
         }

         return -1;
      }

      public int IconIDByName(string name)
      {
         if (iconDatabase.ContainsKey(name) == false)
            return -1;

         return iconDatabase[name];
      }

      public int Count
      {
         get
         {
            return database.Length;
         }
      }

      private void SetupIconDatabase()
      {
         iconDatabase.Add("Footman", 0);
         iconDatabase.Add("Grunt", 1);
         iconDatabase.Add("Conjurer", 2);
         iconDatabase.Add("Warlock", 3);
         iconDatabase.Add("Peasant", 4);
         iconDatabase.Add("Peon", 5);
         iconDatabase.Add("CatapultHumans", 6);
         iconDatabase.Add("CatapultOrcs", 7);
         iconDatabase.Add("Knight", 8);
         iconDatabase.Add("Raider", 9);
         iconDatabase.Add("Archer", 10);
         iconDatabase.Add("Spearman", 11);
         iconDatabase.Add("Cleric", 12);
         iconDatabase.Add("Necrolyte", 13);
         iconDatabase.Add("FarmHumans", 14);
         iconDatabase.Add("FarmOrc", 15);
         iconDatabase.Add("BarracksHumans", 16);
         iconDatabase.Add("BarracksOrcs", 17);
         iconDatabase.Add("TowerHumans", 18);
         iconDatabase.Add("TowerOrcs", 19);
         iconDatabase.Add("TownhallHumans", 20);
         iconDatabase.Add("TownhallOrcs", 21);
         iconDatabase.Add("LumbermillHumans", 22);
         iconDatabase.Add("LumbermillOrcs", 23);
         iconDatabase.Add("Stables", 24);
         iconDatabase.Add("Kennel", 25);
         iconDatabase.Add("BlacksmithHumans", 26);
         iconDatabase.Add("BlacksmithOrcs", 27);
         iconDatabase.Add("Church", 28);
         iconDatabase.Add("Temple", 29);
         iconDatabase.Add("Goldmine", 30);
         iconDatabase.Add("Stormwind", 31);
         iconDatabase.Add("BlackRock", 32);
         iconDatabase.Add("MoveHumans", 33);
         iconDatabase.Add("MoveOrcs", 34);
         iconDatabase.Add("Repair", 35);
         iconDatabase.Add("Harvest", 36);
         iconDatabase.Add("Build", 37);
         iconDatabase.Add("BuildAdvanced", 38);
         iconDatabase.Add("ReturnLumber", 39);
         iconDatabase.Add("Cancel", 40);
         iconDatabase.Add("Wall", 41);
         iconDatabase.Add("Road", 42);
         iconDatabase.Add("Unk", 43);
         iconDatabase.Add("Ogre", 44);
         iconDatabase.Add("Spider", 45);
         iconDatabase.Add("Slime", 46);
         iconDatabase.Add("FireElemental", 47);
         iconDatabase.Add("Scorpion", 48);
         iconDatabase.Add("OrcSkeleton", 49);
         iconDatabase.Add("Skeleton", 50);
         iconDatabase.Add("Daemon", 51);
         iconDatabase.Add("WaterElemental", 52);
         iconDatabase.Add("Lothar", 53);
         iconDatabase.Add("Medivh", 54);
         iconDatabase.Add("Garona", 55);
         iconDatabase.Add("Griselda", 56);
         iconDatabase.Add("Wounded", 57);
         iconDatabase.Add("Brigand", 58);
         iconDatabase.Add("Holy Lance", 59); // (Attack, Cleric)
         iconDatabase.Add("Elemental Blast", 60); // (Attack, Conjurer)
         iconDatabase.Add("Shadow Spear", 61); // (Attack, Necrolyte)
         iconDatabase.Add("Fireball", 62); // (Attack, Warlock)
         iconDatabase.Add("Sword1", 63);
         iconDatabase.Add("Sword2", 64);
         iconDatabase.Add("Sword3", 65);
         iconDatabase.Add("Axe1", 66);
         iconDatabase.Add("Axe2", 67);
         iconDatabase.Add("Axe3", 68);
         iconDatabase.Add("Wolf1", 69);
         iconDatabase.Add("Wolf2", 70);
         iconDatabase.Add("Crossbar1", 71);
         iconDatabase.Add("Crossbar2", 72);
         iconDatabase.Add("Crossbar3", 73);
         iconDatabase.Add("Spear1", 74);
         iconDatabase.Add("Spear2", 75);
         iconDatabase.Add("Spear3", 76);
         iconDatabase.Add("Horse1", 77);
         iconDatabase.Add("Horse2", 78);
         iconDatabase.Add("Shield1", 79);
         iconDatabase.Add("Shield2", 80);
         iconDatabase.Add("Shield3", 81);
         iconDatabase.Add("Shield1Orcs", 82);
         iconDatabase.Add("Shield2Orcs", 83);
         iconDatabase.Add("Shield3Orcs", 84);
         iconDatabase.Add("Heal", 85);
         iconDatabase.Add("Far Sight", 86);
         iconDatabase.Add("Invisibility", 87);
         iconDatabase.Add("Rain of Fire", 88);
         iconDatabase.Add("Raising Dead", 89);
         iconDatabase.Add("Dark Vision", 90);
         iconDatabase.Add("Unholy Armor", 91);
         iconDatabase.Add("Cloud of Poison", 92);
      }

      internal void DumpResource(int index, WarResource res, bool writeToConsole = true, string filename = null)
      {
#if !NETFX_CORE
         if (index < 0 || index >= Count ||
             this[index] == null || res == null)
         {
            return;
         }

         if (writeToConsole)
         {
            Console.WriteLine(index.ToString("000") + ": " + this[index].text + " (" + res.data.Length + " bytes)");
            Console.Write(((0 / 64) * 64).ToString("x4") + ": ");
            for (int b = 0; b < res.data.Length; b++)
            {
               Console.Write(res.data[b].ToString("x2"));
               Console.Write(" ");
               if (b % 64 == 63)
               {
                  Console.WriteLine();
                  Console.Write((((b + 1) / 64) * 64).ToString("x4") + ": ");
               }
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int b = 0; b < res.data.Length / 2; b++)
            {
               if (res.data[b] >= 32)
                  Console.Write((char)res.data[b] + " ");
               else
                  Console.Write("   ");
               if (b % 64 == 63)
               {
                  Console.WriteLine();
                  Console.Write((((b + 1) / 64) * 64).ToString("x4") + ": ");
               }
            }
            Console.WriteLine();
         }

         if (filename != null)
         {
            System.IO.File.WriteAllBytes(filename, res.data);
         }
#endif
      }
   }
}

/*
FileLevelInfo:
Offset            Data
------------------------------------------------
0x0000            Unbekannte Daten (Header)     (Length 0x36)
0x003A            Unbekannte Daten           (Length 0x20)
0x005C            Initial amount: Lumber
0x0070            Initial amount: Gold
0x0094            Offset of mission text
0x                Offset of mission data (Buildings, Units, etc...)
   (z.B. bei 0x0111 in 118)

0x00CC            X-Position der "Kamera" (multipliziert mit 2)
0x00CE            Y-Position der "Kamera" (multipliziert mit 2)

Wege (5 bytes pro Weg)
2 bytes: X,Y vom Anfang (mul 2)
2 bytes: X,Y vom Ende (mul 2)
1 byte: ???? (Owner vielleicht?)

Einheiten/Geb�ude (4 (bzw.6) bytes lang):
1 byte: X-Position (mul 2)
1 byte: Y-Position (mul 2)
1 byte: Typ
1 byte: Seite (0 = Player; 1 = Computer, 4 = Neutral)
Wenn Typ==32 (Goldmine)
1 byte: 0xFE
1 byte: Menge an Gold (multiply by 250)
*/


/*
      #region Hardcoded palette
      // Number of entries = 128 * 3 = 384
      // 0, 0, 0 => means unknown
      internal static byte[] hardcoded_pal = {
         0, 0, 0, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         0, 0, 0, // 4
         0, 0, 0, // 5
         64, 72, 40, // 6
         0, 0, 0, // 7
         8, 32, 8,   // 8
         0, 0, 0, // 9
         0, 0, 0, // 10
         24, 36, 16, // 11
         4, 20, 8,   // 12
         0, 0, 0, // 13
         0, 0, 0, // 14
         0, 0, 0, // 15
         0, 0, 0, // 16
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         255, 255, 255, // not part of the hardcoded palette
         252, 252, 252, // 32
         224, 232, 224, // 33
         200, 212, 200, // 34
         180, 192, 180, // 35
         156, 172, 156, // 36
         136, 152, 136, // 37
         116, 132, 116, // 38
         96, 112, 96,      // 39
         84, 100, 84,   // 40
         76, 92, 84,    // 41
         64, 76, 68, // 42
         52, 64, 64, // 43
         39, 51, 51, // 44
         32, 40, 44,    // 45
         24, 32, 36, // 46
         16, 24, 24, // 47
         164, 136, 68,  // 48
         148, 120, 52,  // 49
         132, 104, 40,  // 50
         116, 88, 28,   // 51
         96, 72, 16, // 52
         84, 56, 8,  // 53
         68, 44, 4,  // 54
         56, 36, 4,  // 55
         42, 27, 3,  // 56
         30, 19, 0,  // 57
         0, 0, 0, // 58
         0, 0, 0, // 59
         0, 0, 0, // 60
         140, 144, 8,   // 61
         12, 148, 0, // 62
         8, 108, 0,  // 63
         0, 0, 0, // 64
         0, 0, 0, // 65
         68, 68, 0,  // 66
         101, 93, 77,   // 67
         0, 0, 0, // 68
         0, 0, 0, // 69
         0, 0, 0, // 70
         0, 0, 0, // 71
         0, 0, 0, // 72
         0, 0, 0, // 73    known! ==> black (actually: transparent shadow!)
         48, 32, 28, // 74
         56, 36, 28, // 75
         64, 44, 32, // 76
         76, 51, 36, // 77
         88, 60, 40, // 78
         104, 72, 48,   // 79
         92, 68, 48, // 80
         100, 76, 48,   // 81
         108, 84, 52,   // 82
         116, 92, 56,   // 83
         120, 100, 60,  // 84
         0, 0, 0, // 85
         0, 0, 0, // 86
         0, 0, 0, // 87
         0, 0, 0, // 88
         156, 40, 0, // 89
         252, 48, 0, // 90
         252, 84, 8, // 91
         252, 120, 24,  // 92
         252, 152, 40,  // 93
         252, 180, 56,  // 94
         252, 208, 72,  // 95
         0, 0, 0, // 96    known! ==> black (actually: transparent shadow!)
         0, 0, 0, // 97
         64, 20, 20, // 98
         88, 24, 24, // 99
         112, 112, 76,  // 100
         0, 0, 0, // 101
         0, 0, 0, // 102
         0, 0, 0, // 103
         0, 0, 0, // 104
         0, 0, 0, // 105
         0, 0, 0, // 106
         0, 0, 0, // 107
         0, 0, 0, // 108
         0, 0, 0, // 109
         0, 0, 0, // 110
         0, 0, 0, // 111
         0, 0, 0, // 112
         0, 0, 0, // 113
         0, 0, 0, // 114
         0, 0, 0, // 115
         0, 0, 0, // 116
         0, 0, 0, // 117
         0, 0, 0, // 118
         0, 0, 0, // 119
         0, 0, 0, // 120
         0, 0, 0, // 121
         0, 0, 0, // 122
         0, 0, 0, // 123
         0, 0, 0, // 124
         0, 0, 0, // 125
         0, 0, 0, // 126
         0, 0, 0,  // 127
         0, 0, 0,  // 128
         0, 0, 0,  // 129
         0, 0, 0,  // 130
         0, 0, 0,  // 131
         0, 0, 0,  // 132
         0, 0, 0,  // 133
         31, 14, 5,  // 134
         51, 27, 18,  // 135
         81, 39, 23,  // 136
         104, 51, 31,  // 137
         130, 68, 43,  // 138
         76, 51, 36,  // 139
         175, 101, 75,  // 140
         203, 120, 90,  // 141
         227, 141, 104,  // 142
         0, 0, 0,  // 143
         0, 0, 0,  // 144
         0, 0, 0,  // 145
         0, 0, 0,  // 146
         0, 0, 0,  // 147
         0, 0, 0,  // 148
         0, 0, 0,  // 149
         0, 0, 0,  // 150
         0, 0, 0,  // 151
         0, 0, 0,  // 152
         0, 0, 0,  // 153
         0, 0, 0,  // 154
         0, 0, 0,  // 155
         0, 0, 0,  // 156
         0, 0, 0,  // 157
         0, 0, 0,  // 158
         0, 0, 0,  // 159
         0, 0, 0,  // 160
         0, 0, 0,  // 161
         0, 0, 0,  // 162
         0, 0, 0,  // 163
         0, 0, 0,  // 164
         0, 0, 0,  // 165
         0, 0, 0,  // 166
         0, 0, 0,  // 167
         1, 23, 11,  // 168
         2, 34, 14,  // 169
         5, 51, 15,  // 170
         11, 69, 10,  // 171
         22, 85, 14,  // 172
         48, 108, 48,  // 173
         72, 139, 17,  // 174
         109, 166, 21,  // 175
         56, 0, 0,  // 176
         72, 0, 0,  // 177
         92, 1, 0,  // 178
         106, 0, 7,  // 179
         134, 9, 9,  // 180
         154, 22, 29,  // 181
         181, 31, 24,  // 182
         207, 43, 27,  // 183
         11, 18, 11,  // 184
         27, 31, 31,  // 185
         54, 64, 69,  // 186
         89, 97, 105,  // 187
         117, 130, 141,  // 188
         146, 159, 177,  // 189
         178, 186, 211,  // 190
         211, 211, 239,  // 191
         0, 0, 0,  // 192
         182, 93, 52,  // 193
         215, 143, 80,  // 194
         249, 192, 112,  // 195
         214, 0, 22,  // 196
         78, 0, 13,  // 197
         0, 0, 0,  // 198
         0, 0, 0,  // 199
         0, 3, 35,  // 200
         3, 9, 64,  // 201
         10, 20, 96,  // 202
         18, 29, 129,  // 203
         27, 25, 153,  // 204
         40, 14, 181,  // 205
         0, 0, 0,  // 206
         0, 0, 0,  // 207
         0, 0, 0,  // 208
         0, 0, 0,  // 209
         0, 0, 0,  // 210
         0, 0, 0,  // 211
         0, 0, 0,  // 212
         0, 0, 0,  // 213
         0, 0, 0,  // 214
         0, 0, 0,  // 215
         0, 0, 0,  // 216
         0, 0, 0,  // 217
         0, 0, 0,  // 218
         0, 0, 0,  // 219
         0, 0, 0,  // 220
         0, 0, 0,  // 221
         0, 0, 0,  // 222
         0, 0, 0,  // 223
         0, 0, 0,  // 224
         0, 0, 0,  // 225
         0, 0, 0,  // 226
         0, 0, 0,  // 227
         0, 0, 0,  // 228
         0, 0, 0,  // 229
         0, 0, 0,  // 230
         0, 0, 0,  // 231
         0, 0, 0,  // 232
         0, 0, 0,  // 233
         0, 0, 0,  // 234
         0, 0, 0,  // 235
         0, 0, 0,  // 236
         0, 0, 0,  // 237
         0, 0, 0,  // 238
         0, 0, 0,  // 239
         0, 0, 0,  // 240
         0, 0, 0,  // 241
         0, 0, 0,  // 242
         0, 0, 0,  // 243
         0, 0, 0,  // 244
         0, 0, 0,  // 245
         0, 0, 0,  // 246
         0, 0, 0,  // 247
         0, 0, 0,  // 248
         0, 0, 0,  // 249
         0, 0, 0,  // 250
         0, 0, 0,  // 251
         0, 0, 0,  // 252
         0, 0, 0,  // 253
         0, 0, 0,  // 254
         0, 0, 0,  // 255
      };

      #endregion
      */
