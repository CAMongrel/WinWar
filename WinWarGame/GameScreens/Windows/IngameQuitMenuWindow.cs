using System;
using WinWarGame.Data;
using WinWarGame.Data.Game;
using WinWarGame.Data.Resources;
using WinWarGame.Graphics;
using WinWarGame.Gui;
using WinWarGame.Platform;

namespace WinWarGame.GameScreens.Windows
{
   internal class IngameQuitMenuWindow : UIWindow
   {
      private UIImage background;
      private UIButton quitButton;
      private UIButton menuButton;
      private UIButton cancelButton;

      internal IngameQuitMenuWindow(Race setRace)
      {
         UIResource res = WarFile.GetUIResource(setRace == Race.Humans ? 391 : 392);

         background = new UIImage(WWTexture.FromImageResource(WarFile.GetImageResource(res.BackgroundImageResourceIndex)));
         background.InitWithUIResource (res);
         AddComponent (background);

         background.CenterOnScreen();

         cancelButton = (UIButton)background.Components [3];
         cancelButton.OnMouseUpInside += cancelButton_OnMouseUpInside;

         menuButton = (UIButton)background.Components [2];
         menuButton.OnMouseUpInside += menuButton_OnMouseUpInside;

         quitButton = (UIButton)background.Components [1];
         quitButton.OnMouseUpInside += quitButton_OnMouseUpInside;

         MouseCursor.State = MouseCursorState.Pointer;
      }

      void menuButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         // TODO: End game

         MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(false));
      }

      void quitButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         Sys.Exit ();
      }

      void cancelButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         Close ();
      }

   }
}

