using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Game
{
   enum PlayerType
   {
      Human,
      AI
   }

   internal class BasePlayer
   {
      internal string Name { get; set; }

      internal Race Race { get; set; }

      internal PlayerType PlayerType { get; private set; }

      internal List<Entity> Entities { get; private set; }

      internal BasePlayer (PlayerType setPlayerType)
      {
         Name = "Player";
         Race = Game.Race.Humans;
         PlayerType = setPlayerType;
         Entities = new List<Entity> ();
      }

      internal void ClaimeOwnership(Entity setEntity)
      {
         if (setEntity == null)
            return;

         if (setEntity.Owner == this)
            return;

         Entities.Add (setEntity);

         setEntity.AssignOwner (this);
      }

      internal void RemoveOwnership(Entity setEntity)
      {
         if (setEntity == null)
            return;

         if (setEntity.Owner != this)
            return;

         if (Entities.Contains (setEntity))
            Entities.Remove (setEntity);

         setEntity.AssignOwner(null);
      }

      public bool IsNeutralTowards (BasePlayer player)
      {
         if (player == null)
            return true;

         return false;
      }

      public bool IsHostileTowards (BasePlayer player)
      {
         if (player == null)
            return false;

         if (player != this)
            return true;

         return false;
      }

      public bool IsFriendlyTowards (BasePlayer player)
      {
         if (player == null)
            return false;

         if (player == this)
            return true;

         return false;
      }
   }
}
