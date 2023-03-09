using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
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

      internal MapPath Path;

      private bool isMoving;
      private bool leaveRequested;

      internal StateMove(Entity Owner, int targetTileX, int targetTileY)
         : base(Owner)
      {
         this.targetTileX = targetTileX;
         this.targetTileY = targetTileY;

         isMoving = false;
         leaveRequested = false;
      }

      internal override bool Enter()
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

            IMapPathNode initialNode = Path[curNodeIdx++];
            if (initialNode == null)
            {
               return false;
            }

            targetPosX = initialNode.X;
            targetPosY = initialNode.Y;

            if (Owner is Unit)
            {
               Unit unit = (Unit)Owner;
               unit.Sprite.SetCurrentAnimationByName("Walk");
               unit.Orientation = Unit.OrientationFromDiff((targetPosX - startPosX), (targetPosY - startPosY));
            }
         }
         else
         {
            return false;
         }

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
            isMoving = true;
            
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
         {
            leaveRequested = false;
            isMoving = false;
            return;
         }

         if (curNodeIdx >= Path.Count - 1)
         {
            leaveRequested = false;
            isMoving = false;
            Owner.SetPosition (targetPosX, targetPosY);
            this.Owner.Idle();
            return;
         }

         if (leaveRequested)
         {
            isMoving = false;
            return;
         }

         IMapPathNode node = Path[curNodeIdx++];

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

