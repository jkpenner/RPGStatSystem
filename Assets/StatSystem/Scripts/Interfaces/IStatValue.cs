using System;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Delegate that passes a IStatValueChange when activated
    /// </summary>
    public delegate void StatValueChangeEvent(IStatValue iStatValue);

    /// <summary>
    /// Used to indicate when the stat's value changes
    /// </summary>
    public interface IStatValue {
        int StatValue { get; }

        void AddValueListener(StatValueChangeEvent func);
        void RemoveValueListener(StatValueChangeEvent func);
    }
}
