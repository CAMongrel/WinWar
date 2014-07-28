using System;

namespace WinWarCS.Data.Game
{
   internal class HumanStormwind : Building
   {
      public HumanStormwind (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Stormwind")));
      }

      public override int TileSizeX 
      {
         get 
         {
            return 4;
         }
      }
      public override int TileSizeY
      {
         get 
         {
            return 4;
         }
      }
   }
}

