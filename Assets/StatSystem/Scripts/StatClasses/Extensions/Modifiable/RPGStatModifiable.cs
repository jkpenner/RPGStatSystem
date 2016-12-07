using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// A RPGStat Type that implements both IStatModifiable
    /// </summary>
    public class RPGStatModifiable : RPGStat, IStatModifiable {
        /// <summary>
        /// List of RPGStatModifiers applied to the stat
        /// </summary>
        private List<RPGStatModifier> _statMods;

        /// <summary>
        /// Used by the StatModifierValue Property
        /// </summary>
        private int _statModValue;

        /// <summary>
        /// The stat's total value including StatModifiers
        /// </summary>
        public override int StatValue {
            get { return base.StatValue + StatModifierValue; }
        }

        /// <summary>
        /// The total value of the stat modifiers applied to the stat
        /// </summary>
        public int StatModifierValue {
            get { return _statModValue; }
        }

        /// <summary>
        /// Constructor that takes a stat asset
        /// </summary>
        public RPGStatModifiable(RPGStatModifiableAsset asset) : base(asset) {
            _statModValue = 0;
            _statMods = new List<RPGStatModifier>();

        }

        /// <summary>
        /// Get the number of modifiers active on the stat
        /// </summary>
        public int GetModifierCount() {
            return _statMods.Count;
        }

        /// <summary>
        /// Get the stat modifier with the given index
        /// </summary>
        public RPGStatModifier GetModifierAt(int index) {
            if (index >= 0 && index < _statMods.Count - 1) {
                return _statMods[index];
            }
            return null;
        }

        /// <summary>
        /// Adds Modifier to stat and listens to the mod's value change event
        /// </summary>
        public void AddModifier(RPGStatModifier mod) {
            _statMods.Add(mod);
            mod.AddValueListener(OnModValueChange);
        }

        /// <summary>
        /// Removes modifier from stat and stops listening to value change event
        /// </summary>
        public void RemoveModifier(RPGStatModifier mod) {
            mod.RemoveValueListener(OnModValueChange);
            _statMods.Remove(mod);
        }

        /// <summary>
        /// Removes all modifiers from the stat and stops listening to the value change event
        /// </summary>
        public void ClearModifiers() {
            foreach (var mod in _statMods) {
                mod.RemoveValueListener(OnModValueChange);
                mod.OnModifierRemove();
            }
            _statMods.Clear();
        }

        /// <summary>
        /// Updates the StatModifierValue based of the currently applied modifier values
        /// </summary>
        public void UpdateModifiers() {
            int newStatModValue = 0;

            // Group modifers by the order they are applied
            var orderGroups = _statMods.GroupBy(m => m.Order);
            foreach (var group in orderGroups) {
                // Find the total sum for all stackable modifiers
                // and the max value of all not stackable modifiers
                float sum = 0, max = 0;
                foreach (var mod in group) {
                    if (mod.Stacks == false) {
                        max = Mathf.Max(max, mod.Value);
                    } else {
                        sum += mod.Value;
                    }
                }

                // Apply the stat modifier with either the total sum or
                // the max value, depending on which on is greater
                newStatModValue += group.First().ApplyModifier(
                    StatBaseValue + newStatModValue,
                    sum > max ? sum : max);
            }

            // Trigger value change if stat mod value changed
            if (_statModValue != newStatModValue) {
                _statModValue = newStatModValue;
                TriggerValueChange();
            }
        }

        /// <summary>
        /// Used to listen to the applied stat modifier OnValueChange events
        /// </summary>
        private void OnModValueChange(RPGStatModifier mod) {
            UpdateModifiers();
        }
    }
}
