using System;
using WinWarCS;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace WinWarCS.MacOS
{
	[MonoMac.Foundation.Register ("AppDelegate")]
	class AppDelegate : NSApplicationDelegate
	{
		public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
		{
			Program.game = new MainGame ();
			Program.game.Run ();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}

	static class Program
	{
		internal static MainGame game;

		static void Main (string[] args)
		{
			NSApplication.Init ();

			using (var p = new NSAutoreleasePool ()) 
			{
				NSApplication.SharedApplication.Delegate = new AppDelegate ();

				NSApplication.Main (args);
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/*[STAThread]
		static void Main ()
		{
			NSApplication.Init ();

			NSApplicationDelegate del = NSApplication.SharedApplication.Delegate;

			//NSApplication.SharedApplication.InvokeOnMainThread (() => {
				game = new MainGame ();
				game.Run ();
			//});
		}*/
	}
}

