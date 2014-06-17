using System;
using System.Collections.Generic;
using System.Text;

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

		internal virtual void Update(float FrameInterval)
		{
		}

		internal virtual void Leave()
		{
		}
	}

	class StateBuilding : State
	{
		float currentBuildingTime;
		BuildEntity currentEntity;
		BuildEntity currentBuildingEntity;

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

		internal override void Update(float FrameInterval)
		{
			if (currentEntity == null)
			{
				((BuildEntity)this.Owner).Idle();
				return;
			}

			currentBuildingTime -= FrameInterval;

			if (currentBuildingTime <= 0)
			{
				// TODO: Find empty spot to spawn
				float x, y, x2, y2;

				x = this.Owner.X;
				y = this.Owner.Y - 1;

				bool bFound = false;
				for (y2 = -1; y2 <= currentBuildingEntity.TileSizeY; y2++)
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
				}

				if (!bFound)
				{
					Log.logWarning("No place found to spawn entity!");
					return;
				}

				BuildEntity entity = EntitySets.CreateBuildEntityFromEntityID(currentEntity.EntityID, this.Owner.Owner);
				entity.X = x;
				entity.Y = y;
				entity.UniqueID = Level.NextUniqueID;
				Log.logStatus("unit.UniqueID: " + entity.UniqueID);

				entity.Width = -1;
				entity.Height = -1;

				entity.Idle();

				Log.logAI(currentBuildingEntity, "Spawning a " + entity.Name + " at " + entity.X + "," + entity.Y);
				Level.InvokeBuildingComplete(entity);

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

	class StateAttack : State
	{
		internal int targetX;
		internal int targetY;

		internal float attackTimer;

		internal int curNodeIdx;

		internal List<Node> Path;

		internal StateAttack(Entity Owner, Entity Target)
			: base(Owner)
		{
			if (Target == null)
				throw new ArgumentNullException("Target");

			Owner.CurrentTarget = Target;
			Path = null;
		}

		internal override void Enter()
		{
			Owner.SetHateValue(Owner.CurrentTarget, 25, HateListParam.PushToTop);

			Log.logAI(this.Owner, "Attacking " + Owner.CurrentTarget.Name + Owner.CurrentTarget.UniqueID);

			targetX = Owner.CurrentTarget.X;
			targetY = Owner.CurrentTarget.Y;

			attackTimer = ((BuildEntity)Owner).AttackSpeed;
		}

		internal override void Update(float FrameInterval)
		{
			attackTimer -= FrameInterval;
			if (attackTimer > 0)
				return;

			attackTimer = ((BuildEntity)Owner).AttackSpeed;

			this.Owner.UpdateHateList();

			HateListEntry entry = this.Owner.GetHighestHateListEntry();
			if (entry == null)
			{
				((BuildEntity)this.Owner).Idle();
				return;
			}

			MoveOrAttack(entry.Target);
		}

		private void MoveOrAttack(Entity ent)
		{
			int offx = this.Owner.X - ent.X;
			int offy = this.Owner.Y - ent.Y;

			float sqr_dist = (offx * offx + offy * offy);

			float sqr_meleerange = 3.0f;
			if (ent is BuildEntity)
				sqr_meleerange = ((BuildEntity)ent).AttackRange * ((BuildEntity)ent).AttackRange;

			if (sqr_dist < sqr_meleerange)
			{
				// Target is in range -> Perform an attack
				if (this.Owner is BuildEntity && ent is BuildEntity)
					((BuildEntity)this.Owner).PerformAttack((BuildEntity)ent);
			}
			else
			{
				// Target is out of range -> Move towards it

				// If no path has been calculated yet or if the target has moved, calculate new path
				if (Path == null || ent.X != targetX || ent.Y != targetY)
				{
					targetX = ent.X;
					targetY = ent.Y;
					Path = StateMachine.CalcPath(Owner.X, Owner.Y, targetX, targetY);
					if (Path == null)
						return;
					curNodeIdx = 0;
				}

				if (curNodeIdx < Path.Count)
				{
					Node node = Path[curNodeIdx++];
					Owner.X = node.X;
					Owner.Y = node.Y;
				}
				else
					Log.logError("Error calculating path");
			}
		}
	}

	class StateAttackMove : State
	{
		internal int destX;
		internal int destY;
		internal int targetX;
		internal int targetY;

		internal bool bUpdatePath;

		internal int curNodeIdx;

		internal float moveTimer;

		internal List<Node> Path;
		internal List<Node> TargetPath;

		internal StateAttackMove(Entity Owner, int X, int Y)
			: base(Owner)
		{
			Path = null;
			TargetPath = null;
			destX = X;
			destY = Y;
			bUpdatePath = true;
		}

		internal override void Enter()
		{
			moveTimer = 1.0f;
			curNodeIdx = -1;
			Path = StateMachine.CalcPath(Owner.X, Owner.Y, destX, destY);
			if (Path != null)
			{
				curNodeIdx = 0;
				bUpdatePath = false;
			}
		}

		internal override void Update(float FrameInterval)
		{
			moveTimer -= FrameInterval;
			if (moveTimer > 0)
				return;

			moveTimer = 1.0f;

			this.Owner.UpdateHateList();

			HateListEntry entry = this.Owner.GetHighestHateListEntry();
			if (entry == null)
			{
				Log.logAI(this.Owner, "No enemy ... moving!");

				if (bUpdatePath)
				{
					Path = StateMachine.CalcPath(Owner.X, Owner.Y, destX, destY);
					if (Path != null)
					{
						curNodeIdx = 0;
						bUpdatePath = false;
					}
				}

				if (curNodeIdx == -1 || Path == null)
					return;

				if (curNodeIdx >= Path.Count)
				{
					((BuildEntity)this.Owner).Idle();
					return;
				}

				Node node = Path[curNodeIdx++];
				Owner.X = node.X;
				Owner.Y = node.Y;
				return;
			}

			this.Owner.CurrentTarget = entry.Target;

			Log.logAI(this.Owner, "Enemy spotted! Attacking " + entry.Target.Name + entry.Target.UniqueID);

			bUpdatePath = true;
			Path = null;

			MoveOrAttack(entry.Target);
		}

		private void MoveOrAttack(Entity ent)
		{
			int offx = this.Owner.X - ent.X;
			int offy = this.Owner.Y - ent.Y;

			float sqr_dist = (offx * offx + offy * offy);

			float sqr_meleerange = 3.0f;
			if (ent is BuildEntity)
				sqr_meleerange = ((BuildEntity)ent).AttackRange * ((BuildEntity)ent).AttackRange;

			if (sqr_dist < sqr_meleerange)
			{
				if (((BuildEntity)this.Owner).GetRuleState(EntityRules.Attack))
				{
					// Target is in range -> Perform an attack
					if (this.Owner is BuildEntity && ent is BuildEntity)
						((BuildEntity)this.Owner).PerformAttack((BuildEntity)ent);
				}
			}
			else
			{
				// Target is out of range -> Move towards it

				Log.logAI(this.Owner, "Target is out of range ... moving towards it!");

				// If no path has been calculated yet or if the target has moved, calculate new path
				if (TargetPath == null || ent.X != targetX || ent.Y != targetY)
				{
					targetX = ent.X;
					targetY = ent.Y;
					TargetPath = StateMachine.CalcPath(Owner.X, Owner.Y, targetX, targetY);
					if (TargetPath == null)
						return;
					curNodeIdx = 0;
				}

				Node node = TargetPath[curNodeIdx++];
				Owner.X = node.X;
				Owner.Y = node.Y;
			}
		}
	}

	class StateIdle : State
	{
		internal float updateTimer;

		internal StateIdle(Entity Owner)
			: base(Owner)
		{
		}

		internal override void Enter()
		{
			updateTimer = 1.0f;
			this.Owner.WipeHatelist();
		}

		internal override void Update(float FrameInterval)
		{
			updateTimer -= FrameInterval;
			if (updateTimer > 0)
				return;

			updateTimer = 1.0f;

			float sqr_aggrorange = 50.0f;

			foreach (Player pl in Level.Players)
			{
				if (pl == this.Owner.Owner || pl.IsNeutral)
					continue;

				foreach (Entity ent in pl.entities)
				{
					int offx = this.Owner.X - ent.X;
					int offy = this.Owner.Y - ent.Y;

					float dist = (offx * offx + offy * offy);

					if (dist < sqr_aggrorange)
					{
						((BuildEntity)this.Owner).Attack(ent);
					}
				}
			}
		}
	}

	class StateMove : State
	{
		internal int targetX;
		internal int targetY;

		internal float moveTimer;

		internal int curNodeIdx;

		internal List<Node> Path;

		internal StateMove(Entity Owner, int targetX, int targetY)
			: base(Owner)
		{
			this.targetX = targetX;
			this.targetY = targetY;
		}

		internal override void Enter()
		{
			moveTimer = 1.0f;
			curNodeIdx = -1;
			Path = StateMachine.CalcPath(Owner.X, Owner.Y, targetX, targetY);
			if (Path != null)
				curNodeIdx = 0;
		}

		internal override void Update(float FrameInterval)
		{
			moveTimer -= FrameInterval;
			if (moveTimer > 0)
				return;

			moveTimer = 1.0f;

			if (curNodeIdx == -1 || Path == null)
				return;

			if (curNodeIdx >= Path.Count)
			{
				((BuildEntity)this.Owner).Idle();
				return;
			}

			Node node = Path[curNodeIdx++];
			Owner.X = node.X;
			Owner.Y = node.Y;
		}
	}

	class StateDeath : State
	{
		internal StateDeath(Entity Owner)
			: base(Owner)
		{
		}

		internal override void Enter()
		{
			//
		}
	}

	class StateMachine
	{
		internal static void ChangeState(State newState)
		{
			if (newState == null)
				throw new ArgumentNullException("newState");
			if (newState.Owner == null)
				throw new ArgumentNullException("newState.owner");

			Entity owner = newState.Owner;

			Log.logAI((BuildEntity)owner, "Changing state to " + newState.ToString());

			owner.previousState = owner.currentState;

			Log.logAI((BuildEntity)owner, "Previous state was " + (owner.previousState != null ? owner.previousState.ToString() : "null"));

			if (owner.currentState != null)
				owner.currentState.Leave();

			owner.currentState = newState;

			owner.currentState.Enter();
		}

		internal static List<Node> CalcPath(int startX, int startY, int endX, int endY)
		{
			Level.Pathfinder.StartX = startX;
			Level.Pathfinder.StartY = startY;
			Level.Pathfinder.EndX = endX;
			Level.Pathfinder.EndY = endY;

			Log.logStatus("StateMachine: Calculating path from " + startX + "," + 
				startY + " to " + endX + "," + endY + "...");

			if (Level.Pathfinder.FindPath())
			{
				List<Node> Path = new List<Node>(Level.Pathfinder.PathNodeCount);
				for (int i = 0; i < Level.Pathfinder.PathNodeCount; i++)
					Path.Add(Level.Pathfinder.GetPathNode(i));
				Log.logStatus("... success (" + Path.Count + " Nodes)!");
				return Path;
			}

			Log.logStatus("... failed!");

			return null;
		}
	}
}
