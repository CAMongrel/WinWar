using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
{
    class StateAttack : State
    {
        private int targetX;
        private int targetY;

        private double attackTimer;

        private int curNodeIdx;

        private MapPath Path;

        internal StateAttack(Entity owner, Entity target)
            : base(owner)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            owner.CurrentTarget = target;
            Path = null;
        }

        internal override bool Enter()
        {
            var owner = Owner;
            if (owner == null)
            {
                return false;
            }
            
            if (owner is Unit unit)
            {
                unit.Sprite.SetCurrentAnimationByName("Attack");
            }

            owner.HateList.SetHateValue(owner.CurrentTarget, 25, HateListParam.PushToTop);

            Log.AI(owner.ToString(), "Attacking " + owner.CurrentTarget.Name + owner.CurrentTarget.UniqueId);

            targetX = owner.CurrentTarget.TileX;
            targetY = owner.CurrentTarget.TileY;

            // TODO: This may lead to bugs, if the user manages to quickly switch states.
            // This must be fixed by moving the attackTimer to the unit itself.
            attackTimer = 0;

            return true;
        }

        internal override void Update(GameTime gameTime)
        {
            attackTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (attackTimer > 0)
            {
                return;
            }
            
            var owner = Owner;
            if (owner == null)
            {
                return;
            }

            attackTimer = owner.AttackSpeed;

            owner.UpdateHateList();

            HateListEntry entry = owner.HateList.GetHighestHateListEntry();
            if (entry.Target == null)
            {
                owner.Idle();
                return;
            }

            MoveOrAttack(entry.Target);
        }

        private void MoveOrAttack(Entity ent)
        {
            var owner = Owner;
            if (owner == null)
            {
                return;
            }
            
            float offx = ent.X - this.Owner.X;
            float offy = ent.Y - this.Owner.Y;

            double sqrDist = (offx * offx + offy * offy);

            double sqrMeleerange = ent.AttackRange * ent.AttackRange;

            if (sqrDist < sqrMeleerange)
            {
                // Target is in range -> Perform an attack
                if (owner.PerformAttack(ent))
                {
                    if (owner is Unit unit)
                    {
                        unit.Orientation = Unit.OrientationFromDiff(offx, offy);
                        unit.Sprite.CurrentAnimation.Reset();
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
                    Path = owner.CurrentMap.CalcPath(owner.TileX, owner.TileY, targetX, targetY);
                    if (Path == null)
                    {
                        return;
                    }

                    curNodeIdx = 0;
                }

                if (curNodeIdx < Path.Count)
                {
                    IMapPathNode node = Path[curNodeIdx++];
                    // TODO!!! Move, not Set
                    owner.SetPosition(node.X, node.Y);
                }
                else
                {
                    Log.Error("Error calculating path");
                }
            }
        }
    }
}