using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class NewGameWindow : Window
    {
        public NewGameWindow()
        {
            InitWithTextResource("Select Game Type");

            // Single Player
            Button singlePlayerBtn = Components[1] as Button;
            singlePlayerBtn.OnMouseUpInside += singlePlayerBtn_OnMouseUpInside;

            // Modem Game
            Button modemGameBtn = Components[2] as Button;
            modemGameBtn.OnMouseUpInside += modemGameBtn_OnMouseUpInside;

            // Network Game
            Button networkGameBtn = Components[3] as Button;
            networkGameBtn.OnMouseUpInside += networkGameBtn_OnMouseUpInside;

            // Direct Link Game
            Button directLinkGameBtn = Components[4] as Button;
            directLinkGameBtn.OnMouseUpInside += directLinkGameBtn_OnMouseUpInside;

            // Cancel
            Button cancelBtn = Components[5] as Button;
            cancelBtn.OnMouseUpInside += cancelBtn_OnMouseUpInside;
        }

        async void networkGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        async void directLinkGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        async void modemGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        void singlePlayerBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MenuGameScreen.Menu.SetCurrentWidow(new ChooseCampaignWindow());
        }

        void cancelBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MenuGameScreen.Menu.SetCurrentWidow(new MainMenuWindow());
        }
    }
}
