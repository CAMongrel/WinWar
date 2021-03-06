﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;
using WinWarCS.Data.Game;
using WinWarCS.Data.Resources;
using WinWarCS.Gui;
using System.IO;

namespace WinWarCS.GameScreens.Windows
{
   class MainMenuWindow : UIWindow
   {
      internal MainMenuWindow()
      {
         this.Y = 70;

         InitWithUIResource("Main Menu Text");

         // Start new game
         UIButton newGameBtn = Components[0] as UIButton;
         newGameBtn.OnMouseUpInside += newGameBtn_OnMouseUpInside;

         // Load existing game
         UIButton loadGameBtn = Components[1] as UIButton;
         loadGameBtn.OnMouseUpInside += loadGameBtn_OnMouseUpInside;

         // Replay introduction
         UIButton replayIntroBtn = Components[2] as UIButton;
         replayIntroBtn.OnMouseUpInside += replayIntroBtn_OnMouseUpInside;

         // Quit Game
         UIButton quitGameBtn = Components[3] as UIButton;
         quitGameBtn.OnMouseUpInside += quitGameBtn_OnMouseUpInside;        

         /*string unitName = "Goldmine";

         SpriteResource res = WarFile.GetSpriteResource(KnowledgeBase.IndexByName(unitName));
         res.CreateImageData (false, false, false);
         Sprite spr = new Sprite (res);
         spr.CurrentFrame.WriteToFile ("/Users/henningthole/temp/paltest/mine_kb.png");

         for (int i = 0; i < WarFile.Count; i++) 
         {
            if (KnowledgeBase.KB_List [i].type == WarFileType.FilePalette) 
            {
               res = new SpriteResource (WarFile.GetResourceByName (unitName), WarFile.GetResource (i));
               res.CreateImageData (false, false, false);
               spr = new Sprite (res);
               spr.CurrentFrame.WriteToFile ("/Users/henningthole/temp/paltest/mine_pal_" + i + ".png");
            }
         }*/
            
         /*UISpriteImage sprImg = new UISpriteImage(new UnitSprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName(unitName))));
         //sprImg.Sprite.SetCurrentAnimationByName ("Walk");
         sprImg.X = 10;
         sprImg.Y = 10;
         AddComponent(sprImg);*/

         /*for (int i = 0; i <= (int)Orientation.NorthWest; i++) 
         {
            UISpriteImage sprImg = new UISpriteImage(new UnitSprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName("Orc Grunt"))));
            sprImg.Sprite.SetCurrentAnimationByName ("Walk");
            sprImg.Orientation = (Orientation)i;
            sprImg.X = 30 + 30 * i;
            sprImg.Y = 10;
            AddComponent(sprImg);
         }*/
      }

      async void loadGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet!");
      }

      async void replayIntroBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         if (WarFile.IsDemo)
         {
            await WinWarCS.Platform.UI.ShowMessageDialog ("No idea ... ask Blizzard");
            return;
         }

         Stream resultFile = await MainGame.AssetProvider.OpenContentFile(Path.Combine("Assets" + MainGame.AssetProvider.DirectorySeparatorChar + "Data", "TITLE.WAR"));

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
