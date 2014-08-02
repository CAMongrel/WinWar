using System;

namespace WinWarCS.Data.Game
{
   internal class OrcAxethrower : Unit
   {
      public OrcAxethrower (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Axethrower")));
      }
   }
}

