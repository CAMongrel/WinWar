using System;

namespace WinWarGame.Data.Game
{
   internal class HumanChurch : Building
   {
      public HumanChurch (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Church")));
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

