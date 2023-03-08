using System;

namespace WinWarGame.Data.Game
{
   internal class HumanArcher : Unit
   {
      public HumanArcher (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Bowman")));
      }
   }
}

