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
   }
}

