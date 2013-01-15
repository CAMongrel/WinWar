using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitWithTextResource("Main Menu Text");

            // The order in the text resource is wrong, so switch first and second button
            BaseComponent comp1 = Components[0];
            Components[0] = Components[1];
            Components[1] = comp1;

            // Also switch actual screen position
            int y = Components[0].Y;
            Components[0].Y = Components[1].Y;
            Components[1].Y = y;

            // Start new game
            Button newGameBtn = Components[0] as Button;
            newGameBtn.OnMouseUpInside += newGameBtn_OnMouseUpInside;

            // Replay introduction
            Button replayIntroBtn = Components[1] as Button;
            replayIntroBtn.OnMouseUpInside += replayIntroBtn_OnMouseUpInside;

            // Load existing game
            Button loadGameBtn = Components[2] as Button;
            loadGameBtn.OnMouseUpInside += loadGameBtn_OnMouseUpInside;

            // Quic Game
            Button quitGameBtn = Components[3] as Button;
            quitGameBtn.OnMouseUpInside += quitGameBtn_OnMouseUpInside;
        }

        async void loadGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        async void replayIntroBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        void newGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MenuGameScreen.Menu.SetCurrentWidow(new NewGameWindow());
        }

        void quitGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            App.Current.Exit();
        }
    }
}
