using System;

namespace WinWarCS.Data.Game
{
   internal class HumanPeasant : Unit
   {
      public HumanPeasant (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Peasant")));
      }
   }
}

