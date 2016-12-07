using System;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Used to indicate when the stat's current value changes
    /// </summary>
    public interface IStatCurrentValueChange {
        void AddCurrentValueListener(CurrentValueEvent func);
        void RemoveCurrentValueListener(CurrentValueEvent func);
    }
}
