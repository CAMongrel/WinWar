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

        private UIButton menuButton;

        public UIMapControl MapControl { get; private set; }
        public UIMinimapControl MinimapControl { get; private set; }

        public GameBackgroundWindow()
        {
            InitUI();
        }

        private void LoadUIImage(ref UIImage img, string name)
        {
            img = UIImage.FromImageResource(name);
            AddComponent(img);
        }

        private void InitUI()
        {
            ClearComponents();

            MapControl = new UIMapControl();
            AddComponent(MapControl);

            MinimapControl = new UIMinimapControl(MapControl);

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

            MapControl.X = leftSidebarTop.Width;
            MapControl.Y = topBar.Height;
            MapControl.Width = rightBar.X - MapControl.X;
            MapControl.Height = bottomBar.Y - MapControl.Y;

            MinimapControl.X = 3;
            MinimapControl.Y = 6;
            MinimapControl.Width = 64;
            MinimapControl.Height = 64;
            MinimapControl.Init();

            menuButton = new UIButton("Menu", UIButton.ButtonType.SmallButton);
            menuButton.Width = (int)((float)menuButton.Width * 1.22f);
            menuButton.Height = (int)((float)menuButton.Height / 1.3f);
            menuButton.X = leftSidebar.Width / 2 - menuButton.Width / 2 - 1;
            menuButton.Y = leftSidebarTop.Height + leftSidebar.Height - menuButton.Height - 1;
            menuButton.OnMouseUpInside += menuButton_OnMouseUpInside;
            AddComponent(menuButton);

            AddComponent(MinimapControl);
        }

        void menuButton_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            IngameMenuWindow menu = new IngameMenuWindow();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return base.PointerDown(position);
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return base.PointerMoved(position);
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return base.PointerUp(position);
        }
    }
}
