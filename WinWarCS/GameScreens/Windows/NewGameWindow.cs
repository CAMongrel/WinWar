using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class NewGameWindow : UIWindow
    {
        internal NewGameWindow()
        {
            InitWithTextResource("Select Game Type");

            // Single Player
            UIButton singlePlayerBtn = Components[1] as UIButton;
            singlePlayerBtn.OnMouseUpInside += singlePlayerBtn_OnMouseUpInside;

            // Modem Game
            UIButton modemGameBtn = Components[2] as UIButton;
            modemGameBtn.OnMouseUpInside += modemGameBtn_OnMouseUpInside;

            // Network Game
            UIButton networkGameBtn = Components[3] as UIButton;
            networkGameBtn.OnMouseUpInside += networkGameBtn_OnMouseUpInside;

            // Direct Link Game
            UIButton directLinkGameBtn = Components[4] as UIButton;
            directLinkGameBtn.OnMouseUpInside += directLinkGameBtn_OnMouseUpInside;

            // Cancel
            UIButton cancelBtn = Components[5] as UIButton;
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
            new ChooseCampaignWindow();
            Close();
        }

        void cancelBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            new MainMenuWindow();
            Close();
        }
    }
}
