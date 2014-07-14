using System;
using WinWarCS.Data.Game;

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
         Console.WriteLine (GetTimestamp() + "[AI] " + entity + ": " + message);
      }

      internal static void Status(string message)
      {
         Console.WriteLine (GetTimestamp() + "[Status]: " + message);
      }

      internal static void Warning(string message)
      {
         Console.WriteLine (GetTimestamp() + "[Warning]: " + message);
      }

      internal static void Error(string message)
      {
         Console.WriteLine (GetTimestamp() + "[Error]: " + message);
      }
   }
}

