using System;

namespace WinWarCS.Data.Game
{
   internal class HumanTower : Building
   {
      public HumanTower (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Tower")));
      }

      public override int TileSizeX 
      {
         get 
         {
            return 2;
         }
      }
      public override int TileSizeY
      {
         get 
         {
            return 2;
         }
      }
   }
}

