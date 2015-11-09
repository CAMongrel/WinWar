#if !WINFX_CORE
using System;
using WinWarCS;
#if OSX
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif

namespace WinWarCS.Platform
{
#if OSX
   [MonoMac.Foundation.Register ("AppDelegate")]
   class AppDelegate : NSApplicationDelegate
   {
      public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
      {
         Program.game = new MainGame ();
         //Program.game.IsMouseVisible = true;
         Program.game.Run ();
      }

      public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
      {
         return true;
      }
   }
#endif

	static class Program
	{
      internal static MainGame game;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
#if OSX
         NSApplication.Init ();

         using (var p = new NSAutoreleasePool ()) 
         {
            NSApplication.SharedApplication.Delegate = new AppDelegate ();

            NSApplication.Main (args);
         }
#else
			game = new MainGame ();
			game.Run ();
#endif
		}
	}
}

#endif // !WINFX_CORE