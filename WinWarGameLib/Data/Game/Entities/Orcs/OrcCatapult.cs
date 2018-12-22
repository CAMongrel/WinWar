using System;

namespace WinWarCS.Data.Game
{
   internal class OrcCatapult : Unit
   {
      public OrcCatapult (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Catapult")));
      }
   }
}

