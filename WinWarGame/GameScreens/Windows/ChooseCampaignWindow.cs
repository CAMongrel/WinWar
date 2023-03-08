using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Game;
using WinWarGame.Gui;

namespace WinWarGame.GameScreens.Windows
{
    class ChooseCampaignWindow : UIWindow
    {
        internal ChooseCampaignWindow()
        {
            this.Y = 70;

            InitWithUIResource("Choose Campaign");

            // Orc Campaign
            UIButton orcBtn = Components[1] as UIButton;
            orcBtn.OnMouseUpInside += orcBtn_OnMouseUpInside;

            // Human Campaign
            UIButton humanBtn = Components[2] as UIButton;
            humanBtn.OnMouseUpInside += humanBtn_OnMouseUpInside;

            // Custom Game
            UIButton customGameBtn = Components[3] as UIButton;
            customGameBtn.OnMouseUpInside += customGameBtn_OnMouseUpInside;

            // Cancel
            UIButton cancelBtn = Components[4] as UIButton;
            cancelBtn.OnMouseUpInside += cancelBtn_OnMouseUpInside;
        }

        private void StartNewCampaign(Race setRace)
        {
            Campaign campaign = new Campaign(setRace);
            campaign.StartNew();

            MainGame.WinWarGame.SetNextGameScreen(new LevelGameScreen(campaign));
        }

        private void orcBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            StartNewCampaign(Race.Orcs);
        }

        private void humanBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            StartNewCampaign(Race.Humans);
        }

        private void customGameBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            Platform.UI.ShowMessageDialog("Not implemented yet");
        }

        private void cancelBtn_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            new NewGameWindow();
            Close();
        }
    }
}
