﻿using System;

namespace WinWarCS.Data.Game
{
   internal class OrcNecrolyte : Unit
   {
      public OrcNecrolyte (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Necro")));
      }
   }
}

