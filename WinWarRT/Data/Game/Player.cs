using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Data.Game
{
    class Player
    {
        public string Name { get; set; }
        public Race Race { get; set; }

        public Player()
        {
            Name = "Player";
            Race = Game.Race.Humans;
        }
    }
}
