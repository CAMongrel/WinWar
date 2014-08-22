using System;
using WinWarCS.Data.Game;

// TODO: Make platform specific log writer

namespace WinWarCS.Util
{
   [Flags]
   public enum LogType
   {
      None = 0,
      Generic = 1,
      Resources = 2,
      AI = 4,
      Performance = 8,
      Game = Generic | AI,
      All = Generic | Resources | AI | Performance,
   }

   public enum LogSeverity
   {
      Fatal,
      Error,
      Warning,
      Status,
      Debug,
   }

   public static class Log
   {
      public static LogType Type = LogType.Game;
      public static LogSeverity Severity = LogSeverity.Debug;

      private static string GetTimestamp()
      {
         return "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + "] ";
      }

      private static void LogInternal(LogType type, LogSeverity severity, string message)
      {
#if !NETFX_CORE
         if (((int)severity <= (int)Severity) && ((Type & type) == type))
            Console.WriteLine(GetTimestamp() + "[" + severity + "] [" + type + "]: " + message);
#endif
      }

      internal static void Write(LogType type, LogSeverity severity, string message)
      {
         LogInternal(type, severity, message);
      }

      internal static void Status(string message)
      {
         LogInternal(LogType.Generic, LogSeverity.Status, message);
      }

      internal static void Warning(string message)
      {
         LogInternal(LogType.Generic, LogSeverity.Warning, message);
      }

      internal static void Error(string message)
      {
         LogInternal(LogType.Generic, LogSeverity.Error, message);
      }

      // Specialized logging methods
      internal static void AI(Entity entity, string message, LogSeverity severity = LogSeverity.Debug)
      {
         LogInternal(LogType.AI, severity, entity + ": " + message);
      }
   }
}

