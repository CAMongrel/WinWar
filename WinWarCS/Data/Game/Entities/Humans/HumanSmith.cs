using System;

namespace WinWarCS.Data.Game
{
   internal class HumanSmith : Building
   {
      public HumanSmith (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Smith")));
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

