using System;

namespace WinWarGame.Data.Game.Definitions;

public class CampaignDefinition
{
    public LevelDefinition[] Levels { get; set; } = Array.Empty<LevelDefinition>();
}