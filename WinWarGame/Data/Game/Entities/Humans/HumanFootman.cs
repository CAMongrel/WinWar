using System;

namespace WinWarGame.Data.Game
{
   internal class HumanFootman : Unit
   {
      public HumanFootman (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Warrior")));
      }
   }
}

