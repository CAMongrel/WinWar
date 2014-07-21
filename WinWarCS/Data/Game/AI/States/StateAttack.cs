using System;
using System.Collections.Generic;
using WinWarCS.Util;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
   class StateAttack : State
   {
      internal int targetX;
      internal int targetY;

      internal double attackTimer;

      internal int curNodeIdx;

      internal MapPath Path;

      internal StateAttack(Entity Owner, Entity Target)
         : base(Owner)
      {
         if (Target == null)
            throw new ArgumentNullException("Target");

         Owner.CurrentTarget = Target;
         Path = null;
      }

      internal override void Enter()
      {
         if (Owner is Unit) 
         {
            Unit unit = (Unit)Owner;
            unit.Sprite.SetCurrentAnimationByName ("Attack");
         }

         Owner.HateList.SetHateValue(Owner.CurrentTarget, 25, HateListParam.PushToTop);

         Log.AI(this.Owner, "Attacking " + Owner.CurrentTarget.Name + Owner.CurrentTarget.UniqueID);

         targetX = Owner.CurrentTarget.TileX;
         targetY = Owner.CurrentTarget.TileY;

         // TODO: This may lead to bugs, if the user manages to quickly switch states.
         // This must be fixed by moving the attackTimer to the unit itself.
         attackTimer = 0;
      }

      internal override void Update(GameTime gameTime)
      {
         attackTimer -= gameTime.ElapsedGameTime.TotalSeconds;
         if (attackTimer > 0)
            return;

         attackTimer = Owner.AttackSpeed;

         this.Owner.UpdateHateList();

         HateListEntry entry = this.Owner.HateList.GetHighestHateListEntry();
         if (entry.Target == null)
         {
            this.Owner.Idle();
            return;
         }

         MoveOrAttack(entry.Target);
      }

      private void MoveOrAttack(Entity ent)
      {
         float offx = ent.X - this.Owner.X;
         float offy = ent.Y - this.Owner.Y;

         float sqr_dist = (offx * offx + offy * offy);

         float sqr_meleerange = ent.AttackRange * ent.AttackRange;

         if (sqr_dist < sqr_meleerange)
         {
            // Target is in range -> Perform an attack
            if (this.Owner.PerformAttack (ent)) 
            {
               if (this.Owner is Unit) 
               {
                  Unit unit = (Unit)this.Owner;
                  unit.Orientation = Unit.OrientationFromDiff (offx, offy);
                  unit.Sprite.CurrentAnimation.Reset ();
               }
            }
         }
         else
         {
            // Target is out of range -> Move towards it

            // If no path has been calculated yet or if the target has moved, calculate new path
            if (Path == null || ent.TileX != targetX || ent.TileY != targetY)
            {
               targetX = ent.TileX;
               targetY = ent.TileY;
               Path = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, targetX, targetY);
               if (Path == null)
                  return;
               curNodeIdx = 0;
            }

            if (curNodeIdx < Path.Count)
            {
               Node node = Path[curNodeIdx++];
               // TODO!!! Move, not Set
               Owner.SetPosition (node.X, node.Y);
            }
            else
               Log.Error("Error calculating path");
         }
      }
   }
}

