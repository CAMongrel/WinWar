using System;
using System.Threading.Tasks;

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
#else
         Console.WriteLine("ShowMessageDialog: " + message);
         System.Windows.Forms.MessageBox.Show(message, "WinWarCS");
#endif
         return true;
      }
   }
}

