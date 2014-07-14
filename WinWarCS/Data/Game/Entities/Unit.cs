using System;

namespace WinWarCS.Data.Game
{
   enum Orientation
   {
      North,
      NorthEast,
      East,
      SouthEast,
      South,
      SouthWest,
      West,
      NorthWest,
   }

   internal class Unit : Entity
   {
      internal Orientation Orientation
      {
         get
         { 
            return Sprite.SpriteOrientation;
         }
         set
         { 
            Sprite.SpriteOrientation = value;
         }
      }

      public UnitSprite Sprite
      {
         get
         {
            return base.sprite as UnitSprite;
         }
      }

      public Unit (Map currentMap)
         : base (currentMap)
      {
         AttackRange = 2;
         MaxHitPoints = 20;
         HitPoints = MaxHitPoints;
         MinDamage = 5;
         RandomDamage = 2;
         ArmorPoints = 1;
         AttackSpeed = 1.0f;
         WalkSpeed = 0.7f;
      }

      public override bool CanAttack 
      {
         get 
         {
            return true;
         }
      }
      public override bool CanMove
      {
         get 
         {
            return true;
         }
      }
      public override bool LookaroundWhileIdle 
      {
         get 
         {
            return true;
         }
      }

      public void SetRandomOrientation()
      {
         Orientation = (Orientation)((int)(CurrentMap.Rnd.NextDouble () * 8));
      }

      internal override void DidSpawn ()
      {
         SetRandomOrientation ();
      }

      internal static Orientation OrientationFromDiff(float x, float y)
      {
         if (x < 0 && y < 0)
            return Orientation.NorthWest;
         if (x == 0 && y < 0)
            return Orientation.North;
         if (x > 0 && y < 0)
            return Orientation.NorthEast;

         if (x < 0 && y == 0)
            return Orientation.West;
         if (x > 0 && y == 0)
            return Orientation.East;

         if (x < 0 && y > 0)
            return Orientation.SouthWest;
         if (x == 0 && y > 0)
            return Orientation.South;
         if (x > 0 && y > 0)
            return Orientation.SouthEast;

         return Orientation.North;
      }
   }
}

