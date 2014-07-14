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

      internal List<Node> Path;

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

         attackTimer = Owner.AttackSpeed;
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
            ((BuildEntity)this.Owner).Idle();
            return;
         }

         MoveOrAttack(entry.Target);
      }

      private void MoveOrAttack(Entity ent)
      {
         float offx = this.Owner.X - ent.X;
         float offy = this.Owner.Y - ent.Y;

         float sqr_dist = (offx * offx + offy * offy);

         float sqr_meleerange = ent.AttackRange * ent.AttackRange;

         if (sqr_dist < sqr_meleerange)
         {
            // Target is in range -> Perform an attack
            this.Owner.PerformAttack(ent);
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

