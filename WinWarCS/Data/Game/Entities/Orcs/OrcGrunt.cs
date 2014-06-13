using System;

namespace WinWarCS.Data.Game
{
   internal class OrcGrunt : Unit
   {
      public OrcGrunt ()
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Grunt")));
      }
   }
}

