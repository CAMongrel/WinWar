using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
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

         Log.AI(owner.ToString(), "Requesting state change to " + requestedNewState.ToString());

         State previousState = CurrentState;

         Log.AI(owner.ToString(), "Current state ist " + (previousState != null ? previousState.ToString() : "null"));

         if (CurrentState != null)
         {
            bool didLeave = CurrentState.Leave();
            if (didLeave == false)
            {
               Log.AI(owner.ToString(), "Current state rejects leave, aborting state change and trying again later.");
               return;
            }
         }

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
