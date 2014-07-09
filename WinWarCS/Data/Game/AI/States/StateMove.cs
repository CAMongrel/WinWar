using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
   class StateMove : State
   {
      internal int targetX;
      internal int targetY;

      internal double moveTimer;

      internal int curNodeIdx;

      internal List<Node> Path;

      internal StateMove(Entity Owner, int targetX, int targetY)
         : base(Owner)
      {
         this.targetX = targetX;
         this.targetY = targetY;
      }

      internal override void Enter()
      {
         moveTimer = 1.0f;
         curNodeIdx = -1;
         Path = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, targetX, targetY);
         if (Path != null)
            curNodeIdx = 0;
      }

      internal override void Update(GameTime gameTime)
      {
         moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
         if (moveTimer > 0)
            return;

         moveTimer = 1.0f;

         if (curNodeIdx == -1 || Path == null)
            return;

         if (curNodeIdx >= Path.Count)
         {
            ((BuildEntity)this.Owner).Idle();
            return;
         }

         Node node = Path[curNodeIdx++];
         // TODO!!! Move, not Set
         Owner.SetPosition (node.X, node.Y);
      }
   }
}

