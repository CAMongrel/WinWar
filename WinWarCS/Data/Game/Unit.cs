using System;
using WinWarCS.Data.Game;

namespace WinWarCS.MacOS
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

