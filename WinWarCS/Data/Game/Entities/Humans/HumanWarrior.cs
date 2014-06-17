using System;

namespace WinWarCS.Data.Game
{
   internal class HumanWarrior : Unit
   {
      public HumanWarrior (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Warrior")));
      }
   }
}

