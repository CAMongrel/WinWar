using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Util;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
	class StateMachine
	{
      private State previousState;
      internal State CurrentState { get; private set; }

      private Entity owner;

      internal StateMachine(Entity setOwner)
      {
         owner = setOwner;
         previousState = null;
         CurrentState = null;
      }

      internal void ChangeState(State newState)
		{
			if (newState == null)
				throw new ArgumentNullException("newState");
			if (newState.Owner == null)
				throw new ArgumentNullException("newState.owner");

			Log.AI(owner, "Changing state to " + newState.ToString());

         previousState = CurrentState;

			Log.AI(owner, "Previous state was " + (previousState != null ? previousState.ToString() : "null"));

         if (CurrentState != null)
            CurrentState.Leave();

         CurrentState = newState;

         CurrentState.Enter();
		}

      internal void Update(GameTime gameTime)
      {
         if (CurrentState != null)
            CurrentState.Update (gameTime);
      }
	}
}
