using System;
using System.Collections.Generic;
using WinWarCS.Data.Resources;
using Microsoft.Xna.Framework;
using WinWarCS.Util;
using WinWarCS.Graphics;
using System.IO;
using System.Xml;
using System.Globalization;
using WinWarCS.Gui;

#if NETFX_CORE
using RectangleF = WinWarCS.Platform.WindowsRT.RectangleF;
#else
using RectangleF = System.Drawing.RectangleF;
#endif

namespace WinWarCS.Data.Game
{
    internal class Entity
    {
        private static Dictionary<LevelObjectType, Dictionary<string, string>> defaultValueDict;

        protected Sprite sprite;

        public float X { get; private set; }
        public float Y { get; private set; }

        public int TileX
        {
            get
            {
                return (int)X;// / (float)CurrentMap.TileWidth); 
            }
        }
        public int TileY
        {
            get
            {
                return (int)Y; // / (float)CurrentMap.TileHeight); 
            }
        }
        public virtual int TileSizeX
        {
            get
            {
                return 1;
            }
        }
        public virtual int TileSizeY
        {
            get
            {
                return 1;
            }
        }

        public BasePlayer Owner { get; private set; }

        public HateList HateList { get; private set; }

        public StateMachine StateMachine { get; private set; }

        public Map CurrentMap { get; private set; }

        private Entity currentTarget;

        public Entity PreviousTarget { get; private set; }

        internal LevelObjectType Type { get; private set; }

        public string Name
        {
            get
            {
                return ToString();
            }
        }

        public int UniqueID { get; set; }

        /// <summary>
        /// Attack range
        /// </summary>
        public double AttackRange;
        /// <summary>
        /// The attack speed.
        /// </summary>
        public double AttackSpeed;
        /// <summary>
        /// The visible range.
        /// </summary>
        public double VisibleRange;
        /// <summary>
        /// The visible range.
        /// </summary>
        public double AggroRange;
        /// <summary>
        /// The walking speed.
        /// </summary>
        public double WalkSpeed;
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
        /// Hit points
        /// </summary>
        public short Mana;
        /// <summary>
        /// Maximum hit points
        /// </summary>
        public short MaxMana;
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

        public int IconIndex;

        public Entity(Map currentMap)
        {
            Performance.Push("Entity ctor");
            VisibleRange = 0;

            IconIndex = 0;

            currentTarget = null;
            PreviousTarget = null;

            CurrentMap = currentMap;

            Performance.Push("Create base sprite");
            sprite = new Sprite(WarFile.GetSpriteResource(WarFile.KnowledgeBase.IndexByName("Human Peasant")));
            Performance.Pop();

            Performance.Push("new HateList");
            HateList = new HateList();
            Performance.Pop();

            Performance.Push("new StateMachine");
            StateMachine = new StateMachine(this);
            Performance.Pop();

            Performance.Push("StateMachine.ChangeState");
            StateMachine.ChangeState(new StateIdle(this));
            Performance.Pop();

            Performance.Pop();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="newX">New x.</param>
        /// <param name="newY">New y.</param>
        public void SetPosition(float newX, float newY)
        {
            Log.AI(this.ToString(), "Setting position to " + newX + "," + newY);

            CurrentMap.Pathfinder.SetFieldsFree(TileX, TileY, this.TileSizeX, this.TileSizeY);

            X = newX;
            Y = newY;

            CurrentMap.Pathfinder.SetFieldsBlocked(TileX, TileY, this.TileSizeX, this.TileSizeY);
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
        /// Returns a rectangle encompassing (in screen coordinates) the bounding box of the entity
        /// </summary>
        internal RectangleF GetTileRectangle(float offsetX, float offsetY, float tileOffsetX, float tileOffsetY)
        {
            int startTileX = ((int)tileOffsetX / CurrentMap.TileWidth);
            int startTileY = ((int)tileOffsetY / CurrentMap.TileHeight);

            RectangleF rect = new RectangleF();
            rect.X = offsetX + (X - startTileX) * CurrentMap.TileWidth;// - (TileWidth / 2);
            rect.Y = offsetY + (Y - startTileY) * CurrentMap.TileHeight;// - (TileHeight / 2);
            rect.Width = TileSizeX * CurrentMap.TileWidth;
            rect.Height = TileSizeY * CurrentMap.TileHeight;
            return rect;
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
        public void Render(float offsetX, float offsetY, float tileOffsetX, float tileOffsetY)
        {
            if (sprite == null || CurrentMap == null)
                return;

            bool shouldFlipX = sprite.ShouldFlipX;

            RectangleF rect = GetTileRectangle(offsetX, offsetY, tileOffsetX, tileOffsetY);
            rect.Width = sprite.MaxWidth;
            rect.Height = sprite.MaxHeight;

            RectangleF tileRectangle = new RectangleF(0, 0, TileSizeX * CurrentMap.TileWidth, TileSizeY * CurrentMap.TileHeight);

            rect.X -= (rect.Width * 0.5f - tileRectangle.Width * 0.5f);
            rect.Y -= (rect.Height * 0.5f - tileRectangle.Height * 0.5f);

            if (sprite.CurrentFrame != null)
                sprite.CurrentFrame.texture.RenderOnScreen(rect, shouldFlipX, false);

            if (DebugOptions.ShowUnitFrames)
                WWTexture.RenderRectangle(rect, Color.Blue);
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
                if (pl == this.Owner || pl.IsNeutralTowards(this.Owner))
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

        internal virtual void AddCustomUI(UIBaseComponent parentComponent)
        {
            //
        }

        /// <summary>
        /// Update the specified gameTime.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        internal void Update(GameTime gameTime)
        {
            if (StateMachine != null)
            {
                StateMachine.Update(gameTime);
            }

            if (sprite != null)
                sprite.Update(gameTime);
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
            Log.AI(this.ToString(), "Idling...");
            StateMachine.ChangeState(new StateIdle(this));
        } // Idle()

        /// <summary>
        /// Orders the unit to die. This will spawn a corpse at the current tile and prevents any further action.
        /// </summary>
        public void Die()
        {
            Log.AI(this.ToString(), "Dieing...");
            StateMachine.ChangeState(new StateDeath(this));
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
                Log.AI(this.ToString(), "Moving to " + x + "," + y + " and attacking all enemies on the way!");

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
                Log.AI(this.ToString(), "Attacking " + Target.Name + Target.UniqueID);

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
                Log.AI(this.ToString(), "Moving to " + x + "," + y);

                StateMachine.ChangeState(new StateMove(this, x, y));
            }
        } // MoveTo(x, y)

        // 
        internal void TakeDamage(short damage, Entity instigator)
        {
            damage -= this.ArmorPoints;
            if (damage < 0)
                damage = 0;

            HitPoints -= (short)damage;
            Log.AI(this.ToString(), this.Name + this.UniqueID + " takes " + damage + " point(s) of damage. (reduced by armor and effects)");
            Log.AI(this.ToString(), this.Name + this.UniqueID + " has " + this.HitPoints + " hitpoints left.");
            if (HitPoints <= 0)
                Die();

            if (instigator != null)
                HateList.SetHateValue(instigator, damage, HateListParam.AddValue);
        }

        /// <summary>
        /// Perform attack
        /// </summary>
        internal bool PerformAttack(Entity Target)
        {
            if (Target.ShouldBeAttacked == false)
                return false;

            int damage = this.MinDamage + CurrentMap.Rnd.Next(this.RandomDamage);

            Log.AI(this.ToString(), "Hitting " + Target.Name + Target.UniqueID + " for " + damage + " (unmitigated) point(s) of damage.");

            Target.TakeDamage((short)damage, this);

            return true;
        } // PerformAttack(Target)

        internal virtual void DidSpawn()
        {
        }

        internal virtual void DidSelect()
        {
        }

        internal virtual bool WillSelect()
        {
            return CanBeSelected;
        }

        internal virtual void DidDeselect()
        {
        }

        internal virtual bool WillDeselect()
        {
            return true;
        }

        internal virtual void DestroyAndSpawnRemains()
        {
            if (CurrentMap != null)
                CurrentMap.RemoveEntity(this);
        }

        internal bool CanGiveCommands()
        {
            if (Owner == null)
                return false;

            if (Owner == CurrentMap.HumanPlayer)
                return true;

            return false;
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
        public virtual bool CanHarvest
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
        public virtual bool CanRepair
        {
            get
            {
                return false;
            }
        }
        public virtual bool CanStop
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
        public virtual bool CanBeSelected
        {
            get
            {
                return true;
            }
        }
        public virtual bool AllowsMultiSelection
        {
            get
            {
                return false;
            }
        }
        public virtual bool IsDead
        {
            get
            {
                return (StateMachine.CurrentState is StateDeath);
            }
        }
        public virtual bool ShouldBeAttacked
        {
            get
            {
                if (IsDead)
                    return false;

                return true;
            }
        }

        internal bool IsNeutralTowards(BasePlayer player)
        {
            if (this.Owner == null)
                return true;

            return this.Owner.IsNeutralTowards(player);
        }
        internal bool IsHostileTowards(BasePlayer player)
        {
            if (this.Owner == null)
                return true;

            return this.Owner.IsHostileTowards(player);
        }
        internal bool IsFriendlyTowards(BasePlayer player)
        {
            if (this.Owner == null)
                return true;

            return this.Owner.IsFriendlyTowards(player);
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public static void LoadDefaultValues(IAssetProvider assetProvider)
        {
            XmlDocument doc = new XmlDocument();
            using (FileStream stream = assetProvider.OpenAssetFile("entities.xml"))
            {
                doc.Load(stream);
            }

            defaultValueDict = new Dictionary<LevelObjectType, Dictionary<string, string>>();

            XmlNodeList list = doc.DocumentElement.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode node = list[i];
                Dictionary<string, string> entityValues = new Dictionary<string, string>();
                for (int j = 0; j < node.ChildNodes.Count; j++)
                {
                    string name = node.ChildNodes[j].Name;
                    string value = node.ChildNodes[j].InnerText;
                    entityValues.Add(name, value);
                }
                defaultValueDict.Add((LevelObjectType)int.Parse(node.Attributes["type"].InnerText), entityValues);
            }
        }

        private static void ApplyDefaultValues(Entity entity, LevelObjectType entityType)
        {
            if (defaultValueDict.ContainsKey(entityType) == false)
            {
                Log.Warning("Unable to apply default values for entityType '" + entityType + "'. EntityType not found in database.");
                return;
            }

            Dictionary<string, string> values = defaultValueDict[entityType];

            entity.ArmorPoints = short.Parse(values["ArmorPoints"], CultureInfo.InvariantCulture);
            entity.AttackRange = double.Parse(values["AttackRange"], CultureInfo.InvariantCulture);
            entity.AttackSpeed = double.Parse(values["AttackSpeed"], CultureInfo.InvariantCulture);
            entity.AggroRange = double.Parse(values["AggroRange"], CultureInfo.InvariantCulture);
            entity.DecayRate = short.Parse(values["DecayRate"], CultureInfo.InvariantCulture);
            entity.GoldCost = short.Parse(values["GoldCost"], CultureInfo.InvariantCulture);
            entity.IconIndex = int.Parse(values["IconIndex"], CultureInfo.InvariantCulture);
            entity.LumberCost = short.Parse(values["LumberCost"], CultureInfo.InvariantCulture);
            entity.MaxHitPoints = short.Parse(values["MaxHitPoints"], CultureInfo.InvariantCulture);
            entity.MaxMana = short.Parse(values["MaxMana"], CultureInfo.InvariantCulture);
            entity.MinDamage = byte.Parse(values["MinDamage"], CultureInfo.InvariantCulture);
            entity.RandomDamage = byte.Parse(values["RandomDamage"], CultureInfo.InvariantCulture);
            entity.TimeToBuild = short.Parse(values["TimeToBuild"], CultureInfo.InvariantCulture);
            entity.VisibleRange = double.Parse(values["VisibleRange"], CultureInfo.InvariantCulture);
            entity.WalkSpeed = double.Parse(values["WalkSpeed"], CultureInfo.InvariantCulture);

            entity.HitPoints = entity.MaxHitPoints;
            entity.Mana = entity.MaxMana;
        }

        public static Entity CreateEntityFromType(LevelObjectType entityType, Map inMap)
        {
            Performance.Push("CreateEntityFromType");
            Entity result = null;

            switch (entityType)
            {
                // Orc Units
                case LevelObjectType.Peon:
                    result = new OrcPeon(inMap);
                    break;

                case LevelObjectType.Grunt:
                    result = new OrcGrunt(inMap);
                    break;

                case LevelObjectType.Spearman:
                    result = new OrcSpearman(inMap);
                    break;

                case LevelObjectType.CatapultOrcs:
                    result = new OrcCatapult(inMap);
                    break;

                case LevelObjectType.Raider:
                    result = new OrcRaider(inMap);
                    break;

                case LevelObjectType.Necrolyte:
                    result = new OrcNecrolyte(inMap);
                    break;

                case LevelObjectType.Warlock:
                    result = new OrcWarlock(inMap);
                    break;

                // Human Units
                case LevelObjectType.Peasant:
                    result = new HumanPeasant(inMap);
                    break;

                case LevelObjectType.Footman:
                    result = new HumanFootman(inMap);
                    break;

                case LevelObjectType.Archer:
                    result = new HumanArcher(inMap);
                    break;

                case LevelObjectType.CatapultHumans:
                    result = new HumanCatapult(inMap);
                    break;

                case LevelObjectType.Conjurer:
                    result = new HumanConjurer(inMap);
                    break;

                case LevelObjectType.Knight:
                    result = new HumanKnight(inMap);
                    break;

                case LevelObjectType.Cleric:
                    result = new HumanCleric(inMap);
                    break;

                // Orc Buildings
                case LevelObjectType.FarmOrc:
                    result = new OrcFarm(inMap);
                    break;

                case LevelObjectType.TownhallOrcs:
                    result = new OrcTownhall(inMap);
                    break;

                // Human Buildings
                case LevelObjectType.FarmHumans:
                    result = new HumanFarm(inMap);
                    break;

                case LevelObjectType.TownhallHumans:
                    result = new HumanTownhall(inMap);
                    break;

                case LevelObjectType.BarracksHumans:
                    result = new HumanBarracks(inMap);
                    break;

                case LevelObjectType.BlacksmithHumans:
                    result = new HumanBlacksmith(inMap);
                    break;

                case LevelObjectType.Church:
                    result = new HumanChurch(inMap);
                    break;

                case LevelObjectType.LumbermillHumans:
                    result = new HumanBarracks(inMap);
                    break;

                case LevelObjectType.Stables:
                    result = new HumanStables(inMap);
                    break;

                case LevelObjectType.TowerHumans:
                    result = new HumanTower(inMap);
                    break;

                case LevelObjectType.Stormwind:
                    result = new HumanStormwind(inMap);
                    break;

                // Neutral
                case LevelObjectType.Goldmine:
                    result = new Goldmine(inMap);
                    break;

                case LevelObjectType.Orc_corpse:
                    result = new Corpse(inMap);
                    break;

                default:
                    result = new Entity(inMap);
                    break;
            }

            result.Type = entityType;

            ApplyDefaultValues(result, entityType);

            Performance.Pop();
            return result;
        }
    }
}

