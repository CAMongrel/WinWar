using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data;

namespace WinWarCS.Data.Resources
{
   public class LevelPassableResource : BasicResource
   {
      internal short width;
      internal short height;
      internal short[,] passableData;

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelPassableResource(WarResource setData)
      {
         Type = ContentFileType.FileLevelPassable;

         width = 64;
         height = 64;

         CreatePassableData(setData);
      }
      // LevelVisualResource(setData, setOffset)

      /// <summary>
      /// Create visual data
      /// </summary>
      private void CreatePassableData(WarResource setData)
      {
         passableData = new short[width, height];

         if (setData.data != null)
         {
            unsafe
            {
               fixed (byte* org_ptr = &setData.data[0])
               {
                  short* ptr = (short*)org_ptr;

                  for (int y = 0; y < height; y++)
                  {
                     for (int x = 0; x < width; x++)
                     {
                        passableData[x, y] = *ptr;
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
      internal void DestroyPassableData()
      {
      }
      // DestroyVisualData()
   }
}
