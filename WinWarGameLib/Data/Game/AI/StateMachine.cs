using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Util;
using Microsoft.Xna.Framework;

namespace WinWarCS.Data.Game
{
	class StateMachine
	{
      private State requestedNewState;
      internal State CurrentState { get; private set; }

      private Entity owner;

      internal StateMachine(Entity setOwner)
      {
         owner = setOwner;
         CurrentState = null;
         requestedNewState = null;
      }

      internal void ChangeState(State newState)
		{
         requestedNewState = newState;
		}

      private void PerformChangeState()
      {
         if (requestedNewState == null)
            throw new ArgumentNullException("requestedNewState");
         if (requestedNewState.Owner == null)
            throw new ArgumentNullException("requestedNewState.owner");

         Log.AI(owner.ToString(), "Changing state to " + requestedNewState.ToString());

         State previousState = CurrentState;

         Log.AI(owner.ToString(), "Previous state was " + (previousState != null ? previousState.ToString() : "null"));

         if (CurrentState != null)
            CurrentState.Leave();

         CurrentState = requestedNewState;
         requestedNewState = null;

         if (CurrentState.Enter() == false)
         {
            CurrentState = null;
            Log.AI(owner.ToString(), "Failed to enter new state. Reverting to previous state.");
            ChangeState(previousState);
         }
      }

      internal void Update(GameTime gameTime)
      {
         if (requestedNewState != null)
         {
            PerformChangeState();
         }

         if (CurrentState != null)
            CurrentState.Update (gameTime);
      }
	}
}
