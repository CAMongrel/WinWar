using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class ChooseCampaignWindow : Window
    {
        public ChooseCampaignWindow()
        {
            InitWithTextResource("Choose Campaign");

            // Orc Campaign
            Button orcBtn = Components[1] as Button;
            orcBtn.OnMouseUpInside += orcBtn_OnMouseUpInside;

            // Human Campaign
            Button humanBtn = Components[2] as Button;
            humanBtn.OnMouseUpInside += humanBtn_OnMouseUpInside;

            // Custom Game
            Button customGameBtn = Components[3] as Button;
            customGameBtn.OnMouseUpInside += customGameBtn_OnMouseUpInside;

            // Cancel
            Button cancelBtn = Components[4] as Button;
            cancelBtn.OnMouseUpInside += cancelBtn_OnMouseUpInside;
        }

        void orcBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            
        }

        void humanBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            
        }

        async void customGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MessageDialog dlg = new MessageDialog("Not implemented yet");
            await dlg.ShowAsync();
        }

        private void cancelBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            MenuGameScreen.Menu.SetCurrentWidow(new NewGameWindow());
        }
    }
}
