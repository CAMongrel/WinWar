using System;

namespace WinWarGame.Data.Game
{
   internal class HumanFarm : Building
   {
      public HumanFarm (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Farm")));
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

