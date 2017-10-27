using System.Threading;
using NUnit.Framework;
using WinWarCS.Util;

namespace DataWarLibTests
{
   [TestFixture ()]
   public class PerformanceTests
   {
      [Test ()]
      public void TestNotEnabled ()
      {
         Performance.Enabled = false;

         Performance.Push ("Test");

         // Wait five milliseconds
         Thread.Sleep (5);

         double result = Performance.Pop ();

         Assert.IsTrue (result == 0.0);
      }

      [Test ()]
      public void TestEnabled ()
      {
         Performance.Enabled = true;

         Performance.Push ("Test");

         // Wait five milliseconds
         Thread.Sleep (5);

         double result = Performance.Pop ();

         // Due to inexact timing, we must use an epsilon test
         Assert.AreEqual(0.005, result, 0.003);
      }
   }
}
