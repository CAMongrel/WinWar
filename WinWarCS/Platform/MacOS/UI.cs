using System;
using System.Threading.Tasks;
using MonoMac.AppKit;

namespace WinWarCS.Platform
{
   public static class UI
   {
      public static async Task<bool> ShowMessageDialog(string message)
      {
         NSAlert alert = NSAlert.WithMessage (message, "Ok", null, null, "");
         alert.RunModal ();
         return true;
      }
   }
}

