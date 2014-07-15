using System;
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

         /*sprImg = new UISpriteImage(new UnitSprite(WarFile.GetSpriteResource(KnowledgeBase.IndexByName(unitName))));
         sprImg.Sprite.SetCurrentAnimationByName ("Attack");
         sprImg.X = 30;
         sprImg.Y = 10;
         AddComponent(sprImg);*/

         //sprImg.Sprite.DumpToDirectory ("/data/Temp/WinWar/Garana/", "Garana");
      }

      async void loadGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet!");
      }

      async void replayIntroBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
      {
         if (WarFile.IsDemo)
         {
            await WinWarCS.Platform.UI.ShowMessageDialog ("Not available in demo!");
            return;
         }

         Stream resultFile = await WinWarCS.Platform.IO.OpenContentFile(Path.Combine("Assets" + Platform.IO.DirectorySeparatorChar + "Data", "TITLE.WAR"));

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
