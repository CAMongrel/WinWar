namespace WinWarGame.Data.Game.Definitions;

public class LevelScript
{
    /// <summary>
    /// Called after the level is fully initialized (all players loaded and initial
    /// entities are spawned) and right before the first tick. 
    /// </summary>
    /// <param name="map"></param>
    public virtual void OnLevelInitialized(IScriptMap map)
    {
        //
    }
    
    /// <summary>
    /// Called every tick.
    ///
    /// Don't do any heavy-load logic here
    /// </summary>
    public virtual void UpdateLevel(double deltaTime)
    {
        //
    }
    
    /// <summary>
    /// Invoked after the entity was spawned into the level
    /// </summary>
    public virtual void OnEntitySpawned(IScriptEntity entity)
    {
        //
    }
    
    public virtual void OnEntityDestroyed(IScriptEntity entity)
    {
        
    }

    public virtual void OnEntityDespawned(IScriptEntity entity)
    {
        
    }
}