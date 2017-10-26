using MonoMac.AppKit;
using MonoMac.Foundation;
using WinWarCS.Data;
using WinWarCS.MacOS;

namespace WinWarCS.Platform
{
   [MonoMac.Foundation.Register ("AppDelegate")]
   class AppDelegate : NSApplicationDelegate
   {
      public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
      {
         Program.RunGame();
      }

      public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
      {
         return true;
      }
   }

   static class Program
   {
      internal static MainGame game;

      internal static void RunGame ()
      {
         IAssetProvider assetProvider = new MacOSAssetProvider ();

         game = new MainGame(assetProvider);
         game.Run ();
      }

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main (string [] args)
      {
         NSApplication.Init ();

         using (var p = new NSAutoreleasePool ()) 
         {
            NSApplication.SharedApplication.Delegate = new AppDelegate ();

            NSApplication.Main (args);
         }
      }
   }
}
