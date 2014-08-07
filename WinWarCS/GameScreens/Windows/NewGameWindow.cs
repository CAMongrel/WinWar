using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Gui;

namespace WinWarCS.GameScreens.Windows
{
   class NewGameWindow : UIWindow
   {
      internal NewGameWindow ()
      {
         this.Y = 70;

         InitWithUIResource ("Select Game Type");

         // Single Player
         UIButton singlePlayerBtn = Components [1] as UIButton;
         singlePlayerBtn.OnMouseUpInside += singlePlayerBtn_OnMouseUpInside;

         // Modem Game
         UIButton modemGameBtn = Components [2] as UIButton;
         modemGameBtn.OnMouseUpInside += modemGameBtn_OnMouseUpInside;

         // Network Game
         UIButton networkGameBtn = Components [3] as UIButton;
         networkGameBtn.OnMouseUpInside += networkGameBtn_OnMouseUpInside;

         // Direct Link Game
         UIButton directLinkGameBtn = Components [4] as UIButton;
         directLinkGameBtn.OnMouseUpInside += directLinkGameBtn_OnMouseUpInside;

         // Cancel
         UIButton cancelBtn = Components [5] as UIButton;
         cancelBtn.OnMouseUpInside += cancelBtn_OnMouseUpInside;
      }

      async void networkGameBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet");
      }

      async void directLinkGameBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet");
      }

      async void modemGameBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         await WinWarCS.Platform.UI.ShowMessageDialog ("Not implemented yet");
      }

      void singlePlayerBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         new ChooseCampaignWindow ();
         Close ();
      }

      void cancelBtn_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         new MainMenuWindow ();
         Close ();
      }
   }
}
