﻿using System;

namespace WinWarGame.Data.Game
{
   internal class HumanConjurer : Unit
   {
      public HumanConjurer (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Wizard")));
      }
   }
}

