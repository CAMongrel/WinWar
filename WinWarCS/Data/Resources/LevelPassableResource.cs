using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data.Game;

namespace WinWarCS.Data.Resources
{
   internal class LevelPassableResource : BasicResource
   {
      internal short width;
      internal short height;
      internal short[,] passableData;

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelPassableResource (WarResource setData)
      {
         Type = ContentFileType.FileLevelPassable;

         this.Resource = setData;

         width = 64;
         height = 64;
			
         CreatePassableData ();
      }
      // LevelVisualResource(setData, setOffset)

      internal void FillAStar(AStar2D astar)
      {
         astar.SetField (passableData, width, height);
      }
		
      /// <summary>
      /// Create visual data
      /// </summary>
      private void CreatePassableData ()
      {
         passableData = new short[width, height];
			
         if (Resource.data != null) 
         {
            unsafe 
            {
               fixed (byte* org_ptr = &Resource.data[0]) 
               {
                  short* ptr = (short*)org_ptr;
						
                  for (int y = 0; y < height; y++) 
                  {
                     for (int x = 0; x < width; x++) 
                     {
                        passableData [x, y] = *ptr;
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
      internal void DestroyPassableData ()
      {
      }
      // DestroyVisualData()
   }
}
