using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Game;
using WinWarCS.Gui;
using WinWarCS.Gui.Input;
using WinWarCS.Data;
using WinWarCS.Util;

namespace WinWarCS.GameScreens.Windows
{
   class GameBackgroundWindow : UIWindow
   {
      private UIImage leftSidebar;
      private UIImage leftSidebarTop;
      private UIImage topBar;
      private UIImage bottomBar;
      private UIImage rightBar;
      private UIButton menuButton;
      private UILabel goldLabel;
      private UILabel goldValueLabel;
      private UILabel lumberLabel;
      private UILabel lumberValueLabel;

      internal UIEntityControl EntityControl { get; private set; }
      internal UIMapControl MapControl { get; private set; }
      internal UIMinimapControl MinimapControl { get; private set; }

      private Vector2 currentPointerPos;
      private Vector2 scrollDelta;
      private float scrollSpeed;

      private LevelGameScreen levelGameScreenOwner;

      internal bool GamePaused { get; set; }

      internal GameBackgroundWindow (LevelGameScreen setLevelGameScreenOwner)
      {
         levelGameScreenOwner = setLevelGameScreenOwner;
         scrollSpeed = 125.0f;
         currentPointerPos = new Vector2 (50, 50);

         InitUI ();
      }

      private void LoadUIImage (ref UIImage img, string name)
      {
         img = UIImage.FromImageResource (name);
         AddComponent (img);
      }

      private void InitUI ()
      {
         ClearComponents ();

         MapControl = new UIMapControl();
         MapControl.OnSelectedEntitiesChanged += HandleOnSelectedEntitiesChanged;
         AddComponent (MapControl);

         EntityControl = new UIEntityControl(LevelGameScreen.Game.UIRace);
         MinimapControl = new UIMinimapControl(MapControl);

         LoadUIImage (ref leftSidebarTop, "Sidebar Left Minimap Black (" + LevelGameScreen.Game.UIRace + ")");
         LoadUIImage (ref leftSidebar, "Sidebar Left (" + LevelGameScreen.Game.UIRace + ")");
         leftSidebar.Y = leftSidebarTop.Height;

         LoadUIImage (ref topBar, "Topbar (" + LevelGameScreen.Game.UIRace + ")");
         LoadUIImage (ref bottomBar, "Lower Bar (" + LevelGameScreen.Game.UIRace + ")");

         topBar.X = leftSidebarTop.Width;
         bottomBar.X = leftSidebar.Width;
         bottomBar.Y = 200 - bottomBar.Height;

         LoadUIImage (ref rightBar, "Sidebar Right (" + LevelGameScreen.Game.UIRace + ")");
         rightBar.X = 320 - rightBar.Width;

         MapControl.X = leftSidebarTop.Width;
         MapControl.Y = topBar.Height;
         MapControl.Width = rightBar.X - MapControl.X;
         MapControl.Height = bottomBar.Y - MapControl.Y;

         MinimapControl.X = 3;
         MinimapControl.Y = 6;
         MinimapControl.Width = 64;
         MinimapControl.Height = 64;
         MinimapControl.BackgroundColor = Color.AliceBlue;
         MinimapControl.Init ();

         menuButton = new UIButton ("", WarFile.KnowledgeBase.IndexByName("Menu Button"), WarFile.KnowledgeBase.IndexByName("Menu Button (Pressed)"));
         menuButton.X = leftSidebar.Width / 2 - menuButton.Width / 2 - 1;
         menuButton.Y = leftSidebarTop.Height + leftSidebar.Height - menuButton.Height - 1;
         menuButton.OnMouseUpInside += menuButton_OnMouseUpInside;
         AddComponent (menuButton);

         EntityControl.X = 2;
         EntityControl.Y = MinimapControl.Y + MinimapControl.Height + 2;
         EntityControl.Width = 64;
         EntityControl.Height = MainGame.OriginalAppHeight - EntityControl.Y - menuButton.Height - 2;

         lumberLabel = new UILabel ("Lumber:");
         lumberLabel.X = 95;
         lumberLabel.Y = 0;
         lumberLabel.Width = 72;
         lumberLabel.Height = 10;
         lumberLabel.TextAlign = TextAlignHorizontal.Left;
         AddComponent (lumberLabel);  

         lumberValueLabel = new UILabel ("1000");
         lumberValueLabel.X = 95;
         lumberValueLabel.Y = 0;
         lumberValueLabel.Width = 72;
         lumberValueLabel.Height = 10;
         lumberValueLabel.TextAlign = TextAlignHorizontal.Right;
         AddComponent (lumberValueLabel);

         goldLabel = new UILabel ("Gold:");
         goldLabel.X = 206;
         goldLabel.Y = 0;
         goldLabel.Width = 60;
         goldLabel.Height = 10;
         goldLabel.TextAlign = TextAlignHorizontal.Left;
         AddComponent (goldLabel);  

         goldValueLabel = new UILabel ("1000");
         goldValueLabel.X = 206;
         goldValueLabel.Y = 0;
         goldValueLabel.Width = 60;
         goldValueLabel.Height = 10;
         goldValueLabel.TextAlign = TextAlignHorizontal.Right;
         AddComponent (goldValueLabel);  

         AddComponent (MinimapControl);
         AddComponent (EntityControl);
      }

      private void HandleOnSelectedEntitiesChanged (object sender, EventArgs e)
      {
         Map map = (Map)sender;

         EntityControl.SetEntities(map.GetSelectedEntities());
      }

      private void menuButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         IngameMenuWindow menu = new IngameMenuWindow (levelGameScreenOwner.UIRace);
      }

      internal void SetGoldValue(int newValue)
      {
         goldValueLabel.Text = newValue.ToString ();
      }

      internal void SetLumberValue(int newValue)
      {
         lumberValueLabel.Text = newValue.ToString ();
      }

      private void UpdateUI()
      {
         SetGoldValue(MapControl.CurrentMap.HumanPlayer.Gold);
         SetLumberValue(MapControl.CurrentMap.HumanPlayer.Lumber);
      }

      internal override void Update (Microsoft.Xna.Framework.GameTime gameTime)
      {
         if (GamePaused)
            return;

         Performance.Push("Game loop - base.Update");
         base.Update(gameTime);
         Performance.Pop();

         Performance.Push("Game loop - UpdateUI");
         UpdateUI();
         Performance.Pop();

         bool leftClickNeeded = false;
         if (MapControl.InputHandler.InputMode == InputMode.Classic)
            // TODO: Use me
            leftClickNeeded = true;

         bool shouldScroll = false;

         MouseCursorState newState = MouseCursorState.None;

         if (currentPointerPos.X <= 3)
         {
            shouldScroll = true;
            scrollDelta.X -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            newState = MouseCursorState.ScrollLeft;
         }
         if (currentPointerPos.Y <= 3)
         {
            shouldScroll = true;
            scrollDelta.Y -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (newState == MouseCursorState.None)
               newState = MouseCursorState.ScrollTop;
            else
               newState = MouseCursorState.ScrollTopleft;
         }
         if (currentPointerPos.X >= Width - 3)
         {
            shouldScroll = true;
            scrollDelta.X += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (newState == MouseCursorState.ScrollTop)
               newState = MouseCursorState.ScrollTopright;
            else
               newState = MouseCursorState.ScrollRight;
         }
         if (currentPointerPos.Y >= Height - 3)
         {
            shouldScroll = true;
            scrollDelta.Y += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (newState == MouseCursorState.ScrollLeft)
               newState = MouseCursorState.ScrollBottomleft;
            else if (newState == MouseCursorState.ScrollRight)
               newState = MouseCursorState.ScrollBottomright;
            else
               newState = MouseCursorState.ScrollBottom;
         }

         if (shouldScroll)
         {
            MouseCursor.State = newState;

            Vector2 clampedScrollDelta = scrollDelta;
            clampedScrollDelta.X = (float)((int)scrollDelta.X / MapControl.TileWidth) * MapControl.TileWidth;
            clampedScrollDelta.Y = (float)((int)scrollDelta.Y / MapControl.TileHeight) * MapControl.TileHeight;

            MapControl.SetCameraOffset (MapControl.CameraTileX * MapControl.TileWidth + clampedScrollDelta.X, 
               MapControl.CameraTileY * MapControl.TileHeight + clampedScrollDelta.Y);

            if (clampedScrollDelta.X != 0)
               scrollDelta.X = 0;
            if (clampedScrollDelta.Y != 0)
               scrollDelta.Y = 0;
         } else
         {
            scrollDelta.X = 0;
            scrollDelta.Y = 0;
         }
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return base.PointerDown (position, pointerType);
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         currentPointerPos = position;

         MouseCursor.State = MouseCursorState.Pointer;

         return base.PointerMoved (position);
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         return base.PointerUp (position, pointerType);
      }
   }
}
