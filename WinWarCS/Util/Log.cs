using System;
using WinWarCS.Data.Game;

namespace WinWarCS.Util
{
   public class Log
   {
      internal static void AI(Entity entity, string message)
      {
         Console.WriteLine ("[AI] " + entity + ": " + message);
      }

      internal static void Status(string message)
      {
         Console.WriteLine ("[Status]: " + message);
      }

      internal static void Warning(string message)
      {
         Console.WriteLine ("[Warning]: " + message);
      }

      internal static void Error(string message)
      {
         Console.WriteLine ("[Error]: " + message);
      }
   }
}

