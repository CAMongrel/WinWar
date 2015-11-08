using System;

namespace WinWarCS.Util
{
   public class DebugOptions
   {
      public static bool ShowTiles;
      public static bool ShowUnitFrames;
      public static bool ShowBlockedTiles;
      public static bool ShowFullMapOnLoad;

      static DebugOptions()
      {
         ShowTiles = false;
         ShowUnitFrames = false;
         ShowBlockedTiles = false;
         ShowFullMapOnLoad = false;
      }
   }
}

