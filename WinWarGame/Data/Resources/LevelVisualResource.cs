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

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelVisualResource(WarResource setData)
      {
         Type = ContentFileType.FileLevelVisual;

         CreateVisualData(setData);
      }
      // LevelVisualResource(setData, setOffset)

      /// <summary>
      /// Create visual data
      /// </summary>
      void CreateVisualData(WarResource setData)
      {
         // Levels are always 64 x 64 in size
         visualData = new ushort[64 * 64];

         if (setData.data != null)
         {
            unsafe
            {
               fixed (byte* org_ptr = &setData.data[0])
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
