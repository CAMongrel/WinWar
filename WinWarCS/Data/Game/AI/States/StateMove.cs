using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WinWarCS.Util;

namespace WinWarCS.Data.Game
{
   class StateMove : State
   {
      internal int targetTileX;
      internal int targetTileY;

      private float startPosX;
      private float startPosY;
      private float movePosX;
      private float movePosY;
      private float targetPosX;
      private float targetPosY;

      private double walkDistance;
      internal double moveTimer;

      internal int curNodeIdx;

      internal List<Node> Path;

      internal StateMove(Entity Owner, int targetTileX, int targetTileY)
         : base(Owner)
      {
         this.targetTileX = targetTileX;
         this.targetTileY = targetTileY;
      }

      internal override void Enter()
      {
         moveTimer = Owner.WalkSpeed;
         walkDistance = Owner.WalkSpeed;
         curNodeIdx = -1;
         Path = Owner.CurrentMap.CalcPath(Owner.TileX, Owner.TileY, targetTileX, targetTileY);

         startPosX = Owner.X;
         startPosY = Owner.Y;

         if (Path != null) 
         {
            curNodeIdx = 0;

            Node initialNode = Path[curNodeIdx++];

            targetPosX = initialNode.X;
            targetPosY = initialNode.Y;

            if (Owner is Unit) 
            {
               Unit unit = (Unit)Owner;
               unit.Sprite.SetCurrentAnimationByName ("Walk");
               unit.Orientation = Unit.OrientationFromDiff ((targetPosX - startPosX), (targetPosY - startPosY));
            }
         }
      }

      internal override void Update(GameTime gameTime)
      {
         moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
         if (moveTimer > 0) 
         {
            float scale = (float)(1.0f - (moveTimer / walkDistance));

            movePosX = scale * (targetPosX - startPosX);
            movePosY = scale * (targetPosY - startPosY);

            Owner.SetPosition (startPosX + movePosX, startPosY + movePosY);
            return;
         }

         double remainingDiff = -moveTimer;

         Owner.SetPosition (targetPosX, targetPosY);

         walkDistance = Owner.WalkSpeed + remainingDiff;
         moveTimer = walkDistance;

         startPosX = Owner.X;
         startPosY = Owner.Y;

         movePosX = startPosX;
         movePosY = startPosY;

         if (curNodeIdx == -1 || Path == null)
            return;

         if (curNodeIdx >= Path.Count - 1)
         {
            Owner.SetPosition (targetPosX, targetPosY);
            this.Owner.Idle();
            return;
         }

         Node node = Path[curNodeIdx++];

         targetPosX = node.X;
         targetPosY = node.Y;

         if (Owner is Unit) 
         {
            Unit unit = (Unit)Owner;
            unit.Orientation = Unit.OrientationFromDiff ((targetPosX - startPosX), (targetPosY - startPosY));
         }
      }
   }
}

