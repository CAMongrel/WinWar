using System;

namespace WinWarGame.Data.Game.Definitions;

public class ScriptIdentifierAttribute : Attribute
{
    public string Name { get; init; }
}