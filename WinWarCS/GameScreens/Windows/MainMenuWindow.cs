﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data;
using WinWarRT.Data.Game;
using WinWarRT.Data.Resources;
using WinWarRT.Gui;
using System.IO;

namespace WinWarRT.GameScreens.Windows
{
   class MainMenuWindow : UIWindow
   {
      internal MainMenuWindow()
      {
         InitWithTextResource("Main Menu Text");

         // The order in the text resource is wrong, so switch first and second button
         UIBaseComponent comp1 = Components[0];
         Components[0] = Components[1];
         Components[1] = comp1;

         // Also switch actual screen position
         int y = Components[0].Y;
         Components[0].Y = Components[1].Y;
         Components[1].Y = y;

         // Start new game
         UIButton newGameBtn = Components[0] as UIButton;
         newGameBtn.OnMouseUpInside += newGameBtn_OnMouseUpInside;

         // Replay introduction
         UIButton replayIntroBtn = Components[1] as UIButton;
         replayIntroBtn.OnMouseUpInside += replayIntroBtn_OnMouseUpInside;

         // Load existing game
         UIButton loadGameBtn = Components[2] as UIButton;
         loadGameBtn.OnMouseUpInside += loadGameBtn_OnMouseUpInside;

         // Quic Game
         UIButton quitGameBtn = Components[3] as UIButton;
         quitGameBtn.OnMouseUpInside += quitGameBtn_OnMouseUpInside;

         UISpriteImage sprImg = new UISpriteImage(new UnitSprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName("Orc Grunt"))));
         AddComponent(sprImg);
      }

      async void loadGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet!");
      }

      async void replayIntroBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         Stream resultFile = WinWarCS.Platform.IO.OpenContentFile(Path.Combine("Assets" + Path.DirectorySeparatorChar + "Data", "TITLE.WAR"));

         MovieGameScreen.PlayMovie(resultFile,
             delegate
             {
                if (MenuGameScreen.Menu == null)
                   MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(true));
                else
                {
                   MenuGameScreen.Menu.ResetFade();
                   MainGame.WinWarGame.SetNextGameScreen(MenuGameScreen.Menu);
                }
             });
      }

      void newGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         new NewGameWindow();
         Close();
      }

      void quitGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         WinWarCS.Platform.Sys.Exit ();
      }
   }
}