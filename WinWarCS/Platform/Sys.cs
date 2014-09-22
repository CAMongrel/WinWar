using System;

namespace WinWarCS.Platform
{
   public static class Sys
   {
      public static void Exit(int exitCode = 0)
      {
#if WINFX_CORE
         App.Current.Exit();
#else
         Environment.Exit(exitCode);
#endif
      }
   }
}

