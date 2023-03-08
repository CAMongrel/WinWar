using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WinWarGame.Graphics;
using WinWarGame.Data;
using WinWarGame.Data.Game;
using WinWarGame.Data.Resources;
using WinWarGame.GameScreens;
using WinWarGame.Util;

namespace WinWarGame.Gui
{
   internal class UIEntityControl : UIBaseComponent
   {
      private Entity[] currentEntities;
      private Dictionary<Entity, UIImage> healthBars;
      private Race race;
      private bool isInSubmenu;

      internal UIEntityControl(Race setRace)
      {
         isInSubmenu = false;
         race = setRace;

         healthBars = new Dictionary<Entity, UIImage>();

         RebuildUI();
      }

      private void SetHealthbar(UIImage img, Entity ent)
      {
         img.Width = (int)(((float)ent.HitPoints / (float)ent.MaxHitPoints) * 27.0f);
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

         UILabel nameLabel = new UILabel(currentEntities[0].Name);
         nameLabel.TextAlign = TextAlignHorizontal.Left;
         nameLabel.X = 4;
         nameLabel.Y = unitIcon.Y + unitIcon.Height + 2;
         unitFrame.AddComponent(nameLabel);

         UIImage img = new UIImage(null);
         img.X = 35;
         img.Y = 20;
         img.Height = 3;
         img.BackgroundColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
         SetHealthbar(img, ent);
         unitFrame.AddComponent(img);

         healthBars.Add(ent, img);
      }

      private void ShowMultiUI()
      {
         // Fail-safe
         if (currentEntities.Length <= 1 || currentEntities.Length > 4)
            return;

         SpriteResource res = WarFile.GetSpriteResource(race == Race.Humans ? 360 : 359);

      }

      private void ShowBuildSubMenu()
      {
         // Remove all old components
         ClearComponents();

         isInSubmenu = true;

         healthBars.Clear();

         // No entities? Then just exit
         if (currentEntities == null || currentEntities.Length == 0)
            return;

         if (currentEntities.Length == 1)
            ShowSingleUI();
         else
            ShowMultiUI();

         int buttonIndex = 0;
         if (ShowCancel)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Cancel");
            btn.OnMouseUpInside += (position) => { RebuildUI(); };
            SetButtonPosition(btn, 5);
            AddComponent(btn);

            buttonIndex++;
         }
      }

      private void RebuildUI()
      {
         // Remove all old components
         ClearComponents();

         isInSubmenu = false;

         healthBars.Clear();

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
            UISpriteButton moveBtn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Move" + race);
            moveBtn.OnMouseUpInside += (position) => { SetMapUnitOrder(MapUnitOrder.Move); };
            SetButtonPosition(moveBtn, buttonIndex);
            AddComponent(moveBtn);

            buttonIndex++;
         }
         if (ShowStop)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Shield1" + (race == Race.Humans ? "" : "Orcs"));
            btn.OnMouseUpInside += (position) => { Stop(); };
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowAttack)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Sword1");
            btn.OnMouseUpInside += (position) => { SetMapUnitOrder(MapUnitOrder.Attack); };
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowRepair)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Repair");
            btn.OnMouseUpInside += (position) => { SetMapUnitOrder(MapUnitOrder.Repair); };
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowHarvest)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Harvest");
            btn.OnMouseUpInside += (position) => { SetMapUnitOrder(MapUnitOrder.Harvest); };
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowBuild)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Build");
            btn.OnMouseUpInside += (position) => { ShowBuildSubMenu(); };
            SetButtonPosition(btn, buttonIndex);
            AddComponent(btn);

            buttonIndex++;
         }
         if (ShowCancel)
         {
            UISpriteButton btn = new UISpriteButton(new Sprite(WarFile.GetSpriteResource(361)), "Cancel");
            btn.OnMouseUpInside += (position) => { RebuildUI(); };
            SetButtonPosition(btn, 6);
            AddComponent(btn);

            buttonIndex++;
         }

         if (currentEntities.Length == 1)
            currentEntities[0].AddCustomUI(this);
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

         // Update healthbars
         for (int i = 0; i < currentEntities.Length; i++)
         {
            if (healthBars.ContainsKey(currentEntities[i]) == false)
            {
               Log.Warning("Trying to update healthbar for entity '" + currentEntities[i] + "', but there's no matching healthbar!");
               continue;
            }

            UIImage hpBar = healthBars[currentEntities[i]];
            SetHealthbar(hpBar, currentEntities[i]);
         }
      }

      #region Order helpers
      private void Stop()
      {
         if (currentEntities == null)
            return;

         for (int i = 0; i < currentEntities.Length; i++)
         {
            currentEntities[i].Idle();
         }
      }

      private void SetMapUnitOrder(MapUnitOrder order)
      {
         if (LevelGameScreen.Game != null)
         {
            LevelGameScreen.Game.SetMapUnitOrder(order);
         }
      }
      #endregion

      #region Properties
      private bool ShowCancel
      {
         get
         {
            if (isInSubmenu)
               return true;

            return false;
         }
      }
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
      private bool ShowHarvest
      {
         get
         {
            if (currentEntities == null)
               return false;

            for (int i = 0; i < currentEntities.Length; i++)
            {
               if (currentEntities[i].CanHarvest == false)
                  return false;
            }

            return true;
         }
      }
      private bool ShowBuild
      {
         get
         {
            if (currentEntities == null || currentEntities.Length > 1)
               return false;

            if (currentEntities[0].CanBuild == false)
               return false;

            return true;
         }
      }
      private bool ShowRepair
      {
         get
         {
            if (currentEntities == null)
               return false;

            for (int i = 0; i < currentEntities.Length; i++)
            {
               if (currentEntities[i].CanRepair == false)
                  return false;
            }

            return true;
         }
      }
      private bool ShowStop
      {
         get
         {
            if (currentEntities == null)
               return false;

            for (int i = 0; i < currentEntities.Length; i++)
            {
               if (currentEntities[i].CanStop == false)
                  return false;
            }

            return true;
         }
      }
      #endregion
   }
}

