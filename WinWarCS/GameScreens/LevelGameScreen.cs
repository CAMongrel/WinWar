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

      internal List<BasePlayer> Players;

      internal bool IsCampaignLevel
      {
         get
         {
            return HumanPlayer.Campaign != null;
         }
      }

      private GameBackgroundWindow backgroundWindow;

      internal bool GamePaused
      {
         get { return backgroundWindow.GamePaused; }
         set { backgroundWindow.GamePaused = value; }
      }

      internal LevelGameScreen (HumanPlayer setHumanPlayer)
      {
         Game = this;

         HumanPlayer = setHumanPlayer;

         Players = new List<BasePlayer> ();
         Players.Add (HumanPlayer);
      }

      internal override void InitUI ()
      {
         MouseCursor.State = MouseCursorState.Pointer;

         backgroundWindow = new GameBackgroundWindow (this);

         if (IsCampaignLevel) 
         {
            // Create AI player
            BasePlayer ai = new AIPlayer ();
            ai.Race = Race.Humans;
            if (HumanPlayer.Race == Race.Humans)
               ai.Race = Race.Orcs;
            ai.Name = ai.Race.ToString ();
            Players.Add (ai);

            // Load map
            backgroundWindow.MapControl.LoadCampaignLevel(HumanPlayer.Race, HumanPlayer.Campaign.Level);
            backgroundWindow.MapControl.CurrentMap.Start(Players);
         } 
         else 
         {
            throw new NotImplementedException ();
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

      internal override void Update (Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update (gameTime);

         backgroundWindow.SetGoldValue (HumanPlayer.Gold);
         backgroundWindow.SetLumberValue (HumanPlayer.Lumber);
      }

      internal override void PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         UIWindowManager.PointerDown (position, pointerType);
      }

      internal override void PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         UIWindowManager.PointerUp (position, pointerType);
      }

      internal override void PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         UIWindowManager.PointerMoved (position);
      }
   }
}
