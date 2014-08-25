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

      internal Campaign Campaign { get; private set; }

      internal Race UIRace
      {
         get
         {
            if (Campaign != null)
               return Campaign.Race;

            // TODO: Implement me for custom levels
            return Race.Humans;
         }
      }

      internal bool IsCampaignLevel
      {
         get
         {
            return Campaign != null;
         }
      }

      private GameBackgroundWindow backgroundWindow;

      internal bool GamePaused
      {
         get { return backgroundWindow.GamePaused; }
         set { backgroundWindow.GamePaused = value; }
      }

      internal LevelGameScreen(Campaign setCampaign)
      {
         Game = this;

         Campaign = setCampaign;
      }

      internal override void InitUI()
      {
         MouseCursor.State = MouseCursorState.Pointer;

         backgroundWindow = new GameBackgroundWindow(this);

         if (IsCampaignLevel)
         {
            // Load map
            backgroundWindow.MapControl.LoadCampaignLevel(Campaign.Race, Campaign.Level);
            backgroundWindow.MapControl.CurrentMap.Start();
         }
         else
         {
            throw new NotImplementedException();
         }
      }

      internal override void Close()
      {
         UIWindowManager.Clear();
      }

      internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
      {
         UIWindowManager.Render();
      }

      internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update(gameTime);
      }

      internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         UIWindowManager.PointerDown(position, pointerType);
      }

      internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         UIWindowManager.PointerUp(position, pointerType);
      }

      internal override void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
      {
         UIWindowManager.PointerMoved(position);
      }
   }
}
