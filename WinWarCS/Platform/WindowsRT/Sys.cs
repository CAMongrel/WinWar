﻿using System;

namespace WinWarCS.Platform
{
   public static class Sys
   {
      public static void Exit(int exitCode = 0)
      {
         App.Current.Exit();
      }
   }
}
