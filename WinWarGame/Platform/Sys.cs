﻿using System;

namespace WinWarGame.Platform
{
   public static class Sys
   {
      public static void Exit(int exitCode = 0)
      {
#if WINFX_CORE
         App.Current.Exit();
#elif __IOS__
         // Not allowed on iOS
#else
         Environment.Exit(exitCode);
#endif
      }
   }
}

