﻿using System;

namespace WinWarGame.Data.Game
{
   internal class OrcSpearman : Unit
   {
      public OrcSpearman (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Axethrower")));
      }
   }
}

