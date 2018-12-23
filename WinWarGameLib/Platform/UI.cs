using System;
using System.Threading.Tasks;
using WinWarCS.Gui;
#if __IOS__
using UIKit;
#elif OSX
using MonoMac.AppKit;
#endif

namespace WinWarCS.Platform
{
    public static class UI
    {
        public static bool ShowMessageDialog(string message, Action closeAction = null)
        {
            Console.WriteLine("ShowMessageDialog: " + message);

            UIWindow result = new UIWindow();
            UILabel label = new UILabel(message);
            label.TextAlign = TextAlignHorizontal.Center;
            result.AddComponent(label);
            UIButton btn = new UIButton("Ok", 0, 0);
            btn.OnMouseUpInside += (loc) =>
            {
                result.Close();
                MainGame.WinWarGame.SystemGameScreen.IsActive = false;
                closeAction?.Invoke();
            };
            result.AddComponent(btn);

            MainGame.WinWarGame.SystemGameScreen.IsActive = true;

            return true;
        }
    }
}

