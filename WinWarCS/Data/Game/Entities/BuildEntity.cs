using System;
using System.Collections.Generic;

namespace WinWarCS.Data.Game
{
   internal class BuildEntity : Entity
   {
      internal Queue<BuildEntity> BuildQueue { get; private set; }

      public BuildEntity (Map currentMap)
         : base(currentMap)
      {
         BuildQueue = new Queue<BuildEntity> ();
      }
   }
}

