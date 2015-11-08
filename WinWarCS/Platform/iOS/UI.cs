using System;
using System.Threading.Tasks;
using UIKit;

namespace WinWarCS.Platform
{
   public static class UI
   {
      public static async Task<bool> ShowMessageDialog(string message)
      {
			UIAlertView alertView = new UIAlertView ("WinWar", message, null, "Ok", null);
			alertView.Show ();
         return true;
      }
   }
}

