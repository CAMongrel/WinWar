using System;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class Entity
   {
      protected Sprite sprite;

      public float X { get; private set; }
      public float Y { get; private set; }

      public BasePlayer Owner { get; private set; }

      public Entity ()
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Peasant")));
      }

      public void SetPosition(float newX, float newY)
      {
         X = newX;
         Y = newY;
      }

      /// <summary>
      /// Assigns the owner.
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

         SpriteFrame curFrame = sprite.CurrentFrame;

         System.Drawing.RectangleF rect = new System.Drawing.RectangleF ();
         rect.X = offsetX + (X - startTileX) * TileWidth - (TileWidth / 2);
         rect.Y = offsetY + (Y - startTileY) * TileHeight - (TileHeight / 2);
         rect.Width = sprite.MaxWidth;
         rect.Height = sprite.MaxHeight;

         curFrame.texture.RenderOnScreen (rect);
      }

      public static Entity CreateEntityFromType (LevelObjectType entityType)
      {
         switch (entityType) 
         {
         case LevelObjectType.Grunt:
            return new OrcGrunt ();

         case LevelObjectType.Peon:
            return new OrcPeon ();

         case LevelObjectType.Warrior:
            return new HumanWarrior ();

         case LevelObjectType.Bowman:
            return new HumanArcher ();

         case LevelObjectType.Orc_Farm:
            return new OrcFarm ();

         case LevelObjectType.Orc_HQ:
            return new OrcBase ();

         case LevelObjectType.Goldmine:
            return new Goldmine ();

         default:
            return new Entity ();
         }
      }
   }
}

