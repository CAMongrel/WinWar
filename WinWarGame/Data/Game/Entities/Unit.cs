using System;
using WinWarGame.Util;
using WinWarGame.Data.Resources;

namespace WinWarGame.Data.Game
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
            get => Sprite.SpriteOrientation;
            set => Sprite.SpriteOrientation = value;
        }

        public UnitSprite Sprite => base.sprite as UnitSprite;

        public Unit(Map currentMap)
            : base(currentMap)
        {
        }

        public override bool CanAttack => !IsDead && ((MinDamage + RandomDamage) > 0) && AttackSpeed > 0.0;

        public override bool CanMove => !IsDead;

        public override bool LookaroundWhileIdle => !IsDead;

        public override bool AllowsMultiSelection => true;

        public override bool CanStop => !IsDead;

        public void SetRandomOrientation()
        {
            Orientation = (Orientation)((int)(CurrentMap.Rnd.NextDouble() * 8));
        }

        internal override void DidSpawn()
        {
            SetRandomOrientation();
        }

        internal override void DestroyAndSpawnRemains()
        {
            base.DestroyAndSpawnRemains();

            CurrentMap?.CreateEntity(this.TileX, this.TileY, LevelObjectType.Orc_corpse, null);
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