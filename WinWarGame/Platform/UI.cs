using System;
using System.Threading.Tasks;
using WinWarGame.Gui;

#if __IOS__
using UIKit;
#elif OSX
using MonoMac.AppKit;
#endif

namespace WinWarGame.Platform
{
    public static class UI
    {
        public static bool ShowMessageDialog(string message, string okButtonText = "Ok",
            Action closeAction = null)
        {
            Console.WriteLine("ShowMessageDialog: " + message);

            UIWindow result = new UIWindow();
            result.Width = MainGame.OriginalAppWidth;
            result.Height = MainGame.OriginalAppHeight;

            UIButton btn = new UIButton(okButtonText, -1, -1);
            btn.AutoSizeToText();
            btn.Y = result.Height - btn.Height;
            btn.OnMouseUpInside += (loc) =>
            {
                result.Close();
                MainGame.WinWarGame.SetSystemGameScreenActive(false);
                closeAction?.Invoke();
            };
            result.AddComponent(btn);

            btn.CenterXInParent();

            UILabel label = new UILabel(message);
            label.IsUnformattedText = true;
            label.TextAlign = TextAlignHorizontal.Center;
            label.Width = result.Width;
            label.Height = result.Height - btn.Height;
            result.AddComponent(label);

            MainGame.WinWarGame.SetSystemGameScreenActive(true);

            return true;
        }
    }
}

