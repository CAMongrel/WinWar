using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Game;
using WinWarGame.Data.Resources;
using System.IO;
using WinWarGame.Data;
using WinWarGame.Gui;
using WinWarGame.Platform;

namespace WinWarGame.GameScreens.Windows
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
        }

        private void loadGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            Platform.UI.ShowMessageDialog("Not implemented yet!");
        }

        private void replayIntroBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            if (WarFile.IsDemo)
            {
                Platform.UI.ShowMessageDialog("No idea ... ask Blizzard");
                return;
            }

            Stream resultFile = MainGame.AssetProvider.OpenGameDataFile("TITLE.WAR");

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
            Sys.Exit();
        }
    }
}
