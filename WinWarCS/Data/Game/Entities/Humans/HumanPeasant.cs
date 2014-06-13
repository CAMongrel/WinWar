using System;

namespace WinWarCS.Data.Game
{
   internal class HumanPeasant : Unit
   {
      public HumanPeasant ()
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Peasant")));
      }
   }
}

