﻿using System;

namespace WinWarGame.Data.Game
{
   internal class HumanBarracks : Building
   {
      public HumanBarracks (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Barracks")));
      }

      public override int TileSizeX 
      {
         get 
         {
            return 3;
         }
      }
      public override int TileSizeY
      {
         get 
         {
            return 3;
         }
      }
   }
}

