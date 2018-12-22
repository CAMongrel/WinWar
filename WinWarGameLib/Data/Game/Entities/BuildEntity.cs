using System;
using System.Collections.Generic;
using WinWarCS.Util;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class BuildEntity : Entity
   {
      internal Queue<LevelObjectType> BuildQueue { get; private set; }

      public BuildEntity (Map currentMap)
         : base(currentMap)
      {
         BuildQueue = new Queue<LevelObjectType> ();
      }

      /// <summary>
      /// Orders this entity to build another entity
      /// </summary>
      /// <param name="EntityID">The ID of the entity to be built</param>
      public void Build(LevelObjectType EntityID)
      {
         if (!CanBuild)
            return;

         Log.AI(this?.ToString(), "Building '" + EntityID + "'");

         if (StateMachine.CurrentState is StateBuilding)
         {
            BuildQueue.Enqueue(EntityID);
         }
         else
         {
            BuildQueue.Clear();
            BuildQueue.Enqueue(EntityID);
            StateMachine.ChangeState(new StateBuilding(this));
         }
      } // Build(EntityID)

   }
}

