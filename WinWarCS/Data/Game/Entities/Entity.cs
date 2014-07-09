using System;
using WinWarCS.Data.Resources;
using Microsoft.Xna.Framework;
using WinWarCS.Util;


#if NETFX_CORE
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#else
using RectangleF = System.Drawing.RectangleF;
#endif

namespace WinWarCS.Data.Game
{
   internal class Entity
   {
      protected Sprite sprite;

      public float X { get; private set; }
      public float Y { get; private set; }

      public int TileX 
      { 
         get
         { 
            return (int)(X / (float)CurrentMap.TileWidth); 
         }
      }
      public int TileY
      { 
         get
         { 
            return (int)(Y / (float)CurrentMap.TileHeight); 
         }
      }

      public BasePlayer Owner { get; private set; }

      public HateList HateList { get; private set; }

      public StateMachine StateMachine { get; private set; }

      public Map CurrentMap { get; private set; }

      private Entity currentTarget;

      public Entity PreviousTarget { get; private set; }

      public string Name
      {
         get
         {
            return ToString ();
         }
      }

      public int UniqueID { get; set; }

      /// <summary>
      /// Attack range
      /// </summary>
      public byte AttackRange;
      /// <summary>
      /// The attack speed.
      /// </summary>
      public double AttackSpeed;
      /// <summary>
      /// Armor points
      /// </summary>
      public short ArmorPoints;
      /// <summary>
      /// Hit points
      /// </summary>
      public short HitPoints;
      /// <summary>
      /// Maximum hit points
      /// </summary>
      public short MaxHitPoints;
      /// <summary>
      /// Minimum damage
      /// </summary>
      public byte MinDamage;
      /// <summary>
      /// Random damage
      /// </summary>
      public byte RandomDamage;
      /// <summary>
      /// Time to build
      /// </summary>
      public short TimeToBuild;
      /// <summary>
      /// Gold cost
      /// </summary>
      public short GoldCost;
      /// <summary>
      /// Lumber cost
      /// </summary>
      public short LumberCost;
      /// <summary>
      /// Decay rate
      /// </summary>
      public short DecayRate;

      public Entity (Map currentMap)
      {
         currentTarget = null;
         PreviousTarget = null;

         CurrentMap = currentMap;
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Peasant")));

         HateList = new HateList ();
         StateMachine = new StateMachine (this);
         StateMachine.ChangeState (new StateIdle (this));
      }

      /// <summary>
      /// Sets the position.
      /// </summary>
      /// <param name="newX">New x.</param>
      /// <param name="newY">New y.</param>
      public void SetPosition(float newX, float newY)
      {
         X = newX;
         Y = newY;
      }

      /// <summary>
      /// Assigns the owner. Do not call manually.
      /// </summary>
      /// <param name="owner">New owner. May be null</param>
      public void AssignOwner(BasePlayer setOwner)
      {
         Owner = setOwner;
      }

      /// <summary>
      /// Render the Entity.
      /// </summary>
      /// <param name="offsetX">Offset x.</param>
      /// <param name="offsetY">Offset y.</param>
      /// <param name="tileOffsetX">Tile offset x.</param>
      /// <param name="tileOffsetY">Tile offset y.</param>
      /// <param name="TileWidth">Tile width.</param>
      /// <param name="TileHeight">Tile height.</param>
      public void Render(float offsetX, float offsetY, float tileOffsetX, float tileOffsetY, int TileWidth, int TileHeight)
      {
         if (sprite == null)
            return;

         int startTileX = ((int)tileOffsetX / TileWidth);
         int startTileY = ((int)tileOffsetY / TileHeight);

         bool shouldFlipX = sprite.ShouldFlipX;
         SpriteFrame curFrame = sprite.CurrentFrame;

         RectangleF rect = new RectangleF ();
         rect.X = offsetX + (X - startTileX) * TileWidth - (TileWidth / 2);
         if (shouldFlipX)
            rect.X += sprite.MaxWidth;
         rect.Y = offsetY + (Y - startTileY) * TileHeight - (TileHeight / 2);
         rect.Width = shouldFlipX ? -sprite.MaxWidth : sprite.MaxWidth;
         rect.Height = sprite.MaxHeight;

         curFrame.texture.RenderOnScreen (rect);
      }

      /// <summary>
      /// Updates the hate list by adding new entities to it, that just entered 
      /// the aggro range.
      /// Note: This is done automatically by the game each tick and does not 
      /// need to be called from script.
      /// </summary>
      public void UpdateHateList()
      {
         float sqr_aggrorange = 0;//aggrorange * aggrorange;

         for (int i = 0; i < CurrentMap.Players.Count; i++)
         {
            BasePlayer pl = CurrentMap.Players[i];
            if (pl == this.Owner || pl.IsNeutralTowards(this))
               continue;

            foreach (Entity ent in pl.Entities)
            {
               float offx = X - ent.X;
               float offy = Y - ent.Y;

               float dist = (offx * offx + offy * offy);

               if (dist < sqr_aggrorange)
               {
                  HateList.SetHateValue(ent, 1, HateListParam.IgnoreIfPresent);
               }
            } // foreach
         } // foreach
      } // UpdateHateList()

      /// <summary>
      /// Update the specified gameTime.
      /// </summary>
      /// <param name="gameTime">Game time.</param>
      internal void Update(GameTime gameTime)
      {
         if (StateMachine != null) 
         {
            StateMachine.Update (gameTime);
         }
      }

      /// <summary>
      /// Gets or sets the current target entity of this entity.
      /// </summary>
      public Entity CurrentTarget
      {
         get { return currentTarget; }
         set
         {
            PreviousTarget = currentTarget;
            currentTarget = value;
         }
      } // CurrentTarget

      /// <summary>
      /// Orders the unit to be idle. This stops movement and clears the hate list.
      /// </summary>
      public void Idle()
      {
         Log.AI(this, "Idling...");
         StateMachine.ChangeState(new StateIdle(this));
      } // Idle()

      /// <summary>
      /// Orders the entity to move to the coordinates and attack everything it passes on its way to the target
      /// </summary>
      /// <param name="x">Target X tile</param>
      /// <param name="y">Target Y tile</param>
      public void AttackMove(int x, int y)
      {
         if (CanMove)
         {
            Log.AI(this, "Moving to " + x + "," + y + " and attacking all enemies on the way!");

            StateMachine.ChangeState(new StateAttackMove(this, x, y));
         }
      } // AttackMove(x, y)

      /// <summary>
      /// Orders the entity to attack the given target
      /// </summary>
      /// <param name="Target">The entity to attack</param>
      public void Attack(Entity Target)
      {
         if (CanAttack)
         {
            Log.AI(this, "Attacking " + Target.Name + Target.UniqueID);

            StateMachine.ChangeState(new StateAttack(this, Target));
         }
      } // Attack(Target)

      /// <summary>
      /// Orders the unit to move to the coordinates ignoring all enemy entities on the way.
      /// </summary>
      /// <param name="x">Target X tile</param>
      /// <param name="y">Target Y tile</param>
      public void MoveTo(int x, int y)
      {
         if (CanMove)
         {
            Log.AI(this, "Moving to " + x + "," + y);

            StateMachine.ChangeState(new StateMove(this, x, y));
         }
      } // MoveTo(x, y)

      /// <summary>
      /// Perform attack
      /// </summary>
      internal void PerformAttack(BuildEntity Target)
      {
         int damage = this.MinDamage; //TODO!!! + ScriptEnvironment.Random.Next(this.RandomDamage);
         damage -= Target.ArmorPoints;
         if (damage < 0)
            damage = 0;

         Log.AI(this, "Hitting " + Target.Name + Target.UniqueID + " for " + damage + " point(s) of damage.");

         Target.HitPoints -= (short)damage;
         Target.HateList.SetHateValue(this, damage, HateListParam.AddValue);
      } // PerformAttack(Target)

      internal virtual void DidSpawn()
      {
      }

      public virtual bool CanMove
      {
         get
         {
            return false;
         }
      }
      public virtual bool CanAttack
      {
         get
         {
            return false;
         }
      }
      public virtual bool CanBuild
      {
         get
         {
            return false;
         }
      }
      public virtual bool LookaroundWhileIdle
      {
         get
         {
            return false;
         }
      }

      public static Entity CreateEntityFromType (LevelObjectType entityType, Map inMap)
      {
         switch (entityType) 
         {
         // Orc Units
         case LevelObjectType.Peon:
            return new OrcPeon (inMap);

         case LevelObjectType.Grunt:
            return new OrcGrunt (inMap);

         case LevelObjectType.Spearman:
            return new OrcAxethrower (inMap);

            // Human Units
         case LevelObjectType.Peasant:
            return new HumanPeasant (inMap);

         case LevelObjectType.Warrior:
            return new HumanWarrior (inMap);

         case LevelObjectType.Bowman:
            return new HumanArcher (inMap);

            // Orc Buildings
         case LevelObjectType.Orc_Farm:
            return new OrcFarm (inMap);

         case LevelObjectType.Orc_HQ:
            return new OrcBase (inMap);

            // Human Buildings
         case LevelObjectType.Human_Farm:
            return new HumanFarm (inMap);

         case LevelObjectType.Human_HQ:
            return new HumanBase (inMap);

            // Neutral
         case LevelObjectType.Goldmine:
            return new Goldmine (inMap);

         default:
            return new Entity (inMap);
         }
      }
   }
}

