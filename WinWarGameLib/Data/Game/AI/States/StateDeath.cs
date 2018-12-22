using System;

namespace WinWarCS.Data.Game
{
   class StateDeath : State
   {
      internal StateDeath(Entity Owner)
         : base(Owner)
      {
      }

      internal override bool Enter()
      {
         if (Owner is Unit) 
         {
            Unit unit = (Unit)Owner;
            unit.Sprite.SetCurrentAnimationByName ("Death1");
         }

         return true;
      }

      internal override void Update (Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update (gameTime);

         if (Owner is Unit) 
         {
            Unit unit = (Unit)Owner;
            if (unit.Sprite.CurrentAnimation.Phase == SpriteAnimationPhase.Finished) 
            {
               Owner.DestroyAndSpawnRemains ();
            }
         }
      }
   }
}

