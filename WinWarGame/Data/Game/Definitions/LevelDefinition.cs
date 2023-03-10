using WinWarGame.Data.Game.Definitions.Scripts;

namespace WinWarGame.Data.Game.Definitions;

#nullable enable

public abstract class LevelDefinition
{
    public string Identifier { get; set; } = "not set";

    public string Name { get; set; } = "not set";

    public bool IsCampaign { get; set; } = false;

    public string ScriptName { get; set; } = string.Empty;

    public LevelScript Script { get; set; } = new DefaultMeleeScript();

    public void LoadScript()
    {
        if (string.IsNullOrWhiteSpace(ScriptName))
        {
            Script = new DefaultMeleeScript();
        }
        else
        {
            // TODO
        }
    }
}
