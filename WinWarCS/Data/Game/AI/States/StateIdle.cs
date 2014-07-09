using System;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
   class StateIdle : State
   {
      internal double updateTimer;

      internal StateIdle(Entity Owner)
         : base(Owner)
      {
      }

      internal override void Enter()
      {
         updateTimer = 1.0f;
         this.Owner.HateList.Wipe();
      }

      internal override void Update(GameTime gameTime)
      {
         // Only perform updates once per second
         updateTimer -= gameTime.ElapsedGameTime.TotalSeconds;
         if (updateTimer > 0)
            return;

         updateTimer = 1.0f;

         if (Owner.LookaroundWhileIdle) 
         {
            bool shouldSwitch = Owner.CurrentMap.Rnd.NextDouble () < 0.02;
            if (shouldSwitch) 
            {
               Unit unit = Owner as Unit;
               if (unit != null)
                  unit.SetRandomOrientation ();
            }
         }

         float sqr_aggrorange = Owner.AttackRange;

         foreach (BasePlayer pl in Owner.CurrentMap.Players)
         {
            if (pl == this.Owner.Owner || pl.IsNeutralTowards(Owner))
               continue;

            foreach (Entity ent in pl.Entities)
            {
               float offx = this.Owner.X - ent.X;
               float offy = this.Owner.Y - ent.Y;

               float dist = (offx * offx + offy * offy);

               if (dist < sqr_aggrorange)
               {
                  this.Owner.Attack(ent);
               }
            }
         }
      }
   }
}

