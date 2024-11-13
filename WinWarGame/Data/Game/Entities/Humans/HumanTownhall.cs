using System;

namespace WinWarGame.Data.Game
{
   internal class HumanTownhall : Building
   {
      public HumanTownhall (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Base")));
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

