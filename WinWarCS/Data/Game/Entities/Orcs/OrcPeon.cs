using System;

namespace WinWarCS.Data.Game
{
   internal class OrcPeon : Unit
   {
      public OrcPeon (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Peon")));
      }
   }
}

