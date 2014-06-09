using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Game;
using WinWarCS.GameScreens.Windows;
using WinWarCS.Gui;

namespace WinWarCS.GameScreens
{
   class LevelGameScreen : BaseGameScreen
   {
      internal static LevelGameScreen Game { get; private set; }

      internal HumanPlayer HumanPlayer { get; private set; }

      internal bool IsCampaignLevel
      {
         get
         {
            return HumanPlayer.Campaign != null;
         }
      }

      private GameBackgroundWindow backgroundWindow;

      internal LevelGameScreen (HumanPlayer setHumanPlayer)
      {
         Game = this;

         HumanPlayer = setHumanPlayer;
      }

      internal override void InitUI ()
      {
         backgroundWindow = new GameBackgroundWindow ();

         if (IsCampaignLevel) 
         {
            backgroundWindow.MapControl.LoadCampaignLevel (HumanPlayer.Campaign.GetCurrentLevelName());
         }
      }

      internal override void Close ()
      {
         UIWindowManager.Clear ();
      }

      internal override void Draw (Microsoft.Xna.Framework.GameTime gameTime)
      {
         UIWindowManager.Render ();
      }

      internal override void PointerDown (Microsoft.Xna.Framework.Vector2 position)
      {
         UIWindowManager.PointerDown (position);
      }

      internal override void PointerUp (Microsoft.Xna.Framework.Vector2 position)
      {
         UIWindowManager.PointerUp (position);
      }

      internal override void PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         UIWindowManager.PointerMoved (position);
      }
   }
}
