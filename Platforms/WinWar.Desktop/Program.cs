using WinWar.Desktop.Windows;
using WinWarCS;

var assetProvider = new NetAssetProvider();

using var game = new MainGame(assetProvider);
game.Run();
