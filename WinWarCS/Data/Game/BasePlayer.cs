using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Game
{
   enum PlayerType
   {
      Human,
      AI
   }

   internal class BasePlayer
   {
      internal string Name { get; set; }

      internal Race Race { get; set; }

      internal PlayerType PlayerType { get; private set; }

      internal BasePlayer (PlayerType setPlayerType)
      {
         Name = "Player";
         Race = Game.Race.Humans;
         PlayerType = setPlayerType;
      }
   }
}
