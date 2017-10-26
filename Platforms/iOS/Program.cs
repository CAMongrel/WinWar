using System;
using System.IO;
using Foundation;
using UIKit;
using WinWarCS.Data;
using WinWarCS.iOS;

namespace WinWarCS.Platform
{
   [Register ("AppDelegate")]
   class Program : UIApplicationDelegate
   {
      internal static MainGame game;

      internal static void RunGame ()
      {
         IAssetProvider assetProvider = new IOSAssetProvider ();

         game = new MainGame (assetProvider);
         game.Run ();
      }

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main (string [] args)
      {
         UIApplication.Main (args, null, "AppDelegate");
      }

      public override void FinishedLaunching (UIApplication app)
      {
         RunGame ();
      }
   }
}

