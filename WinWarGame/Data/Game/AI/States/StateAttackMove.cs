using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
{
    class StateAttackMove : State
    {
        private readonly int destX;
        private readonly int destY;
        private int targetX;
        private int targetY;

        private bool bUpdatePath;

        private int curNodeIdx;

        private double moveTimer;

        private MapPath path;
        private MapPath targetPath;
        
        private bool isMoving;
        private bool leaveRequested;

        internal StateAttackMove(Entity owner, int x, int y)
            : base(owner)
        {
            path = null;
            targetPath = null;
            destX = x;
            destY = y;
            bUpdatePath = true;
            
            isMoving = false;
            leaveRequested = false;
        }

        internal override bool Enter()
        {
            moveTimer = 1.0f;
            curNodeIdx = -1;
            
            var owner = Owner;
            if (owner == null)
            {
                return false;
            }
            
            path = owner.CurrentMap.CalcPath(owner.TileX, owner.TileY, destX, destY);
            if (path == null)
            {
                return false;
            }

            curNodeIdx = 0;
            bUpdatePath = false;

            return true;
        }
        
        internal override bool Leave()
        {
            if (isMoving == false)
            {
                return true;
            }

            leaveRequested = true;
            return false;
        }

        internal override void Update(GameTime gameTime)
        {
            moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (moveTimer > 0)
            {
                return;
            }

            var owner = Owner;
            if (owner == null)
            {
                return;
            }
            
            moveTimer = 1.0f;

            owner.UpdateHateList();

            HateListEntry entry = this.Owner.HateList.GetHighestHateListEntry();
            if (entry.Target == null)
            {
                Log.AI(owner.ToString(), "No enemy ... moving!");

                if (bUpdatePath)
                {
                    path = owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, destX, destY);
                    if (path != null)
                    {
                        curNodeIdx = 0;
                        bUpdatePath = false;
                    }
                }

                if (curNodeIdx == -1 || path == null)
                {
                    leaveRequested = false;
                    isMoving = false;
                    return;
                }

                if (curNodeIdx >= path.Count)
                {
                    leaveRequested = false;
                    isMoving = false;
                    owner.Idle();
                    return;
                }
                
                if (leaveRequested)
                {
                    isMoving = false;
                    return;
                }

                IMapPathNode node = path[curNodeIdx++];
                // TODO!!! Move, not Set
                owner.SetPosition(node.X, node.Y);
                return;
            }

            owner.CurrentTarget = entry.Target;

            Log.AI(owner.ToString(), "Enemy spotted! Attacking " + entry.Target.Name + entry.Target.UniqueID);

            bUpdatePath = true;
            path = null;

            MoveOrAttack(entry.Target);
        }

        private void MoveOrAttack(Entity ent)
        {
            if (leaveRequested)
            {
                isMoving = false;
                return;
            }
            
            var owner = Owner;
            if (owner == null)
            {
                return;
            }
            
            float offx = owner.X - ent.X;
            float offy = owner.Y - ent.Y;

            double sqrDist = (offx * offx + offy * offy);

            double sqrMeleeRange = ent.AttackRange * ent.AttackRange;

            if (sqrDist < sqrMeleeRange)
            {
                if (owner.CanAttack && ent.ShouldBeAttacked)
                {
                    // Target is in range -> Perform an attack
                    owner.PerformAttack(ent);
                }
            }
            else
            {
                // Target is out of range -> Move towards it

                Log.AI(this.Owner?.ToString(), "Target is out of range ... moving towards it!");

                // If no path has been calculated yet or if the target has moved, calculate new path
                if (targetPath == null || ent.X != targetX || ent.Y != targetY)
                {
                    targetX = ent.TileX;
                    targetY = ent.TileY;
                    targetPath = owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, targetX, targetY);
                    if (targetPath == null)
                    {
                        return;
                    }

                    curNodeIdx = 0;
                }

                IMapPathNode node = targetPath[curNodeIdx++];
                // TODO!!! Move, not Set
                owner.SetPosition(node.X, node.Y);
            }
        }
    }
}