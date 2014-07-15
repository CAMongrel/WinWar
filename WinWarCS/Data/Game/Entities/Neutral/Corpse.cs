using System;

namespace WinWarCS.Data.Game
{
   internal class Corpse : Entity
   {
      public Corpse (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Corpse")));
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

