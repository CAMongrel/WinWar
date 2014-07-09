using System;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
   abstract class State
   {
      internal protected Entity Owner;

      internal State(Entity Owner)
      {
         this.Owner = Owner;
      }

      internal virtual void Enter()
      {
      }

      internal virtual void Update(GameTime gameTime)
      {
      }

      internal virtual void Leave()
      {
      }
   }
}

