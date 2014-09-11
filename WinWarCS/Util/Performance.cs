using System;
using System.Collections.Generic;

namespace WinWarCS.Util
{
   public static class Performance
   {
      private struct PerfEntry
      {
         public long StartTime;
         public string Description;
      }

      public static bool Enabled;

      private static Stack<PerfEntry> counterStack;

      static Performance()
      {
         Enabled = true;
         counterStack = new Stack<PerfEntry>();
      }

      public static void Push(string description)
      {
         if (Enabled == false)
            return;

         PerfEntry entry = new PerfEntry();
         entry.StartTime = System.Diagnostics.Stopwatch.GetTimestamp();
         entry.Description = description;
         counterStack.Push(entry);

         if (DebugOptions.LogLongRunningTasksOnly == false)
            Log.Write(LogType.Performance, LogSeverity.Debug, entry.Description + " starts");
      }

      public static double Pop()
      {
         if (Enabled == false)
            return 0;

         if (counterStack.Count == 0)
            return 0;

         PerfEntry entry = counterStack.Pop();

         long diff = System.Diagnostics.Stopwatch.GetTimestamp() - entry.StartTime;
         double seconds = (double)diff / (double)System.Diagnostics.Stopwatch.Frequency;

         bool performLog = true;
         if (DebugOptions.LogLongRunningTasksOnly == true)
         {
            performLog = (seconds >= DebugOptions.LongRunningTaskThresholdInSeconds);
         }

         if (performLog)
            Log.Write(LogType.Performance, LogSeverity.Debug, entry.Description + " took " + (seconds * 1000) + " ms");

         return seconds;
      }
   }
}

