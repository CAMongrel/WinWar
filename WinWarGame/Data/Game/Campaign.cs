using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarGame.Data.Game
{
   class Campaign
   {
      public int Level { get; private set; }

      public int MaxLevels { get; private set; }

      internal Race Race { get; private set; }

      internal Campaign(Race setRace)
      {
         Race = setRace;

         MaxLevels = 12;
         if (WarFile.IsDemo)
            MaxLevels = 3;

         StartNew();
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

      public string GetCurrentLevelName()
      {
         return Race + " " + Level;
      }
   }
}
