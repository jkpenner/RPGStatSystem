using System;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// RPGStat that inherits from RPGAttribute and implement IStatCurrentValueChange
    /// </summary>
    public class RPGVital : RPGAttribute, IStatCurrentValue, IStatCurrentValueChange {
        /// <summary>
        /// Used by the StatCurrentValue Property
        /// </summary>
        private int _statCurrentValue;

        /// <summary>
        /// Event called when the StatCurrentValue changes
        /// </summary>
        public CurrentValueEvent _onCurrentValueChange;

        /// <summary>
        /// The current value of the stat. Restricted between the values 0 
        /// and StatValue. When set will trigger the OnCurrentValueChange event.
        /// </summary>
        public int StatCurrentValue {
            get {
                return _statCurrentValue;
            }
            set {
                // Clamp value between the stat value and 0;
                int valueClamped = UnityEngine.Mathf.Clamp(value, 0, StatValue);

                if (_statCurrentValue != valueClamped) {
                    _statCurrentValue = valueClamped;
                    TriggerCurrentValueChange();
                }
            }
        }

        /// <summary>
        /// Constructor that takes a stat asset
        /// </summary>
        public RPGVital(RPGVitalAsset asset) : base(asset) {
            _statCurrentValue = 0;
        }

        /// <summary>
        /// Sets the StatCurrentValue to StatValue
        /// </summary>
        public void SetCurrentValueToMax() {
            StatCurrentValue = StatValue;
        }

        /// <summary>
        /// Triggers the OnCurrentValueChange Event
        /// </summary>
        private void TriggerCurrentValueChange() {
            if (_onCurrentValueChange != null) {
                _onCurrentValueChange(this as IStatCurrentValue);
            }
        }

        /// <summary>
        /// Adds a function of type CurrentValueEvent to the OnValueChange delagate.
        /// </summary>
        public void AddCurrentValueListener(CurrentValueEvent func) {
            _onCurrentValueChange += func;
        }

        /// <summary>
        /// Removes a function of type CurrentValueEvent to the OnValueChange delagate.
        /// </summary>
        public void RemoveCurrentValueListener(CurrentValueEvent func) {
            _onCurrentValueChange -= func;
        }
    }
}
