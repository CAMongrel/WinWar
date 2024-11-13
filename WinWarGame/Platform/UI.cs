using System;
using System.Threading.Tasks;
using WinWarGame.Data;
using WinWarGame.Data.Resources;
using WinWarGame.Graphics;
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
        /// <summary>
        /// Ugly looking message box (needs 9patch support)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="okButtonText"></param>
        /// <param name="closeAction"></param>
        /// <returns></returns>
        public static bool ShowMessageDialog(string message, string okButtonText = "Ok",
            Action closeAction = null)
        {
            Console.WriteLine("ShowMessageDialog: " + message);

            int padding = 2;

            UIWindow result = new UIWindow();
            result.Width = MainGame.OriginalAppWidth;
            result.Height = MainGame.OriginalAppHeight;

            // TODO: Needs 9patch support to look good
            UIImage backgroundImage = new UIImage(WWTexture.FromImageResource((ImageResource)WarFile.GetResource(248)));
            backgroundImage.Width = (int)(result.Width * 0.9f);
            backgroundImage.Height = (int)(result.Height * 0.9f);
            result.AddComponent(backgroundImage);
            backgroundImage.CenterInParent();

            UIButton btn = new UIButton(okButtonText, 239, 240);
            btn.Y = backgroundImage.Height - btn.Height - padding * 3;
            btn.OnMouseUpInside += (loc) =>
            {
                result.Close();
                MainGame.WinWarGame.SetSystemGameScreenActive(false);
                closeAction?.Invoke();
            };
            backgroundImage.AddComponent(btn);

            btn.CenterXInParent();

            UILabel label = new UILabel(message);
            label.IsUnformattedText = true;
            label.TextAlign = TextAlignHorizontal.Center;
            label.Width = backgroundImage.Width;
            label.Height = backgroundImage.Height - btn.Height - padding * 4;
            backgroundImage.AddComponent(label);

            MainGame.WinWarGame.SetSystemGameScreenActive(true);

            return true;
        }
    }
}

