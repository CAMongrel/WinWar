using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data.Game;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class GameBackgroundWindow : UIWindow
    {
        private UIImage leftSidebar;
        private UIImage leftSidebarTop;
        private UIImage topBar;
        private UIImage bottomBar;
        private UIImage rightBar;

        private UIMapControl mapControl;

        public GameBackgroundWindow()
        {
            mapControl = new UIMapControl();

            InitUI();
        }

        private void LoadUIImage(ref UIImage img, string name)
        {
            if (img != null)
            {
                this.Components.Remove(img);
            }

            img = UIImage.FromImageResource(name);
            Components.Add(img);
        }

        private void InitUI()
        {
            LoadUIImage(ref leftSidebarTop, "Sidebar Left Minimap Black (" + LevelGameScreen.Game.HumanPlayer.Race + ")");
            LoadUIImage(ref leftSidebar, "Sidebar Left (" + LevelGameScreen.Game.HumanPlayer.Race + ")");
            leftSidebar.Y = leftSidebarTop.Height;

            LoadUIImage(ref topBar, "Topbar (" + LevelGameScreen.Game.HumanPlayer.Race + ")");
            LoadUIImage(ref bottomBar, "Lower Bar (" + LevelGameScreen.Game.HumanPlayer.Race + ")");

            topBar.X = leftSidebarTop.Width;
            bottomBar.X = leftSidebar.Width;
            bottomBar.Y = 200 - bottomBar.Height;

            LoadUIImage(ref rightBar, "Sidebar Right (" + LevelGameScreen.Game.HumanPlayer.Race + ")");
            rightBar.X = 320 - rightBar.Width;

            mapControl.X = leftSidebarTop.Width;
            mapControl.Y = topBar.Height;
        }
    }
}
