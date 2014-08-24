using System;
using System.Threading.Tasks;

namespace WinWarCS.Platform
{
   public static class UI
   {
      public static async Task<bool> ShowMessageDialog(string message)
      {
         Console.WriteLine("ShowMessageDialog: " + message);
         System.Windows.Forms.MessageBox.Show(message, "WinWarCS");
         return true;
      }
   }
}

