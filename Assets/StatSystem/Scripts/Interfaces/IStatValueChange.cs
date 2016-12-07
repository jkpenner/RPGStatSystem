using System;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Used to indicate when the stat's value changes
    /// </summary>
    public interface IStatValueChange {
        void AddValueListener(RPGStatEvent stat);
        void RemoveValueListener(RPGStatEvent stat);
    }
}
