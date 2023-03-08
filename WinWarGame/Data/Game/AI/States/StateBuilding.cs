using System;
using Microsoft.Xna.Framework;
using WinWarGame.Data.Resources;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
{
   class StateBuilding : State
   {
      private double currentBuildingTime;
      private LevelObjectType? currentEntityType;
      private BuildEntity currentBuildingEntity;

      internal StateBuilding(Entity Owner)
         : base(Owner)
      {
      }

      private void DequeueNewEntity()
      {
         currentEntityType = null;
         if (currentBuildingEntity.BuildQueue.Count > 0)
            currentEntityType = currentBuildingEntity.BuildQueue.Dequeue();

         // TODO: Get building time for entity type
         currentBuildingTime = 1.0f; // ((float)currentEntity.TimeToBuild / 100.0f);
      }

      internal override bool Enter()
      {
         if (Owner is BuildEntity)
            currentBuildingEntity = (BuildEntity)Owner;
         else
            return false;

         DequeueNewEntity();

         return true;
      }

      internal override void Update(GameTime gameTime)
      {
         if (currentEntityType == null && this.Owner != null)
         {
            this.Owner.Idle();
            return;
         }

         currentBuildingTime -= gameTime.ElapsedGameTime.TotalSeconds;

         if (currentBuildingTime <= 0)
         {
            // TODO: Find empty spot to spawn
            int x, y, x2, y2;

            x = this.Owner.TileX;
            y = this.Owner.TileY - 1;

            bool bFound = false;
            for (y2 = -1; y2 <= currentBuildingEntity.TileSizeY; y2++)
            {
               if (bFound)
                  break;

               for (x2 = -1; x2 <= currentBuildingEntity.TileSizeX; x2++)
               {
                  if (bFound)
                     break;

                  if (x2 == 0 && y2 == 0)
                     continue;

                  x = this.Owner.TileX + x2;
                  y = this.Owner.TileY + y2;

                  if (Owner.CurrentMap.GetEntityAt(x, y) == null)
                  {
                     bFound = true;
                  }
               }
            }

            if (!bFound)
            {
               Log.Write(LogType.Generic, LogSeverity.Warning, "No place found to spawn unit of type ''");
               return;
            }

            this.Owner.CurrentMap.CreateEntity(x, y, currentEntityType.Value, this.Owner.Owner);

            // Grab new unit
            DequeueNewEntity();
         }
      }
   }
}

