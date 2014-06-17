using System;

namespace WinWarCS.Data.Game
{
   internal class OrcGrunt : Unit
   {
      public OrcGrunt (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Grunt")));
      }
   }
}

