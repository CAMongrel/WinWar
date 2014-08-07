using System;
using WinWarCS.Util;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
   class StateBuilding : State
   {
      private double currentBuildingTime;
      private Entity currentEntity;
      private BuildEntity currentBuildingEntity;

      internal StateBuilding(Entity Owner)
         : base(Owner)
      {
         if (!(Owner is BuildEntity))
            throw new Exception("Only build entities can build other entities");

         currentBuildingEntity = (BuildEntity)Owner;
      }

      internal override void Enter()
      {
         try
         {
            currentEntity = currentBuildingEntity.BuildQueue.Dequeue();
         }
         catch (Exception)
         {
            currentEntity = null;
         }

         if (currentEntity != null)
            currentBuildingTime = ((float)currentEntity.TimeToBuild / 100.0f);
      }

      internal override void Update(GameTime gameTime)
      {
         if (currentEntity == null && this.Owner != null)
         {
            this.Owner.Idle();
            return;
         }

         currentBuildingTime -= gameTime.ElapsedGameTime.TotalSeconds;

         if (currentBuildingTime <= 0)
         {
            // TODO: Find empty spot to spawn
            float x, y, x2, y2;

            x = this.Owner.X;
            y = this.Owner.Y - 1;

            bool bFound = false;
            //TODO!!! 
            /*for (y2 = -1; y2 <= currentBuildingEntity.TileSizeY; y2++)
            {
               if (bFound)
                  break;

               for (x2 = -1; x2 <= currentBuildingEntity.TileSizeX; x2++)
               {
                  if (bFound)
                     break;

                  if (x2 == 0 && y2 == 0)
                     continue;

                  x = this.Owner.X + x2;
                  y = this.Owner.Y + y2;

                  if (Owner.CurrentMap.GetEntityAt(x, y) == null)
                  {
                     bFound = true;
                  }
               }
            }*/

            if (!bFound)
            {
               Log.Warning("No place found to spawn entity!");
               return;
            }

            BuildEntity entity = null;//TODO!!! EntitySets.CreateBuildEntityFromEntityID(currentEntity.EntityID, this.Owner.Owner);
            //TODO!!! entity.X = x;
            //TODO!!! entity.Y = y;
            //TODO!!! entity.UniqueID = Level.NextUniqueID;
            //TODO!!! Log.Status("unit.UniqueID: " + entity.UniqueID);

            //TODO!!! entity.Width = -1;
            //TODO!!! entity.Height = -1;

            //TODO!!! entity.Idle();
            //TODO!!! entity.DidSpawn ();

            //TODO!!! Log.AI(currentBuildingEntity, "Spawning a " + entity.Name + " at " + entity.X + "," + entity.Y);
            //TODO!!! Level.InvokeBuildingComplete(entity);

            // Grab new unit
            try
            {
               currentEntity = currentBuildingEntity.BuildQueue.Dequeue();
            }
            catch (Exception ex)
            {
               currentEntity = null;
            }
            if (currentEntity != null)
               currentBuildingTime = ((float)currentEntity.TimeToBuild / 100.0f);
         }
      }
   }
}

