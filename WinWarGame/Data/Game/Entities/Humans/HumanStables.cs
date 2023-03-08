using System;

namespace WinWarGame.Data.Game
{
   internal class HumanStables : Building
   {
      public HumanStables (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Stables")));
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

