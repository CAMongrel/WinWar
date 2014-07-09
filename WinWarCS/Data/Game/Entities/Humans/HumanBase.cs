using System;

namespace WinWarCS.Data.Game
{
   internal class HumanBase : Building
   {
      public HumanBase (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Base")));
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

