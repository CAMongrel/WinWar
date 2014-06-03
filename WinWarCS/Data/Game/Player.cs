using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Data.Game
{
    class Player
    {
        internal string Name { get; set; }
        internal Race Race { get; set; }

        internal Player()
        {
            Name = "Player";
            Race = Game.Race.Humans;
        }
    }
}
