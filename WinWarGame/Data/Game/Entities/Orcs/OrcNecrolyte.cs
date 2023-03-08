using System;

namespace WinWarGame.Data.Game
{
   internal class OrcNecrolyte : Unit
   {
      public OrcNecrolyte (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Necro")));
      }
   }
}

