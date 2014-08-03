// Author: Henning
// Project: WinWarEngine
// Path: D:\Projekte\Henning\C#\WinWarCS\WinWarEngine\Data\Resources
// Creation date: 27.11.2009 20:22
// Last modified: 29.11.2009 13:31
using WinWarCS.Data.Game;

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace WinWarCS.Data.Resources
{
   internal class LevelVisualResource : BasicResource
   {
      internal ushort[] visualData;

      Tileset tileset;

      internal Tileset Tileset
      {
         get
         {
            return tileset;
         }
      }

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelVisualResource(WarResource setData, Tileset setTileset)
      {
         Init(setData, setTileset);
      }
      // LevelVisualResource(setData, setOffset)

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelVisualResource(string name)
      {
         KnowledgeEntry ke = WarFile.KnowledgeBase[name];

         WarResource res = WarFile.GetResource(ke.id);
         if (res == null)
            throw new ArgumentNullException("res");

         Init(res, (Tileset)ke.param);
      }
      // LevelVisualResource(setData, setOffset)
		
      /// <summary>
      /// Init
      /// </summary>
      private void Init(WarResource setData, Tileset setTileset)
      {
         this.tileset = setTileset;
         this.data = setData;

         CreateVisualData();
      }
      // Init(data, offset)
		
      /// <summary>
      /// Create visual data
      /// </summary>
      void CreateVisualData()
      {
         // Levels are always 64 x 64 in size
         visualData = new ushort[64 * 64];

         if (data.data != null)
         {
            unsafe
            {
               fixed (byte* org_ptr = &data.data[0])
               {
                  ushort* ptr = (ushort*)org_ptr;
						
                  for (int y = 0; y < 64; y++)
                  {
                     for (int x = 0; x < 64; x++)
                     {
                        visualData[x + y * 64] = *ptr;
                        ptr++;
                     }
                  }
               } // fixed
            } // fixed
         } // if
      }
      // CreateVisualData()
		
      /// <summary>
      /// Destroy visual data
      /// </summary>
      internal void DestroyVisualData()
      {
      }
      // DestroyVisualData()
   }
}
