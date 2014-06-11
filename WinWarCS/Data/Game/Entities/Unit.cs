using System;

namespace WinWarCS.Data.Game
{
   internal  class Unit : Entity
   {
      public UnitSprite Sprite
      {
         get
         {
            return base.sprite as UnitSprite;
         }
      }

      public Unit ()
      {
      }
   }
}

