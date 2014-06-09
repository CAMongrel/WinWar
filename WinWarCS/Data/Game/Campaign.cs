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

      private BasePlayer curPlayer;

      internal Campaign (BasePlayer refPlayer)
      {
         curPlayer = refPlayer;
         Level = 1;
      }

      internal void StartNew()
      {
         Level = 1;
      }

      internal void AdvanceNextLevel()
      {
         Level++;
      }

      public string GetCurrentLevelName ()
      {
         return curPlayer.Race + " " + Level;
      }
   }
}
