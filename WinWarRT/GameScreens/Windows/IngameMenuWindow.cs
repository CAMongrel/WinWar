using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Graphics;
using WinWarRT.Gui;

namespace WinWarRT.GameScreens.Windows
{
    class IngameMenuWindow : UIWindow
    {
        UIImage background;

        UIButton closeButton;
        UIButton quitButton;

        internal IngameMenuWindow()
        {
            background = new UIImage(WWTexture.FromImageResource("Window Border"));
            Width = background.Width;
            Height = background.Height;
            AddComponent(background);

            closeButton = new UIButton("Return to Game", UIButton.ButtonType.SmallButton);
            closeButton.OnMouseUpInside += closeButton_OnMouseUpInside;
            AddComponent(closeButton);

            quitButton = new UIButton("Quit to DOS", UIButton.ButtonType.SmallButton);
            quitButton.OnMouseUpInside += quitButton_OnMouseUpInside;
            AddComponent(quitButton);

            closeButton.CenterXInParent();
            closeButton.Y = 4;

            quitButton.CenterXInParent();
            quitButton.Y = quitButton.ParentComponent.Height - 4 - quitButton.Height;

            CenterOnScreen();
        }

        void quitButton_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            App.Current.Exit();
        }

        void closeButton_OnMouseUpInside(Microsoft.Xna.Framework.Vector2 position)
        {
            Close();
        }
    }
}
