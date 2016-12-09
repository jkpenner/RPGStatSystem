using UnityEngine.Events;


public partial class EventManager {
    public class RPGEntityEvent : UnityEvent<Entity> { }
    private RPGEntityEvent _onEntityRespawn = new RPGEntityEvent();
    private RPGEntityEvent _onEntityKilled = new RPGEntityEvent();

    /// <summary>
    /// Triggered when an entity has it's respawn method called
    /// </summary>
    public RPGEntityEvent OnEntityRespawn {
        get { return _onEntityRespawn; }
        private set { _onEntityRespawn = value; }
    }

    /// <summary>
    /// Triggered when an entity has it's health reduced to zero
    /// </summary>
    public RPGEntityEvent OnEntityKilled {
        get { return _onEntityKilled; }
        private set { _onEntityKilled = value; }
    }
}
