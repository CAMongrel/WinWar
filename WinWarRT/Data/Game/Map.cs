// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Data\Game
// Creation date: 27.11.2009 20:22
// Last modified: 27.11.2009 23:04

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Data;
using WinWarRT.Data.Resources;
#endregion

namespace WinWarRT.Data.Game
{
	class Map
	{
		LevelInfoResource levelInfo;
		LevelVisualResource levelVisual;
		LevelPassableResource levelPassable;
		
		MapTileset tileSet;
		
		#region ctor
		/// <summary>
		/// Create map
		/// </summary>
		public Map(LevelInfoResource setLevelInfo, 
			LevelVisualResource setLevelVisual,
			LevelPassableResource setLevelPassable)
		{
			levelInfo = setLevelInfo;
			levelVisual = setLevelVisual;
			levelPassable = setLevelPassable;
			
			tileSet = MapTileset.GetTileset(levelVisual.Tileset);
		} // Map(setLevelInfo, setLevelVisual, setLevelPassable)
		#endregion
		
		#region Render
		/// <summary>
		/// Render
		/// </summary>
		public void Render()
		{
			for (int y = 0; y < 64; y++)
			{
				for (int x = 0; x < 64; x++)
				{
					tileSet.DrawTile(
						levelVisual.visualData[x + y * 64], x * 16, y * 16, 2.0f);
				}
			}
		} // Render()
		#endregion
		
		#region Unit-testing
		/// <summary>
		/// Test map
		/// </summary>
		public static void TestMap()
		{
            throw new NotImplementedException();
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
		} // TestMap()
		#endregion
	}
}
