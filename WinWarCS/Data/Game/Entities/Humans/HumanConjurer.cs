﻿using System;

namespace WinWarCS.Data.Game
{
   internal class HumanConjurer : Unit
   {
      public HumanConjurer (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Wizard")));
      }
   }
}

