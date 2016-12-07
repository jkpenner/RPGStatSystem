using System;
using UnityEngine;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// The base class for all other Stats.
    /// </summary>
    public class RPGStat : IStatValueChange {
        /// <summary>
        /// Used by the StatBase Value Property
        /// </summary>
        private int _statBaseValue;

        /// <summary>
        /// Used by the StatCategoryId Property
        /// </summary>
        private int _statCategoryId;

        /// <summary>
        /// Event that triggers when the stat value changes
        /// </summary>
        private RPGStatEvent _onValueChange;

        /// <summary>
        /// The Total Value of the stat
        /// </summary>
        public virtual int StatValue {
            get { return StatBaseValue; }
        }

        /// <summary>
        /// The Base Value of the stat
        /// </summary>
        public virtual int StatBaseValue {
            get { return _statBaseValue; }
            set {
                if (_statBaseValue != value) {
                    _statBaseValue = value;
                    TriggerValueChange();
                }
            }
        }

        /// <summary>
        /// The stat type the stat is assigned to
        /// </summary>
        public int AssignedStatId { get; private set; }

        /// <summary>
        /// The Stat Category of the stat
        /// </summary>
        public int StatCategoryId { get; private set; }

        /// <summary>
        /// Constructor that takes a stat asset
        /// </summary>
        public RPGStat(RPGStatAsset asset) {
            this.StatBaseValue = asset.StatBaseValue;
            this.AssignedStatId = asset.AssignedStatId;
            this.StatCategoryId = asset.StatCategoryId;
        }

        /// <summary>
        /// Triggers the OnValueChange Event
        /// </summary>
        protected void TriggerValueChange() {
#if DEBUG_STAT_INFO
            Debug.LogFormat("[Stat Category {0}]: Trigger stat value change", StatCategoryId);
#endif
            if (_onValueChange != null) {
                _onValueChange(this);
            }
        }

        /// <summary>
        /// Change the StatBaseValue by a set amount
        /// </summary>
        public void ModifyBaseValue(int amount) {
#if DEBUG_STAT_INFO
            Debug.LogFormat("[Stat Category {0}]: Modify Base Value by {1}", StatCategoryId, amount);
#endif
            if (amount != 0) {
                _statBaseValue += amount;
                TriggerValueChange();
            }
        }

        /// <summary>
        /// Adds a function of type RPGStatModifierEvent to the OnValueChange delagate.
        /// </summary>
        public void AddValueListener(RPGStatEvent func) {
#if DEBUG_STAT_INFO
            Debug.LogFormat("[Stat Category {0}]: Add Value Listener", StatCategoryId);
#endif
            _onValueChange += func;
        }

        /// <summary>
        /// Removes a function of type RPGStatModifierEvent to the OnValueChange delagate.
        /// </summary>
        public void RemoveValueListener(RPGStatEvent func) {
#if DEBUG_STAT_INFO
            Debug.LogFormat("[Stat Category {0}]: Remove Value Listener", StatCategoryId);
#endif
            _onValueChange -= func;
        }
    }
}
