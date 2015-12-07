using System;
using System.Threading.Tasks;
#if __IOS__
using UIKit;
#elif OSX
using MonoMac.AppKit;
#endif

namespace WinWarCS.Platform
{
   public static class UI
   {
      public static async Task<bool> ShowMessageDialog(string message)
      {
#if OSX
         NSAlert alert = NSAlert.WithMessage (message, "Ok", null, null, "");
         alert.RunModal ();
#elif WINFX_CORE
         Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog(message, "WinWarCS - WarCraft for Windows Modern UI");
         await dlg.ShowAsync();
#elif __IOS__
         UIAlertView alertView = new UIAlertView ("WinWar", message, null, "Ok", null);
         alertView.Show ();
#else
         Console.WriteLine("ShowMessageDialog: " + message);
         System.Windows.Forms.MessageBox.Show(message, "WinWarCS");
#endif
         return true;
      }
   }
}

