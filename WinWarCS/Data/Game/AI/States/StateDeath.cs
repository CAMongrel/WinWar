using System;

namespace WinWarCS.Data.Game
{
   class StateDeath : State
   {
      internal StateDeath(Entity Owner)
         : base(Owner)
      {
      }

      internal override void Enter()
      {
         if (Owner is Unit) 
         {
            Unit unit = (Unit)Owner;
            unit.Sprite.SetCurrentAnimationByName ("Death");
         }
      }
   }
}

