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

      public Unit ()
      {
      }
   }
}

