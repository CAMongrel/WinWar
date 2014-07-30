using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WinWarCS.Util;

namespace WinWarCS.Data.Game
{
   class StateAttackMove : State
   {
      internal int destX;
      internal int destY;
      internal int targetX;
      internal int targetY;

      internal bool bUpdatePath;

      internal int curNodeIdx;

      internal double moveTimer;

      internal MapPath Path;
      internal MapPath TargetPath;

      internal StateAttackMove(Entity Owner, int X, int Y)
         : base(Owner)
      {
         Path = null;
         TargetPath = null;
         destX = X;
         destY = Y;
         bUpdatePath = true;
      }

      internal override void Enter()
      {
         moveTimer = 1.0f;
         curNodeIdx = -1;
         Path = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, destX, destY);
         if (Path != null)
         {
            curNodeIdx = 0;
            bUpdatePath = false;
         }
      }

      internal override void Update(GameTime gameTime)
      {
         moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
         if (moveTimer > 0)
            return;

         moveTimer = 1.0f;

         this.Owner.UpdateHateList();

         HateListEntry entry = this.Owner.HateList.GetHighestHateListEntry();
         if (entry.Target == null)
         {
            Log.AI(this.Owner, "No enemy ... moving!");

            if (bUpdatePath)
            {
               Path = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, destX, destY);
               if (Path != null)
               {
                  curNodeIdx = 0;
                  bUpdatePath = false;
               }
            }

            if (curNodeIdx == -1 || Path == null)
               return;

            if (curNodeIdx >= Path.Count)
            {
               this.Owner.Idle();
               return;
            }

            IMapPathNode node = Path[curNodeIdx++];
            // TODO!!! Move, not Set
            Owner.SetPosition (node.X, node.Y);
            return;
         }

         this.Owner.CurrentTarget = entry.Target;

         Log.AI(this.Owner, "Enemy spotted! Attacking " + entry.Target.Name + entry.Target.UniqueID);

         bUpdatePath = true;
         Path = null;

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
            if (this.Owner.CanAttack && ent.ShouldBeAttacked)
            {
               // Target is in range -> Perform an attack
               this.Owner.PerformAttack(ent);
            }
         }
         else
         {
            // Target is out of range -> Move towards it

            Log.AI(this.Owner, "Target is out of range ... moving towards it!");

            // If no path has been calculated yet or if the target has moved, calculate new path
            if (TargetPath == null || ent.X != targetX || ent.Y != targetY)
            {
               targetX = ent.TileX;
               targetY = ent.TileY;
               TargetPath = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, targetX, targetY);
               if (TargetPath == null)
                  return;
               curNodeIdx = 0;
            }

            IMapPathNode node = TargetPath[curNodeIdx++];
            // TODO!!! Move, not Set
            Owner.SetPosition (node.X, node.Y);
         }
      }
   }
}

