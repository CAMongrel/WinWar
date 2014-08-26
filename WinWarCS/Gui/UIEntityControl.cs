using System;
using WinWarCS.Data.Game;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

namespace WinWarCS.Gui
{
   internal class UIEntityControl : UIBaseComponent
   {
      private Entity[] currentEntities;
      private Race race;

      internal UIEntityControl(Race setRace)
      {
         race = setRace;

         RebuildUI();
      }

      private void ShowSingleUI()
      {
         // Fail-safe
         if (currentEntities.Length != 1)
            return;

         SpriteResource res = WarFile.GetSpriteResource(race == Race.Humans ? 360 : 359);
         SpriteResource resUnit = WarFile.GetSpriteResource(361);

         Entity ent = currentEntities[0];

         UISpriteImage unitFrame = new UISpriteImage(new Sprite(res));
         unitFrame.FixedSpriteFrame = ent.Mana > 0 ? 1 : 0;
         unitFrame.X = 0;
         unitFrame.Y = 0;
         AddComponent(unitFrame);

         UISpriteImage unitIcon = new UISpriteImage(new Sprite(resUnit));
         unitIcon.FixedSpriteFrame = ent.IconIndex;
         unitIcon.X = 4;
         unitIcon.Y = 4;
         unitFrame.AddComponent(unitIcon);
      }

      private void ShowMultiUI()
      {
         // Fail-safe
         if (currentEntities.Length <= 1 || currentEntities.Length > 4)
            return;

         SpriteResource res = WarFile.GetSpriteResource(race == Race.Humans ? 360 : 359);

      }

      private void RebuildUI()
      {
         // Remove all old components
         ClearComponents();

         // No entities? Then just exit
         if (currentEntities == null || currentEntities.Length == 0)
            return;

         if (currentEntities.Length == 1)
            ShowSingleUI();
         else
            ShowMultiUI();

         int buttonIndex = 0;

         if (ShowMove)
         {
            UISpriteButton moveBtn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 1);
            SetButtonPosition(moveBtn, buttonIndex);
            AddComponent(moveBtn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 2);
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 2);
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 2);
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 2);
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), 2);
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
      }

      internal void SetEntities(Entity[] newEntities)
      {
         currentEntities = newEntities;
         RebuildUI();
      }

      private void SetButtonPosition(UIBaseComponent comp, int buttonIndex)
      {
         int col = buttonIndex % 2;
         int row = buttonIndex / 2;

         comp.X = 1 + col * 32;
         comp.Y = 35 + row * 24;
      }

      internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update(gameTime);

         if (currentEntities == null)
            return;
      }

      #region Properties
      private bool ShowMove
      {
         get
         {
            if (currentEntities == null)
               return false;

            for (int i = 0; i < currentEntities.Length; i++)
            {
               if (currentEntities[i].CanMove == false)
                  return false;
            }

            return true;
         }
      }
      private bool ShowAttack
      {
         get
         {
            if (currentEntities == null)
               return false;

            for (int i = 0; i < currentEntities.Length; i++)
            {
               if (currentEntities[i].CanAttack == false)
                  return false;
            }

            return true;
         }
      }
      #endregion
   }
}

