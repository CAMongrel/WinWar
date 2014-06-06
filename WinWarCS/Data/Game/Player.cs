using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Game
{
   class Player
   {
      internal string Name { get; set; }

      internal Race Race { get; set; }

      internal Campaign Campaign { get; private set; }

      internal Player ()
      {
         Name = "Player";
         Race = Game.Race.Humans;
         Campaign = null;
      }

      internal void StartNewCampaign()
      {
         Campaign = new Campaign (this);
         Campaign.StartNew ();
      }
   }
}
