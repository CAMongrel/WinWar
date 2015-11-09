#if !WINFX_CORE
using System;
using WinWarCS;
#if OSX
using MonoMac.AppKit;
using MonoMac.Foundation;
#elif __IOS__
using Foundation;
using UIKit;
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

#if __IOS__
   [Register("AppDelegate")]
   class Program : UIApplicationDelegate
#else
   static class Program
#endif
	{
      internal static MainGame game;

      internal static void RunGame ()
      {
         game = new MainGame ();
         game.Run ();
      }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
#if !MONOMAC && !__IOS__     
      [STAThread]
#endif
      static void Main (string[] args)
		{
#if OSX
         NSApplication.Init ();

         using (var p = new NSAutoreleasePool ()) 
         {
            NSApplication.SharedApplication.Delegate = new AppDelegate ();

            NSApplication.Main (args);
         }
#elif __IOS__
         UIApplication.Main(args, null, "AppDelegate");
#else
         RunGame();
#endif
		}

#if __IOS__
      public override void FinishedLaunching(UIApplication app)
      {
         RunGame();
      }
#endif
	}
}

#endif // !WINFX_CORE