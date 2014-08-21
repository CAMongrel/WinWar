using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Graphics;
using WinWarCS.Gui;
using WinWarCS.Data.Game;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

namespace WinWarCS.GameScreens.Windows
{
   class IngameMenuWindow : UIWindow
   {
      private UIImage background;
      private UIButton continueButton;
      private UIButton quitButton;
      private Race curRace;

      internal IngameMenuWindow (Race setRace)
      {
         curRace = setRace;
         LevelGameScreen.Game.GamePaused = true;

         BackgroundColor = new Microsoft.Xna.Framework.Color (0.3f, 0.0f, 0.0f, 0.5f);

         //Width = background.Width;
         //Height = background.Height;

         UIResource res = WarFile.GetUIResource(setRace == Race.Humans ? 368 : 369);

         background = new UIImage(WWTexture.FromImageResource(WarFile.GetImageResource(res.BackgroundImageResourceIndex)));
         background.InitWithUIResource (res);
         AddComponent (background);

         background.X = 120;
         background.Y = 20;

         continueButton = (UIButton)background.Components [6];
         continueButton.OnMouseUpInside += closeButton_OnMouseUpInside;

         quitButton = (UIButton)background.Components [5];
         quitButton.OnMouseUpInside += quitButton_OnMouseUpInside;

         MouseCursor.State = MouseCursorState.Pointer;
      }

      internal override void DidRemove ()
      {
         base.DidRemove ();

         LevelGameScreen.Game.GamePaused = false;
      }

      void quitButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         new IngameQuitMenuWindow(curRace);
      }

      void closeButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         Close ();
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         MouseCursor.State = MouseCursorState.Pointer;

         return base.PointerMoved (position);
      }
   }
}
