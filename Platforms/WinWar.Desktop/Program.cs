using System;
using WinWar.Desktop;
using WinWarGame;

var cmdParams = WinWarGame.Platform.CommandLine.ParseKeyValuePairs(Environment.CommandLine);

var assetProvider = new NetAssetProvider();

using var game = new MainGame(assetProvider, cmdParams);
game.Run();
