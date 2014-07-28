using System;

namespace WinWarCS.Data.Game
{
   internal class HumanBarracks : Building
   {
      public HumanBarracks (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Barracks")));
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

