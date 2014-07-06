using System;
using WinWarCS.Data.Resources;
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

      public BasePlayer Owner { get; private set; }

      public HateList HateList { get; private set; }

      public Map CurrentMap { get; private set; }

      private Entity currentTarget;

      public Entity PreviousTarget { get; private set; }

      /// <summary>
      /// Attack range
      /// </summary>
      public byte AttackRange;
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
      }

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

