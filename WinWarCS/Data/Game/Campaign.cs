using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Game
{
   class Campaign
   {
      public int Level { get; private set; }
      public int MaxLevels { get; private set; }

      private BasePlayer curPlayer;

      internal Campaign (BasePlayer refPlayer)
      {
         MaxLevels = 12;
         if (WarFile.IsDemo)
            MaxLevels = 3;

         curPlayer = refPlayer;

         StartNew ();
      }

      internal void StartNew()
      {
         Level = 1;
      }

      internal void AdvanceNextLevel()
      {
         Level++;

         if (Level > MaxLevels) 
         {
            // TODO: End campaign
         }
      }

      public string GetCurrentLevelName ()
      {
         return curPlayer.Race + " " + Level;
      }
   }
}
