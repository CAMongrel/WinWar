﻿using System;

namespace WinWarGame.Data.Game
{
   internal class HumanCleric : Unit
   {
      public HumanCleric (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Priest")));
      }
   }
}

