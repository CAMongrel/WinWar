using System;
using WinWarCS.Windows;
using WinWarGame.Data;

namespace WinWarCS
{
   static class Program
	{
      internal static MainGame game;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
      [STAThread]
      static void Main (string[] args)
		{
         IAssetProvider assetProvider = new WinDesktopAssetProvider();

         game = new MainGame(assetProvider);
         game.Run();
      }
   }
}