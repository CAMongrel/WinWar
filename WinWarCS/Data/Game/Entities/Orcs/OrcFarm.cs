using System;

namespace WinWarCS.Data.Game
{
   internal class OrcFarm : Building
   {
      public OrcFarm (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Farm")));
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

