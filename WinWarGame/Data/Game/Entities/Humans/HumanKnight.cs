using System;

namespace WinWarCS.Data.Game
{
   internal class HumanKnight : Unit
   {
      public HumanKnight (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Knight")));
      }
   }
}

