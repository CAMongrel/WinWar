using System;

namespace WinWarCS.Util
{
   public class DebugOptions
   {
      public static bool ShowTiles;
      public static bool ShowUnitFrames;
      public static bool ShowBlockedTiles;
      public static bool ShowFullMapOnLoad;
      public static bool LogLongRunningTasksOnly;
      public static double LongRunningTaskThresholdInSeconds;

      static DebugOptions()
      {
         ShowTiles = false;
         ShowUnitFrames = false;
         ShowBlockedTiles = false;
         ShowFullMapOnLoad = true;

         // Long running tasks (Performance)
         LogLongRunningTasksOnly = true;
         LongRunningTaskThresholdInSeconds = 0.2;
      }
   }
}

