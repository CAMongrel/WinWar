using System;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class Goldmine : Building
   {
      public Goldmine (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Goldmine")));
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

