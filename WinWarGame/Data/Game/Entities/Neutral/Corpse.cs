using System;

namespace WinWarGame.Data.Game
{
   internal class Corpse : Entity
   {
      public Corpse (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Corpse")));
      }

      public override bool CanBeSelected 
      {
         get 
         {
            return false;
         }
      }
   }
}

