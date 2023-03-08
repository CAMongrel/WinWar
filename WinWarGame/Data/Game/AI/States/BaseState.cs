using System;
using Microsoft.Xna.Framework;

namespace WinWarGame.Data.Game
{
   abstract class State
   {
      internal protected Entity Owner;

      internal State(Entity Owner)
      {
         this.Owner = Owner;
      }

      internal virtual bool Enter()
      {
         return true;
      }

      internal virtual void Update(GameTime gameTime)
      {
      }

      internal virtual void Leave()
      {
      }
   }
}

