using System;

namespace WinWarCS.Data.Game
{
   internal class OrcRider : Unit
   {
      public OrcRider (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Rider")));
      }
   }
}

