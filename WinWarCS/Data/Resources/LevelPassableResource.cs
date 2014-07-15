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

      internal LevelPassableResource (string res_name)
      {
         // Levels are always 64 x 64 in size
         width = 64;
         height = 64;

         this.data = WarFile.GetResourceByName (res_name);

         CreatePassableData ();
      }

      /// <summary>
      /// Create level visual resource
      /// </summary>
      internal LevelPassableResource (WarResource setData)
      {
         this.data = setData;
			
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
			
         if (data.data != null) 
         {
            unsafe 
            {
               fixed (byte* org_ptr = &data.data[0]) 
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
