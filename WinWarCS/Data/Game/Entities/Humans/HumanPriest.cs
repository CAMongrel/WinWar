using System;

namespace WinWarCS.Data.Game
{
   internal class HumanPriest : Unit
   {
      public HumanPriest (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Priest")));
      }
   }
}

