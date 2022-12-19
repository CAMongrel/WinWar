using WinWar.Desktop.Windows;
using WinWarCS;

var assetProvider = new WinDesktopAssetProvider();

using var game = new MainGame(assetProvider);
game.Run();
