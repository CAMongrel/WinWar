using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WinWarRT.Data
{
	internal enum WarFileType : int
	{
		FileUnknown,
		FileImage,
		FilePalette,
		FileXMID,
		FileCursor,
		FileText,
		FileLevelInfo,
		FileLevelVisual,
		FileLevelPassable,
		FileSprite,
		FileBriefing,
		FileWave,
		FileVOC,
		FileTileSet,
		FileTiles,
		FileTilePAL
	}

	internal enum Tileset : int
	{
		Summer,
		Swamp,
		Dungeon
	}

	internal struct KnowledgeEntry
	{
		internal int id;
		internal WarFileType type;
		internal int param;
		internal string text;

		#region KnowledgeEntry
		internal KnowledgeEntry(int id, WarFileType type, int param, string text)
		{
			this.id = id;
			this.type = type;
			this.param = param;
			this.text = text;
		}
		#endregion
	}

	internal class KnowledgeBase
	{
		#region Hardcoded palette
		// Number of entries = 128 * 3 = 384
        // 0, 0, 0 => means unknown
		internal static byte[] hardcoded_pal = {
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							0, 0, 0,	// 4
							0, 0, 0,	// 5
							64, 72, 40,	// 6
							0, 0, 0,	// 7
							0, 0, 0,	// 8
							0, 0, 0,	// 9
							0, 0, 0,	// 10
							24, 36, 16,	// 11
							0, 0, 0,	// 12
							0, 0, 0,	// 13
							0, 0, 0,	// 14
							0, 0, 0,	// 15
							0, 0, 0,	// 16
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							255, 255, 255,	// not part of the hardcoded palette
							252, 252, 252,	// 32
							224, 232, 224,	// 33
							200, 212, 200,	// 34
							180, 192, 180,	// 35
							156, 172, 156,	// 36
							136, 152, 136,	// 37
							116, 132, 116,	// 38
							96, 112, 96,		// 39
							84, 100, 84,	// 40
							76, 92, 84,		// 41
							64, 76, 68,	// 42
							52, 64, 64,	// 43
							0, 0, 0,	// 44
							32, 40, 44,		// 45
							24, 32, 36,	// 46
							16, 24, 24,	// 47
							164, 136, 68,	// 48
							148, 120, 52,	// 49
							132, 104, 40,	// 50
							116, 88, 28,	// 51
							96, 72, 16,	// 52
							84, 56, 8,	// 53
							68, 44, 4,	// 54
							56, 36, 4,	// 55
							0, 0, 0,	// 56
							0, 0, 0,	// 57
							0, 0, 0,	// 58
							0, 0, 0,	// 59
							0, 0, 0,	// 60
							140, 144, 8,	// 61
							12, 148, 0,	// 62
							8, 108, 0,	// 63
							0, 0, 0,	// 64
							0, 0, 0,	// 65
							68, 68, 0,	// 66
							0, 0, 0,	// 67
							0, 0, 0,	// 68
							0, 0, 0,	// 69
							0, 0, 0,	// 70
							0, 0, 0,	// 71
							0, 0, 0,	// 72
							0, 0, 0,	// 73
							48, 32, 28,	// 74
							56, 36, 28,	// 75
							64, 44, 32,	// 76
							0, 0, 0,	// 77
							88, 60, 40,	// 78
							104, 72, 48,	// 79
							92, 68, 48,	// 80
							100, 76, 48,	// 81
							108, 84, 52,	// 82
							116, 92, 56,	// 83
							120, 100, 60,	// 84
							0, 0, 0,	// 85
							0, 0, 0,	// 86
							0, 0, 0,	// 87
							0, 0, 0,	// 88
							156, 40, 0,	// 89
							252, 48, 0,	// 90
							252, 84, 8,	// 91
							252, 120, 24,	// 92
							252, 152, 40,	// 93
							252, 180, 56,	// 94
							252, 208, 72,	// 95
							0, 0, 0,	// 96		known! ==> black
							0, 0, 0,	// 97
							64, 20, 20,	// 98
							88, 24, 24,	// 99
							112, 112, 76,	// 100
							0, 0, 0,	// 101
							0, 0, 0,	// 102
							0, 0, 0,	// 103
							0, 0, 0,	// 104
							0, 0, 0,	// 105
							0, 0, 0,	// 106
							0, 0, 0,	// 107
							0, 0, 0,	// 108
							0, 0, 0,	// 109
							0, 0, 0,	// 110
							0, 0, 0,	// 111
							0, 0, 0,	// 112
							0, 0, 0,	// 113
							0, 0, 0,	// 114
							0, 0, 0,	// 115
							0, 0, 0,	// 116
							0, 0, 0,	// 117
							0, 0, 0,	// 118
							0, 0, 0,	// 119
							0, 0, 0,	// 120
							0, 0, 0,	// 121
							0, 0, 0,	// 122
							0, 0, 0,	// 123
							0, 0, 0,	// 124
							0, 0, 0,	// 125
							0, 0, 0,	// 126
							0, 0, 0	// 127
						};
		#endregion

		//internal const int KB_Size = 370;
		internal static KnowledgeEntry[] KB_List = 
			{
				new KnowledgeEntry(0, WarFileType.FileXMID, 0, "Music0"),
				new KnowledgeEntry(1, WarFileType.FileXMID, 0, "Music1"),
				new KnowledgeEntry(2, WarFileType.FileXMID, 0, "Music2"),
				new KnowledgeEntry(3, WarFileType.FileXMID, 0, "Music3"),
				new KnowledgeEntry(4, WarFileType.FileXMID, 0, "Music4"),
				new KnowledgeEntry(5, WarFileType.FileXMID, 0, "Music5"),
				new KnowledgeEntry(6, WarFileType.FileXMID, 0, "Music6"),
				new KnowledgeEntry(7, WarFileType.FileXMID, 0, "Music7"),
				new KnowledgeEntry(8, WarFileType.FileXMID, 0, "Music8"),
				new KnowledgeEntry(9, WarFileType.FileXMID, 0, "Music9"),
				new KnowledgeEntry(10, WarFileType.FileXMID, 0, "Music10"),
				new KnowledgeEntry(11, WarFileType.FileXMID, 0, "Music11"),
				new KnowledgeEntry(12, WarFileType.FileXMID, 0, "Music12"),
				new KnowledgeEntry(13, WarFileType.FileXMID, 0, "Music13"),
				new KnowledgeEntry(14, WarFileType.FileXMID, 0, "Music14"),
				new KnowledgeEntry(15, WarFileType.FileXMID, 0, "Music15"),
				new KnowledgeEntry(16, WarFileType.FileXMID, 0, "Music16"),
				new KnowledgeEntry(17, WarFileType.FileXMID, 0, "Music17"),
				new KnowledgeEntry(18, WarFileType.FileXMID, 0, "Music18"),
				new KnowledgeEntry(19, WarFileType.FileXMID, 0, "Music19"),
				new KnowledgeEntry(20, WarFileType.FileXMID, 0, "Music20"),
				new KnowledgeEntry(21, WarFileType.FileXMID, 0, "Music21"),
				new KnowledgeEntry(22, WarFileType.FileXMID, 0, "Music22"),
				new KnowledgeEntry(23, WarFileType.FileXMID, 0, "Music23"),
				new KnowledgeEntry(24, WarFileType.FileXMID, 0, "Music24"),
				new KnowledgeEntry(25, WarFileType.FileXMID, 0, "Music25"),
				new KnowledgeEntry(26, WarFileType.FileXMID, 0, "Music26"),
				new KnowledgeEntry(27, WarFileType.FileXMID, 0, "Music27"),
				new KnowledgeEntry(28, WarFileType.FileXMID, 0, "Music28"),
				new KnowledgeEntry(29, WarFileType.FileXMID, 0, "Music29"),
				new KnowledgeEntry(30, WarFileType.FileXMID, 0, "Music30"),
				new KnowledgeEntry(31, WarFileType.FileXMID, 0, "Music31"),
				new KnowledgeEntry(32, WarFileType.FileXMID, 0, "Music32"),
				new KnowledgeEntry(33, WarFileType.FileXMID, 0, "Music33"),
				new KnowledgeEntry(34, WarFileType.FileXMID, 0, "Music34"),
				new KnowledgeEntry(35, WarFileType.FileXMID, 0, "Music35"),
				new KnowledgeEntry(36, WarFileType.FileXMID, 0, "Music36"),
				new KnowledgeEntry(37, WarFileType.FileXMID, 0, "Music37"),
				new KnowledgeEntry(38, WarFileType.FileXMID, 0, "Music38"),
				new KnowledgeEntry(39, WarFileType.FileXMID, 0, "Music39"),
				new KnowledgeEntry(40, WarFileType.FileXMID, 0, "Music40"),
				new KnowledgeEntry(41, WarFileType.FileXMID, 0, "Music41"),
				new KnowledgeEntry(42, WarFileType.FileXMID, 0, "Music42"),
				new KnowledgeEntry(43, WarFileType.FileXMID, 0, "Music43"),
				new KnowledgeEntry(44, WarFileType.FileXMID, 0, "Music44"),
				new KnowledgeEntry(45, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Orcs 11 (Visual)"),
				new KnowledgeEntry(46, WarFileType.FileLevelPassable, 0, "Orcs 11 (Passable)"),
				new KnowledgeEntry(47, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Humans 6 (Visual)"),
				new KnowledgeEntry(48, WarFileType.FileLevelPassable, 0, "Humans 6 (Passable)"),
				new KnowledgeEntry(49, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Orcs 3 (Visual)"),
				new KnowledgeEntry(50, WarFileType.FileLevelPassable, 0, "Orcs 3 (Passable)"),
				new KnowledgeEntry(51, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Custom Forest 1 (Visual)"),
				new KnowledgeEntry(52, WarFileType.FileLevelPassable, 0, "Custom Forest 1 (Passable)"),
				new KnowledgeEntry(53, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Orcs 10 (Visual)"),
				new KnowledgeEntry(54, WarFileType.FileLevelPassable, 0, "Orcs 10 (Passable)"),
				new KnowledgeEntry(55, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Humans 2 (Visual)"),
				new KnowledgeEntry(56, WarFileType.FileLevelPassable, 0, "Humans 2 (Passable)"),
				new KnowledgeEntry(57, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Humans 5 (Visual)"),
				new KnowledgeEntry(58, WarFileType.FileLevelPassable, 0, "Humans 5 (Passable)"),
				new KnowledgeEntry(59, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Orcs 12 (Visual)"),
				new KnowledgeEntry(60, WarFileType.FileLevelPassable, 0, "Orcs 12 (Passable)"),
				new KnowledgeEntry(61, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Custom Forest 2 (Visual)"),
				new KnowledgeEntry(62, WarFileType.FileLevelPassable, 0, "Custom Forest 2 (Passable)"),
				new KnowledgeEntry(63, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Humans 1 (Visual)"),
				new KnowledgeEntry(64, WarFileType.FileLevelPassable, 0, "Humans 1 (Passable)"),
				new KnowledgeEntry(65, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Orcs 6 (Visual)"),
				new KnowledgeEntry(66, WarFileType.FileLevelPassable, 0, "Orcs 6 (Passable)"),
				new KnowledgeEntry(67, WarFileType.FileLevelVisual, (int)Tileset.Summer, "Humans 7 (Visual)"),
				new KnowledgeEntry(68, WarFileType.FileLevelPassable, 0, "Humans 7 (Passable)"),
				new KnowledgeEntry(69, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Humans 3 (Visual)"),
				new KnowledgeEntry(70, WarFileType.FileLevelPassable, 0, "Humans 3 (Passable)"),
				new KnowledgeEntry(71, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Humans 9 (Visual)"),
				new KnowledgeEntry(72, WarFileType.FileLevelPassable, 0, "Humans 9 (Passable)"),
				new KnowledgeEntry(73, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Humans 10 (Visual)"),
				new KnowledgeEntry(74, WarFileType.FileLevelPassable, 0, "Humans 10 (Passable)"),
				new KnowledgeEntry(75, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Humans 11 (Visual)"),
				new KnowledgeEntry(76, WarFileType.FileLevelPassable, 0, "Humans 11 (Passable)"),
				new KnowledgeEntry(77, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Humans 12 (Visual)"),
				new KnowledgeEntry(78, WarFileType.FileLevelPassable, 0, "Humans 12 (Passable)"),
				new KnowledgeEntry(79, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Orcs 1 (Visual)"),
				new KnowledgeEntry(80, WarFileType.FileLevelPassable, 0, "Orcs 1 (Passable)"),
				new KnowledgeEntry(81, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Orcs 2 (Visual)"),
				new KnowledgeEntry(82, WarFileType.FileLevelPassable, 0, "Orcs 2 (Passable)"),
				new KnowledgeEntry(83, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Orcs 5 (Visual)"),
				new KnowledgeEntry(84, WarFileType.FileLevelPassable, 0, "Orcs 5 (Passable)"),
				new KnowledgeEntry(85, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Orcs 7 (Visual)"),
				new KnowledgeEntry(86, WarFileType.FileLevelPassable, 0, "Orcs 7 (Passable)"),
				new KnowledgeEntry(87, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Orcs 9 (Visual)"),
				new KnowledgeEntry(88, WarFileType.FileLevelPassable, 0, "Orcs 9 (Passable)"),
				new KnowledgeEntry(89, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Custom Swamp 1 (Visual)"),
				new KnowledgeEntry(90, WarFileType.FileLevelPassable, 0, "Custom Swamp 1 (Passable)"),
				new KnowledgeEntry(91, WarFileType.FileLevelVisual, (int)Tileset.Swamp, "Custom Swamp 2 (Visual)"),
				new KnowledgeEntry(92, WarFileType.FileLevelPassable, 0, "Custom Swamp 2 (Passable)"),
				new KnowledgeEntry(93, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Orcs 4 (Visual)"),
				new KnowledgeEntry(94, WarFileType.FileLevelPassable, 0, "Orcs 4 (Passable)"),
				new KnowledgeEntry(95, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Humans 8 (Visual)"),
				new KnowledgeEntry(96, WarFileType.FileLevelPassable, 0, "Humans 8 (Passable)"),
				new KnowledgeEntry(97, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Humans 4 (Visual)"),
				new KnowledgeEntry(98, WarFileType.FileLevelPassable, 0, "Humans 4 (Passable)"),
				new KnowledgeEntry(99, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Orcs 8 (Visual)"),
				new KnowledgeEntry(100, WarFileType.FileLevelPassable, 0, "Orcs 8 (Passable)"),
				new KnowledgeEntry(101, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 1 (Visual)"),
				new KnowledgeEntry(102, WarFileType.FileLevelPassable, 0, "Custom Dungeon 1 (Passable)"),
				new KnowledgeEntry(103, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 2 (Visual)"),
				new KnowledgeEntry(104, WarFileType.FileLevelPassable, 0, "Custom Dungeon 2 (Passable)"),
				new KnowledgeEntry(105, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 3 (Visual)"),
				new KnowledgeEntry(106, WarFileType.FileLevelPassable, 0, "Custom Dungeon 3 (Passable)"),
				new KnowledgeEntry(107, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 4 (Visual)"),
				new KnowledgeEntry(108, WarFileType.FileLevelPassable, 0, "Custom Dungeon 4 (Passable)"),
				new KnowledgeEntry(109, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 5 (Visual)"),
				new KnowledgeEntry(110, WarFileType.FileLevelPassable, 0, "Custom Dungeon 5 (Passable)"),
				new KnowledgeEntry(111, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 6 (Visual)"),
				new KnowledgeEntry(112, WarFileType.FileLevelPassable, 0, "Custom Dungeon 6 (Passable)"),
				new KnowledgeEntry(113, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 7 (Visual)"),
				new KnowledgeEntry(114, WarFileType.FileLevelPassable, 0, "Custom Dungeon 7 (Passable)"),
				new KnowledgeEntry(115, WarFileType.FileLevelVisual, (int)Tileset.Dungeon, "Custom Dungeon 8 (Visual)"),
				new KnowledgeEntry(116, WarFileType.FileLevelPassable, 0, "Custom Dungeon 8 (Passable)"),
				new KnowledgeEntry(117, WarFileType.FileLevelInfo, 0x00E7, "Humans 1"),
				new KnowledgeEntry(118, WarFileType.FileLevelInfo, 0x0111, "Orcs 1"),
/*
FileLevelInfo:
Offset				Data
------------------------------------------------
0x0000				Unbekannte Daten (Header)		(Länge 0x36)
0x003A				Unbekannte Daten				(Länge 0x20)
0x005C				Anfangswert: Lumber
0x0070				Anfangswert: Gold
0x0094				Offset des Missionstextes
0x					Offset der Missionsdaten (Gebäude, Einheiten, etc...)
	(z.B. bei 0x0111 in 118)

0x00CC				X-Position der "Kamera" (multipliziert mit 2)
0x00CE				Y-Position der "Kamera" (multipliziert mit 2)

Wege (5 bytes pro Weg)
2 bytes: X,Y vom Anfang (mul 2)
2 bytes: X,Y vom Ende (mul 2)
1 byte: ???? (Owner vielleicht?)

Einheiten/Gebäude (4 (bzw.6) bytes lang):
1 byte: X-Position (mul 2)
1 byte: Y-Position (mul 2)
1 byte: Typ
1 byte: Seite (0 = Player; 1 = Computer, 4 = Neutral)
Wenn Typ==32 (Goldmine)
1 byte: 0xFE
1 byte: Menge an Gold (multiply by 250)
*/
				new KnowledgeEntry(119, WarFileType.FileLevelInfo, 0x00E7, "Humans 2"),
				new KnowledgeEntry(120, WarFileType.FileLevelInfo, 0x0111, "Orcs 2"),
				new KnowledgeEntry(121, WarFileType.FileLevelInfo, 0x0183, "Humans 3"),
				new KnowledgeEntry(122, WarFileType.FileLevelInfo, 0x0111, "Orcs 3"),
				new KnowledgeEntry(123, WarFileType.FileLevelInfo, 0x00EA, "Humans 4"),
				new KnowledgeEntry(124, WarFileType.FileLevelInfo, 0x0111, "Orcs 4"),
				new KnowledgeEntry(125, WarFileType.FileLevelInfo, 0x0141, "Humans 5"),
				new KnowledgeEntry(126, WarFileType.FileLevelInfo, 0x0135, "Orcs 5"),
				new KnowledgeEntry(127, WarFileType.FileLevelInfo, 0x015F, "Humans 6"),
				new KnowledgeEntry(128, WarFileType.FileLevelInfo, 0x0141, "Orcs 6"),
				new KnowledgeEntry(129, WarFileType.FileLevelInfo, 0x0147, "Humans 7"),
				new KnowledgeEntry(130, WarFileType.FileLevelInfo, 0x0135, "Orcs 7"),
				new KnowledgeEntry(131, WarFileType.FileLevelInfo, 0x00E7, "Humans 8"),
				new KnowledgeEntry(132, WarFileType.FileLevelInfo, 0x0114, "Orcs 8"),
				new KnowledgeEntry(133, WarFileType.FileLevelInfo, 0x010B, "Humans 9"),
				new KnowledgeEntry(134, WarFileType.FileLevelInfo, 0x0177, "Orcs 9"),
				new KnowledgeEntry(135, WarFileType.FileLevelInfo, 0x011D, "Humans 10"),
				new KnowledgeEntry(136, WarFileType.FileLevelInfo, 0x00FF, "Orcs 10"),
				new KnowledgeEntry(137, WarFileType.FileLevelInfo, 0x014D, "Humans 11"),
				new KnowledgeEntry(138, WarFileType.FileLevelInfo, 0x0153, "Orcs 11"),
				new KnowledgeEntry(139, WarFileType.FileLevelInfo, 0x00FF, "Humans 12"),
				new KnowledgeEntry(140, WarFileType.FileLevelInfo, 0x0129, "Orcs 12"),
				new KnowledgeEntry(141, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(142, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(143, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(144, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(145, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(146, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(147, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(148, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(149, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(150, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(151, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(152, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(153, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(154, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(155, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(156, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(157, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(158, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(159, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(160, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(161, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(162, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(163, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(164, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(165, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(166, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(167, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(168, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(169, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(170, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(171, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(172, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(173, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(174, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(175, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(176, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(177, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(178, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(179, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(180, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(181, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(182, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(183, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(184, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(185, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(186, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(187, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(188, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(189, WarFileType.FileTileSet, 191, "Summer 1"),
				new KnowledgeEntry(190, WarFileType.FileTiles, 191, "Summer 2"),
				new KnowledgeEntry(191, WarFileType.FileTilePAL, 0, "Summer 3"),
				new KnowledgeEntry(192, WarFileType.FileTileSet, 194, "Barrens 1"),
				new KnowledgeEntry(193, WarFileType.FileTiles, 194, "Barrens 2"),
				new KnowledgeEntry(194, WarFileType.FileTilePAL, 0, "Barrens 3"),
				new KnowledgeEntry(195, WarFileType.FileTileSet, 197, "Dungeon 1"),
				new KnowledgeEntry(196, WarFileType.FileTiles, 197, "Dungeon 2"),
				new KnowledgeEntry(197, WarFileType.FileTilePAL, 0, "Dungeon 3"),
				new KnowledgeEntry(198, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(199, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(200, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(201, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(202, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(203, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(204, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(205, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(206, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(207, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(208, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(209, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(210, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(211, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(212, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(213, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(214, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(215, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(216, WarFileType.FileImage, 217, "Background 'Blizzard'"),
				new KnowledgeEntry(217, WarFileType.FilePalette, 0, "For Background"),
				new KnowledgeEntry(218, WarFileType.FileImage, 255, "Topbar (Humans)"),
				new KnowledgeEntry(219, WarFileType.FileImage, 217, "Topbar (Orcs)"),
				new KnowledgeEntry(220, WarFileType.FileImage, 255, "Sidebar Right (Humans)"),
				new KnowledgeEntry(221, WarFileType.FileImage, 217, "Sidebar Right (Orcs)"),
				new KnowledgeEntry(222, WarFileType.FileImage, 255, "Lower Bar (Humans)"),
				new KnowledgeEntry(223, WarFileType.FileImage, 217, "Lower Bar (Orcs)"),
				new KnowledgeEntry(224, WarFileType.FileImage, 255, "Sidebar Left Minimap Empty (Humans)"),
				new KnowledgeEntry(225, WarFileType.FileImage, 217, "Sidebar Left Minimap Empty (Orcs)"),
				new KnowledgeEntry(226, WarFileType.FileImage, 255, "Sidebar Left (Humans)"),
				new KnowledgeEntry(227, WarFileType.FileImage, 217, "Sidebar Left (Orcs)"),
				new KnowledgeEntry(228, WarFileType.FileImage, 255, "Sidebar Left Minimap Black (Humans)"),
				new KnowledgeEntry(229, WarFileType.FileImage, 217, "Sidebar Left Minimap Black (Orcs)"),
				new KnowledgeEntry(230, WarFileType.FileText, 0, "Warcraft CD"),
				new KnowledgeEntry(231, WarFileType.FileText, 0, "Purchase the Retail Version of Warcraft"),
				new KnowledgeEntry(232, WarFileType.FileText, 0, "Purchase the Retail Version of Warcraft"),
				new KnowledgeEntry(233, WarFileType.FileImage, 255, "Large Box (Humans)"),
				new KnowledgeEntry(234, WarFileType.FileImage, 217, "Large Box (Orcs)"),
				new KnowledgeEntry(235, WarFileType.FileImage, 255, "Small Box (Humans)"),
				new KnowledgeEntry(236, WarFileType.FileImage, 217, "Small Box (Orcs)"),
				new KnowledgeEntry(237, WarFileType.FileImage, 260, "Large Button"),
				new KnowledgeEntry(238, WarFileType.FileImage, 217, "Large Button (Clicked)"),
				new KnowledgeEntry(239, WarFileType.FileImage, 260, "Medium Button"),
				new KnowledgeEntry(240, WarFileType.FileImage, 217, "Medium Button (Clicked)"),
				new KnowledgeEntry(241, WarFileType.FileImage, 260, "Small Button"),
				new KnowledgeEntry(242, WarFileType.FileImage, 217, "Small Button (Clicked)"),
				new KnowledgeEntry(243, WarFileType.FileImage, 260, "Mainmenu Background Lower Part"),
				new KnowledgeEntry(244, WarFileType.FileImage, 217, "Button Arrow Left"),
				new KnowledgeEntry(245, WarFileType.FileImage, 217, "Button Arrow Left, Grey"),
				new KnowledgeEntry(246, WarFileType.FileImage, 217, "Button Arrow Right"),
				new KnowledgeEntry(247, WarFileType.FileImage, 217, "Button Arrow Right, Grey"),
				new KnowledgeEntry(248, WarFileType.FileImage, 217, "Window Border"),
				new KnowledgeEntry(249, WarFileType.FileImage, 217, "Saving/Loading Screen"),
				new KnowledgeEntry(250, WarFileType.FileImage, 217, "Saving/Loading Screen"),
				new KnowledgeEntry(251, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(252, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(253, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(254, WarFileType.FileImage, 255, "Hot keys"),
				new KnowledgeEntry(255, WarFileType.FilePalette, 0, "Humans"),
				new KnowledgeEntry(256, WarFileType.FileImage, 255, "Button 'ok'"),
				new KnowledgeEntry(257, WarFileType.FileImage, 255, "Button 'ok' 2"),
				new KnowledgeEntry(258, WarFileType.FileImage, 255, "Text 'WarCraft'"),
				new KnowledgeEntry(259, WarFileType.FileText, 0, "Main Menu Text"),
				new KnowledgeEntry(260, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(261, WarFileType.FileImage, 260, "Mainmenu Background"),
				new KnowledgeEntry(262, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(263, WarFileType.FileCursor, 262, "Normal Pointer"),
				new KnowledgeEntry(264, WarFileType.FileCursor, 262, "Not allowed"),
				new KnowledgeEntry(265, WarFileType.FileCursor, 262, "Crosshair Orange"),
				new KnowledgeEntry(266, WarFileType.FileCursor, 262, "Crosshair Red"),
				new KnowledgeEntry(267, WarFileType.FileCursor, 262, "Crosshair Orange 2"),
				new KnowledgeEntry(268, WarFileType.FileCursor, 262, "Magnifier"),
				new KnowledgeEntry(269, WarFileType.FileCursor, 262, "Crosshair Green"),
				new KnowledgeEntry(270, WarFileType.FileCursor, 262, "Loading..."),
				new KnowledgeEntry(271, WarFileType.FileCursor, 262, "Scroll Top"),
				new KnowledgeEntry(272, WarFileType.FileCursor, 262, "Scroll Topright"),
				new KnowledgeEntry(273, WarFileType.FileCursor, 262, "Scroll Right"),
				new KnowledgeEntry(274, WarFileType.FileCursor, 262, "Scroll Bottomright"),
				new KnowledgeEntry(275, WarFileType.FileCursor, 262, "Scroll Bottom"),
				new KnowledgeEntry(276, WarFileType.FileCursor, 262, "Scroll Bottomleft"),
				new KnowledgeEntry(277, WarFileType.FileCursor, 262, "Scroll Left"),
				new KnowledgeEntry(278, WarFileType.FileCursor, 262, "Scroll Topleft"),
				new KnowledgeEntry(279, WarFileType.FileSprite, 217, "Human Warrior"),
				new KnowledgeEntry(280, WarFileType.FileSprite, 217, "Orc Grunt"),
				new KnowledgeEntry(281, WarFileType.FileSprite, 217, "Human Peasant"),
				new KnowledgeEntry(282, WarFileType.FileSprite, 217, "Orc Peon"),
				new KnowledgeEntry(283, WarFileType.FileSprite, 217, "Human Catapult"),
				new KnowledgeEntry(284, WarFileType.FileSprite, 217, "Orc Catapult"),
				new KnowledgeEntry(285, WarFileType.FileSprite, 217, "Human Knight"),
				new KnowledgeEntry(286, WarFileType.FileSprite, 217, "Orc Rider"),
				new KnowledgeEntry(287, WarFileType.FileSprite, 217, "Human Bowman"),
				new KnowledgeEntry(288, WarFileType.FileSprite, 217, "Orc Axethrower"),
				new KnowledgeEntry(289, WarFileType.FileSprite, 217, "Human Wizard"),
				new KnowledgeEntry(290, WarFileType.FileSprite, 217, "Orc Wizard"),
				new KnowledgeEntry(291, WarFileType.FileSprite, 217, "Human Priest"),
				new KnowledgeEntry(292, WarFileType.FileSprite, 217, "Orc Necro"),
				new KnowledgeEntry(293, WarFileType.FileSprite, 217, "Medivh"),
				new KnowledgeEntry(294, WarFileType.FileSprite, 217, "Lothar"),
				new KnowledgeEntry(295, WarFileType.FileSprite, 217, "Wounded"),
				new KnowledgeEntry(296, WarFileType.FileSprite, 217, "Garana"),
				new KnowledgeEntry(297, WarFileType.FileSprite, 217, "Giant"),
				new KnowledgeEntry(298, WarFileType.FileSprite, 217, "Spider"),
				new KnowledgeEntry(299, WarFileType.FileSprite, 217, "Slime"),
				new KnowledgeEntry(300, WarFileType.FileSprite, 217, "Unit_300"),
				new KnowledgeEntry(301, WarFileType.FileSprite, 217, "Scorpion"),
				new KnowledgeEntry(302, WarFileType.FileSprite, 217, "Brigand"),
				new KnowledgeEntry(303, WarFileType.FileSprite, 217, "Skeleton"),
				new KnowledgeEntry(304, WarFileType.FileSprite, 217, "Skeleton 2"),
				new KnowledgeEntry(305, WarFileType.FileSprite, 217, "Demon"),
				new KnowledgeEntry(306, WarFileType.FileSprite, 217, "Water Elemental"),
				new KnowledgeEntry(307, WarFileType.FileSprite, 217, "Human Farm"),
				new KnowledgeEntry(308, WarFileType.FileSprite, 217, "Orc Farm"),
				new KnowledgeEntry(309, WarFileType.FileSprite, 217, "Human Barracks"),
				new KnowledgeEntry(310, WarFileType.FileSprite, 217, "Orc Barracks"),
				new KnowledgeEntry(311, WarFileType.FileSprite, 217, "Human Church"),
				new KnowledgeEntry(312, WarFileType.FileSprite, 217, "Orc Stone Circle"),
				new KnowledgeEntry(313, WarFileType.FileSprite, 217, "Human Tower"),
				new KnowledgeEntry(314, WarFileType.FileSprite, 217, "Orc Skull"),
				new KnowledgeEntry(315, WarFileType.FileSprite, 217, "Human Base"),
				new KnowledgeEntry(316, WarFileType.FileSprite, 217, "Orc Base"),
				new KnowledgeEntry(317, WarFileType.FileSprite, 217, "Human Smith"),
				new KnowledgeEntry(318, WarFileType.FileSprite, 217, "Orc Smith"),
				new KnowledgeEntry(319, WarFileType.FileSprite, 217, "Human Stables"),
				new KnowledgeEntry(320, WarFileType.FileSprite, 217, "Orc Stables"),
				new KnowledgeEntry(321, WarFileType.FileSprite, 217, "Human Smith"),
				new KnowledgeEntry(322, WarFileType.FileSprite, 217, "Orc Smith"),
				new KnowledgeEntry(323, WarFileType.FileSprite, 217, "Stormwind"),
				new KnowledgeEntry(324, WarFileType.FileSprite, 217, "Black Spire"),
				new KnowledgeEntry(325, WarFileType.FileSprite, 217, "Goldmine"),
				new KnowledgeEntry(326, WarFileType.FileSprite, 217, "Corpse"),
				new KnowledgeEntry(327, WarFileType.FileSprite, 217, "Human Peasant with Lumber"),
				new KnowledgeEntry(328, WarFileType.FileSprite, 217, "Orc Peon with Lumber"),
				new KnowledgeEntry(329, WarFileType.FileSprite, 217, "Human Peasant with Gold"),
				new KnowledgeEntry(330, WarFileType.FileSprite, 217, "Orc Peon with Gold"),
				new KnowledgeEntry(331, WarFileType.FileSprite, 217, "Humans Under construction"),
				new KnowledgeEntry(332, WarFileType.FileSprite, 217, "Orcs Under construction"),
				new KnowledgeEntry(333, WarFileType.FileSprite, 217, "Humans Under construction 2"),
				new KnowledgeEntry(334, WarFileType.FileSprite, 217, "Orcs Under construction 2"),
				new KnowledgeEntry(335, WarFileType.FileSprite, 217, "Humans Under construction 3"),
				new KnowledgeEntry(336, WarFileType.FileSprite, 217, "Orcs Under construction 3"),
				new KnowledgeEntry(337, WarFileType.FileSprite, 217, "Humans Under construction 4"),
				new KnowledgeEntry(338, WarFileType.FileSprite, 217, "Orcs Under construction 4"),
				new KnowledgeEntry(339, WarFileType.FileSprite, 217, "Humans Under construction 5"),
				new KnowledgeEntry(340, WarFileType.FileSprite, 217, "Orcs Under construction 5"),
				new KnowledgeEntry(341, WarFileType.FileSprite, 217, "Humans Under construction 6"),
				new KnowledgeEntry(342, WarFileType.FileSprite, 217, "Orcs Under construction 6"),
				new KnowledgeEntry(343, WarFileType.FileSprite, 217, "Humans Under construction 7"),
				new KnowledgeEntry(344, WarFileType.FileSprite, 217, "Orcs Under construction 7"),
				new KnowledgeEntry(345, WarFileType.FileSprite, 217, "Humans Under construction 8"),
				new KnowledgeEntry(346, WarFileType.FileSprite, 217, "Orcs Under construction 8"),
				new KnowledgeEntry(347, WarFileType.FileSprite, 217, "Building explosion"),
				new KnowledgeEntry(348, WarFileType.FileSprite, 217, "Fireball"),
				new KnowledgeEntry(349, WarFileType.FileSprite, 217, "Arrow"),
				new KnowledgeEntry(350, WarFileType.FileSprite, 217, "Unit_350"),
				new KnowledgeEntry(351, WarFileType.FileSprite, 217, "Unit_351"),
				new KnowledgeEntry(352, WarFileType.FileSprite, 217, "Tornado"),
				new KnowledgeEntry(353, WarFileType.FileSprite, 217, "Unit_353"),
				new KnowledgeEntry(354, WarFileType.FileSprite, 217, "Unit_354"),
				new KnowledgeEntry(355, WarFileType.FileSprite, 217, "Unit_355"),
				new KnowledgeEntry(356, WarFileType.FileSprite, 217, "Unit_356"),
				new KnowledgeEntry(357, WarFileType.FileSprite, 217, "Unit_357"),
				new KnowledgeEntry(358, WarFileType.FileSprite, 217, "Unit_358"),
				new KnowledgeEntry(359, WarFileType.FileSprite, 217, "UI Pictures Orcs"),
				new KnowledgeEntry(360, WarFileType.FileSprite, 217, "UI Pictures Humans"),
				new KnowledgeEntry(361, WarFileType.FileSprite, 217, "UI Pictures Icons"),
				new KnowledgeEntry(362, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(363, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(364, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(365, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(366, WarFileType.FileText, 0, "Remote system is not responding"),
				new KnowledgeEntry(367, WarFileType.FileText, 0, "Remote system is not responding (+Save)"),
				new KnowledgeEntry(368, WarFileType.FileText, 0, "Save Game; Load Game; Options; ..."),
				new KnowledgeEntry(369, WarFileType.FileText, 0, "Save Game; Load Game; Options; ..."),
				new KnowledgeEntry(370, WarFileType.FileText, 0, "Empty slots ... loading"),
				new KnowledgeEntry(371, WarFileType.FileText, 0, "Empty slots ... loading"),
				new KnowledgeEntry(372, WarFileType.FileText, 0, "Ok; Cancel"),
				new KnowledgeEntry(373, WarFileType.FileText, 0, "Ok; Cancel"),
				new KnowledgeEntry(374, WarFileType.FileText, 0, "Ok; Cancel"),
				new KnowledgeEntry(375, WarFileType.FileText, 0, "Ok; Cancel"),
				new KnowledgeEntry(376, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(377, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(378, WarFileType.FileText, 0, "Select Game Type"),
				new KnowledgeEntry(379, WarFileType.FileText, 0, "Choose Campaign"),
				new KnowledgeEntry(380, WarFileType.FileText, 0, "CD needed for Multiplayer"),
				new KnowledgeEntry(381, WarFileType.FileText, 0, "Customize your Game"),
				new KnowledgeEntry(382, WarFileType.FileText, 0, "Set Map"),
				new KnowledgeEntry(383, WarFileType.FileText, 0, "Set Opponent"),
				new KnowledgeEntry(384, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(385, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(386, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(387, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(388, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(389, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(390, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(391, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(392, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(393, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(394, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(395, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(396, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(397, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(398, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(399, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(400, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(401, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(402, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(403, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(404, WarFileType.FileText, 0, "Set Player Races"),
				new KnowledgeEntry(405, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(406, WarFileType.FileImage, 255, "Gold (Humans)"),
				new KnowledgeEntry(407, WarFileType.FileImage, 255, "Lumber (Humans)"),
				new KnowledgeEntry(408, WarFileType.FileImage, 217, "Gold (Orcs)"),
				new KnowledgeEntry(409, WarFileType.FileImage, 217, "Lumber (Orcs)"),
				new KnowledgeEntry(410, WarFileType.FileImage, 413, "%complete"),
				new KnowledgeEntry(411, WarFileType.FileImage, 413, "Human point summary"),
				new KnowledgeEntry(412, WarFileType.FileImage, 413, "Orc point summary"),
				new KnowledgeEntry(415, WarFileType.FileImage, 416, "Background Victory"),
				new KnowledgeEntry(416, WarFileType.FilePalette, 0, "for 415"),
				new KnowledgeEntry(417, WarFileType.FileImage, 418, "Background Defeat"),
				new KnowledgeEntry(418, WarFileType.FilePalette, 0, "for 417"),
				new KnowledgeEntry(419, WarFileType.FileImage, 423, "Victory Text"),
				new KnowledgeEntry(420, WarFileType.FileImage, 423, "Defeat Text"),
				new KnowledgeEntry(421, WarFileType.FileImage, 423, "Human Briefing"),
				new KnowledgeEntry(422, WarFileType.FileImage, 424, "Orc Briefing"),
				new KnowledgeEntry(423, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(424, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(425, WarFileType.FileSprite, 424, "Burning Fire Anim"),
				new KnowledgeEntry(426, WarFileType.FileSprite, 424, "Talking Orc Anim"),
				new KnowledgeEntry(427, WarFileType.FileSprite, 424, "Talking Orc 2 Anim"),
				new KnowledgeEntry(428, WarFileType.FileSprite, 423, "Eating Human Anim"),
				new KnowledgeEntry(429, WarFileType.FileSprite, 423, "Human Wizard Anim"),
				new KnowledgeEntry(430, WarFileType.FileSprite, 423, "Anim 1"),
				new KnowledgeEntry(431, WarFileType.FileSprite, 423, "Anim 2"),
				new KnowledgeEntry(432, WarFileType.FileBriefing, 1, "Orcs 1"),
				new KnowledgeEntry(433, WarFileType.FileBriefing, 2, "Orcs 2"),
				new KnowledgeEntry(434, WarFileType.FileBriefing, 3, "Orcs 3"),
				new KnowledgeEntry(435, WarFileType.FileBriefing, 4, "Orcs 4"),
				new KnowledgeEntry(436, WarFileType.FileBriefing, 5, "Orcs 5"),
				new KnowledgeEntry(437, WarFileType.FileBriefing, 6, "Orcs 6"),
				new KnowledgeEntry(438, WarFileType.FileBriefing, 7, "Orcs 7"),
				new KnowledgeEntry(439, WarFileType.FileBriefing, 8, "Orcs 8"),
				new KnowledgeEntry(440, WarFileType.FileBriefing, 9, "Orcs 9"),
				new KnowledgeEntry(441, WarFileType.FileBriefing, 10, "Orcs 10"),
				new KnowledgeEntry(442, WarFileType.FileBriefing, 11, "Orcs 11"),
				new KnowledgeEntry(443, WarFileType.FileBriefing, 12, "Orcs 12"),
				new KnowledgeEntry(444, WarFileType.FileBriefing, 1, "Humans 1"),
				new KnowledgeEntry(445, WarFileType.FileBriefing, 2, "Humans 2"),
				new KnowledgeEntry(446, WarFileType.FileBriefing, 3, "Humans 3"),
				new KnowledgeEntry(447, WarFileType.FileBriefing, 4, "Humans 4"),
				new KnowledgeEntry(448, WarFileType.FileBriefing, 5, "Humans 5"),
				new KnowledgeEntry(449, WarFileType.FileBriefing, 6, "Humans 6"),
				new KnowledgeEntry(450, WarFileType.FileBriefing, 7, "Humans 7"),
				new KnowledgeEntry(451, WarFileType.FileBriefing, 8, "Humans 8"),
				new KnowledgeEntry(452, WarFileType.FileBriefing, 9, "Humans 9"),
				new KnowledgeEntry(453, WarFileType.FileBriefing, 10, "Humans 10"),
				new KnowledgeEntry(454, WarFileType.FileBriefing, 11, "Humans 11"),
				new KnowledgeEntry(455, WarFileType.FileBriefing, 12, "Humans 12"),
				new KnowledgeEntry(456, WarFileType.FileImage, 457, "Human Win"),
				new KnowledgeEntry(457, WarFileType.FilePalette, 0, "for 456"),
				new KnowledgeEntry(458, WarFileType.FileImage, 459, "Orc Win"),
				new KnowledgeEntry(459, WarFileType.FilePalette, 0, "for 458"),
				new KnowledgeEntry(460, WarFileType.FileUnknown, 0, "Unknown"),
				new KnowledgeEntry(461, WarFileType.FileText, 0, "Endgametext: Humans 1"),
				new KnowledgeEntry(462, WarFileType.FileText, 0, "Endgametext: Orcs 1"),
				new KnowledgeEntry(463, WarFileType.FileText, 0, "Endgametext: Humans 2"),
				new KnowledgeEntry(464, WarFileType.FileText, 0, "Endgametext: Orcs 2"),
				new KnowledgeEntry(465, WarFileType.FileText, 0, "Credits"),
				new KnowledgeEntry(466, WarFileType.FileText, 0, "Level won (Humans?)"),
				new KnowledgeEntry(467, WarFileType.FileText, 0, "Level won (Orcs?)"),
				new KnowledgeEntry(468, WarFileType.FileText, 0, "Level lost (Humans?)"),
				new KnowledgeEntry(469, WarFileType.FileText, 0, "Level lost (Orcs?)"),
				new KnowledgeEntry(470, WarFileType.FileImage, 457, "Human Win/Animation 2"),
				new KnowledgeEntry(471, WarFileType.FileImage, 459, "Orc Win/Animation 2"),
				new KnowledgeEntry(472, WarFileType.FileWave, 0, "Unknown"),
				new KnowledgeEntry(473, WarFileType.FileWave, 0, "Unknown"),
				new KnowledgeEntry(474, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(475, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(476, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(477, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(478, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(479, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(480, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(481, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(482, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(483, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(484, WarFileType.FileVOC, 0, "Unknown"),
				new KnowledgeEntry(485, WarFileType.FileWave, 0, "Clicking noise")
			};

		static Dictionary<string, int> hashes;

		#region KnowledgeBase
		static KnowledgeBase()
		{
            hashes = new Dictionary<string, int>(KB_List.Length);

			for (int i = 0; i < KB_List.Length; i++)
			{
				if (hashes.ContainsKey(KB_List[i].text.ToLowerInvariant()))
					continue;

				hashes.Add(KB_List[i].text.ToLowerInvariant(), KB_List[i].id);
			}
		}
		#endregion

        #region Dump
        internal static void Dump()
        {
        }
        #endregion

        #region KEByName
        internal static KnowledgeEntry KEByName(string name)
		{
			name = name.ToLowerInvariant();

			object value = hashes[name];
			if (value == null)
				return new KnowledgeEntry(-1, WarFileType.FileUnknown, 0, "");
			return KB_List[(int)value];
		}
		#endregion

		#region IndexByName
		internal static int IndexByName(string name)
		{
			name = name.ToLowerInvariant();

            if (hashes.ContainsKey(name))
            {
                int value = hashes[name];
                return value;
            }

            return -1;
		}
		#endregion
	}
}
