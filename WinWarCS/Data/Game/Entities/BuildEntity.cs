using System;
using System.Collections.Generic;
using WinWarCS.Util;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class BuildEntity : Entity
   {
      internal Queue<Entity> BuildQueue { get; private set; }

      public BuildEntity (Map currentMap)
         : base(currentMap)
      {
         BuildQueue = new Queue<Entity> ();
      }

      /// <summary>
      /// Orders this entity to build another entity
      /// </summary>
      /// <param name="EntityID">The ID of the entity to be built</param>
      public void Build(LevelObjectType EntityID)
      {
         if (!CanBuild)
            return;

         Entity be = Entity.CreateEntityFromType (EntityID, this.CurrentMap);
         if (be == null)
            return;

         Log.AI(this, "Building '" + be.Name + "'");

         if (StateMachine.CurrentState is StateBuilding)
            BuildQueue.Enqueue(be);
         else
         {
            BuildQueue.Clear();
            BuildQueue.Enqueue(be);
            StateMachine.ChangeState(new StateBuilding(this));
         }
      } // Build(EntityID)

   }
}

