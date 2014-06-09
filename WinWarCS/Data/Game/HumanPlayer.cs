using System;

namespace WinWarCS.Data.Game
{
   internal class HumanPlayer : BasePlayer
   {
      internal Campaign Campaign { get; private set; }

      public HumanPlayer ()
         : base (PlayerType.Human)
      {

      }

      internal void StartNewCampaign()
      {
         Campaign = new Campaign (this);
         Campaign.StartNew ();
      }
   }
}

