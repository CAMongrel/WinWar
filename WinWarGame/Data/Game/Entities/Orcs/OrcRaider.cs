using System;

namespace WinWarGame.Data.Game
{
   internal class OrcRaider : Unit
   {
      public OrcRaider (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Rider")));
      }
   }
}

