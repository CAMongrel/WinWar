using System;
using WinWarCS.Data.Game;

// TODO: Make platform specific log writer

namespace WinWarCS.Util
{
   public class Log
   {
      private static string GetTimestamp()
      {
         return "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + "] ";
      }

      internal static void AI(Entity entity, string message)
      {
#if !NETFX_CORE
         Console.WriteLine (GetTimestamp() + "[AI] " + entity + ": " + message);
#endif
      }

      internal static void Status(string message)
      {
#if !NETFX_CORE
         Console.WriteLine (GetTimestamp() + "[Status]: " + message);
#endif
      }

      internal static void Warning(string message)
      {
#if !NETFX_CORE
         Console.WriteLine (GetTimestamp() + "[Warning]: " + message);
#endif
      }

      internal static void Error(string message)
      {
#if !NETFX_CORE
         Console.WriteLine (GetTimestamp() + "[Error]: " + message);
#endif
      }
   }
}

