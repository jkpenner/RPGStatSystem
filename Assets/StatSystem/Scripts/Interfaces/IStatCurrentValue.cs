using UnityEngine;
using System.Collections;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// For use with stats that have a current value
    /// </summary>
    public interface IStatCurrentValue {
        int StatCurrentValue { get; set; }
        void SetCurrentValueToMax();
    }
}
