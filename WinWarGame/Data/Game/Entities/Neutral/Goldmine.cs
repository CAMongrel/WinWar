using System;
using WinWarGame.Data.Resources;

namespace WinWarGame.Data.Game
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

