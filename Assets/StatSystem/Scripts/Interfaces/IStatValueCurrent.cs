using UnityEngine;
using System.Collections;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Delegate that passes a IStatCurrentValue when activated
    /// </summary>
    public delegate void StatValueCurrentChangeEvent(IStatValueCurrent iStatValueCurrent);

    /// <summary>
    /// For use with stats that have a current value
    /// </summary>
    public interface IStatValueCurrent {
        float StatValueCurrent { get; set; }

        void SetCurrentValueToMax();
        void AddCurrentValueListener(StatValueCurrentChangeEvent func);
        void RemoveCurrentValueListener(StatValueCurrentChangeEvent func);
    }
}
