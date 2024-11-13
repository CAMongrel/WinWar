using System;

namespace WinWarGame.Data.Game
{
   internal class OrcGrunt : Unit
   {
      public OrcGrunt (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Grunt")));
      }
   }
}

