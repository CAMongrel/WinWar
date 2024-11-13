using System;

namespace WinWarGame.Data.Game
{
   internal class HumanCatapult : Unit
   {
      public HumanCatapult (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Catapult")));
      }
   }
}

