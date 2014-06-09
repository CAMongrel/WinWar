using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Game;
using WinWarCS.Gui;

namespace WinWarCS.GameScreens.Windows
{
   class ChooseCampaignWindow : UIWindow
   {
      internal ChooseCampaignWindow ()
      {
         InitWithTextResource ("Choose Campaign");

         // Orc Campaign
         UIButton orcBtn = Components [1] as UIButton;
         orcBtn.OnMouseUpInside += orcBtn_OnMouseUpInside;

         // Human Campaign
         UIButton humanBtn = Components [2] as UIButton;
         humanBtn.OnMouseUpInside += humanBtn_OnMouseUpInside;

         // Custom Game
         UIButton customGameBtn = Components [3] as UIButton;
         customGameBtn.OnMouseUpInside += customGameBtn_OnMouseUpInside;

         // Cancel
         UIButton cancelBtn = Components [4] as UIButton;
         cancelBtn.OnMouseUpInside += cancelBtn_OnMouseUpInside;
      }

      void orcBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         HumanPlayer newPlayer = new HumanPlayer ();
         newPlayer.Race = Race.Orcs;
         newPlayer.StartNewCampaign ();

         MainGame.WinWarGame.SetNextGameScreen (new LevelGameScreen (newPlayer));
      }

      void humanBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         HumanPlayer newPlayer = new HumanPlayer ();
         newPlayer.Race = Race.Humans;
         newPlayer.StartNewCampaign ();

         MainGame.WinWarGame.SetNextGameScreen (new LevelGameScreen (newPlayer));
      }

      async void customGameBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet");
      }

      private void cancelBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         new NewGameWindow ();
         Close ();
      }
   }
}
