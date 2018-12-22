using System;
using WinWarCS;
using WinWarCS.Data;

namespace WinWar
{
    static class Program
    {
        internal static MainGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            IAssetProvider assetProvider = new NetCoreAssetProvider();

            game = new MainGame(assetProvider);
            game.Run();
        }
    }
}