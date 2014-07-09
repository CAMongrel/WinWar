using System;

namespace WinWarCS.Data.Game
{
   internal class OrcBase : Building
   {
      public OrcBase (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Base")));
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

