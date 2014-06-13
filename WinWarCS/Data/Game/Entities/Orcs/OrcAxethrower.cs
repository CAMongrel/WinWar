using System;

namespace WinWarCS.Data.Game
{
   internal class OrcAxethrower : Unit
   {
      public OrcAxethrower ()
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Axethrower")));
      }
   }
}

