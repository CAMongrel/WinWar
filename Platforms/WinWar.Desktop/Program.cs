using WinWar.Desktop;
using WinWarGame;

var assetProvider = new NetAssetProvider();

using var game = new MainGame(assetProvider);
game.Run();
