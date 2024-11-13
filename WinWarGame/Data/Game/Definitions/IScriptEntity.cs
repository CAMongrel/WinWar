namespace WinWarGame.Data.Game.Definitions;

public interface IScriptEntity
{
    int TileX { get; }
    int TileY { get; }
    
    void Attack(IScriptEntity target);
    
    void MoveTo(int x, int y);

    void AttackMove(int x, int y);
}