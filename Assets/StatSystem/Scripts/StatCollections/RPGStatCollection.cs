using UnityEngine;
using System.Collections.Generic;
using RPGSystems.StatSystem.Database;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// The base class used to define a collection of RPGStats.
    /// Also used to apply and remove RPGStatModifiers from individual
    /// RPGStats.
    /// </summary>
    public class RPGStatCollection : MonoBehaviour {
        [SerializeField]
        private int _statCollectionId = -1;
        public int StatCollectionId {
            get { return _statCollectionId; }
            set { _statCollectionId = value; }
        }

        private Dictionary<int, RPGStat> _statDict;

        /// <summary>
        /// Dictionary containing all stats held within the collection
        /// </summary>
        public Dictionary<int, RPGStat> StatDict {
            get {
                if (_statDict == null) {
                    _statDict = new Dictionary<int, RPGStat>();
                }
                return _statDict;
            }
        }

        /// <summary>
        /// Initializes the RPGStats class
        /// </summary>
        private void Awake() {
            var collection = RPGStatCollectionDatabase.Instance.Get(StatCollectionId);
            if (collection != null) {
                SetupCollection(collection);
            }
        }

        public void SetupCollection(RPGStatCollectionAsset collectionAsset) {
            if(collectionAsset != null) {
                // Initial add all stats to the collection
                foreach (var statAsset in collectionAsset.Stats) {
                    //Debug.LogFormat("Adding Stat {0} to Collection, Asset {1}", info.statTypeid, info.statAsset.statName);
                    if (!StatDict.ContainsKey(statAsset.AssignedStatId)) {
                        StatDict.Add(statAsset.AssignedStatId, statAsset.CreateInstance());
                        Debug.LogFormat("Added Stat ID {0} with value {1}", 
                            statAsset.AssignedStatId,
                            StatDict[statAsset.AssignedStatId].StatValue);
                    } else {
                        // Stat already exsists in the collection
                        Debug.LogWarningFormat("Attempting to add Stat with id {0}, " +
                            "but that id already exists in the stat collection.", statAsset.AssignedStatId);
                    }
                }

                // Add all stat linkers to stats
                foreach (var statAsset in collectionAsset.Stats) {
                    var stat = StatDict[statAsset.AssignedStatId] as IStatLinkable;
                    if (stat != null) {
                        var linkAsset = statAsset as IStatLinkableAsset;
                        foreach (var statLinker in linkAsset.StatLinkers) {
                            if (StatDict.ContainsKey(statLinker.linkedStatType)) {
                                var linkedStat = StatDict[statLinker.linkedStatType];
                                if (linkedStat == null) {
                                    Debug.LogErrorFormat("Linked stat is null for type {0}", statLinker.linkedStatType);
                                }
                                stat.AddLinker(statLinker.CreateInstance(linkedStat));
                            } else {
                                // Target of stat linker does not exsist in collection
                            }
                        }
                        stat.UpdateLinkerValue();
                    } else {
                        // Target of stat linker is not linkable
                    }
                }

                // Set all stat's that have a current value to their max values
                foreach (var statAsset in collectionAsset.Stats) {
                    var currentValue = GetStat(statAsset.AssignedStatId) as IStatCurrentValue;
                    if (currentValue != null) {
                        currentValue.SetCurrentValueToMax();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there is a RPGStat with the given type and id
        /// </summary>
        public bool ContainStat(int statTypeId) {
            return StatDict.ContainsKey(statTypeId);
        }

        /// <summary>
        /// Gets the RPGStat with the given RPGStatTyp and ID
        /// </summary>
        public RPGStat GetStat(int statTypeId) {
            if (ContainStat(statTypeId)) {
                return StatDict[statTypeId];
            }
            return null;
        }

        /// <summary>
        /// Gets the RPGStat with the given int and ID as type T
        /// </summary>
        public T GetStat<T>(int statTypeId) where T : RPGStat {
            return GetStat(statTypeId) as T;
        }

        /// <summary>
        /// Creates a new instance of the stat ands adds it to the StatDict
        /// </summary>
        protected T CreateStat<T>(int statTypeId) where T : RPGStat, new() {
            T stat = new T();
            StatDict.Add(statTypeId, stat);
            return stat;
        }

        /// <summary>
        /// Creates or Gets a RPGStat of type T. Used within the setup method during initialization.
        /// </summary>
        protected T CreateOrGetStat<T>(int statTypeId) where T : RPGStat, new() {
            T stat = GetStat<T>(statTypeId);
            if (stat == null) {
                stat = CreateStat<T>(statTypeId);
            }
            return stat;
        }


        /// <summary>
        /// Adds a Stat Modifier to the Target stat.
        /// </summary>
        public void AddStatModifier(int targetId, RPGStatModifier mod) {
            AddStatModifier(targetId, mod, false);
        }

        /// <summary>
        /// Adds a Stat Modifier to the Target stat and then updates the stat's value.
        /// </summary>
        public void AddStatModifier(int targetId, RPGStatModifier mod, bool update) {
            if (ContainStat(targetId)) {
                var modStat = GetStat(targetId) as IStatModifiable;
                if (modStat != null) {
                    modStat.AddModifier(mod);
                    mod.OnModifierApply(this, targetId);
                    if (update == true) {
                        modStat.UpdateModifiers();
                    }
                } else {
                    Debug.Log("[RPGStats] Trying to add Stat Modifier to non modifiable stat \"" + targetId.ToString() + "\"");
                }
            } else {
                Debug.Log("[RPGStats] Trying to add Stat Modifier to \"" + targetId.ToString() + "\", but RPGStats does not contain that stat");
            }
        }

        /// <summary>
        /// Removes a Stat Modifier to the Target stat.
        /// </summary>
        public void RemoveStatModifier(int targetId, RPGStatModifier mod) {
            RemoveStatModifier(targetId, mod, false);
        }

        /// <summary>
        /// Removes a Stat Modifier to the Target stat and then updates the stat's value.
        /// </summary>
        public void RemoveStatModifier(int targetId, RPGStatModifier mod, bool update) {
            if (ContainStat(targetId)) {
                var modStat = GetStat(targetId) as IStatModifiable;
                if (modStat != null) {
                    modStat.RemoveModifier(mod);
                    mod.OnModifierRemove();
                    if (update == true) {
                        modStat.UpdateModifiers();
                    }
                } else {
                    Debug.Log("[RPGStats] Trying to remove Stat Modifier from non modifiable stat \"" + targetId.ToString() + "\"");
                }
            } else {
                Debug.Log("[RPGStats] Trying to remove Stat Modifier from \"" + targetId.ToString() + "\", but RPGStatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Clears all stat modifiers from all stats in the collection.
        /// </summary>
        public void ClearStatModifiers() {
            ClearStatModifiers(false);
        }

        /// <summary>
        /// Clears all stat modifiers from all stats in the collection then updates all the stat's values.
        /// </summary>
        /// <param name="update"></param>
        public void ClearStatModifiers(bool update) {
            foreach (var key in StatDict.Keys) {
                ClearStatModifier(key, update);
            }
        }

        /// <summary>
        /// Clears all stat modifiers from the target stat.
        /// </summary>
        public void ClearStatModifier(int targetId) {
            ClearStatModifier(targetId, false);
        }

        /// <summary>
        /// Clears all stat modifiers from the target stat then updates the stat's value.
        /// </summary>
        public void ClearStatModifier(int targetId, bool update) {
            if (ContainStat(targetId)) {
                var modStat = GetStat(targetId) as IStatModifiable;
                if (modStat != null) {
                    modStat.ClearModifiers();
                    if (update == true) {
                        modStat.UpdateModifiers();
                    }
                } else {
                    Debug.Log("[RPGStats] Trying to clear Stat Modifiers from non modifiable stat \"" + targetId.ToString() + "\"");
                }
            } else {
                Debug.Log("[RPGStats] Trying to clear Stat Modifiers from \"" + targetId.ToString() + "\", but RPGStatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Updates all stat modifier's values
        /// </summary>
        public void UpdateStatModifiers() {
            foreach (var key in StatDict.Keys) {
                UpdateStatModifer(key);
            }
        }

        /// <summary>
        /// Updates the target stat's modifier value
        /// </summary>
        public void UpdateStatModifer(int targetId) {
            if (ContainStat(targetId)) {
                var modStat = GetStat(targetId) as IStatModifiable;
                if (modStat != null) {
                    modStat.UpdateModifiers();
                } else {
                    Debug.Log("[RPGStats] Trying to Update Stat Modifiers for a non modifiable stat \"" + targetId.ToString() + "\"");
                }
            } else {
                Debug.Log("[RPGStats] Trying to Update Stat Modifiers for \"" + targetId.ToString() + "\", but RPGStatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Scales all stats in the collection to the same target level
        /// </summary>
        public void ScaleStatCollection(int level) {
            foreach (var key in StatDict.Keys) {
                ScaleStat(key, level);
            }
        }

        /// <summary>
        /// Scales the target stat in the collection to the target level
        /// </summary>
        public void ScaleStat(int targetId, int level) {
            if (ContainStat(targetId)) {
                var stat = GetStat(targetId) as IStatScalable;
                if (stat != null) {
                    stat.ScaleStatToLevel(level);
                } else {
                    Debug.Log("[RPGStats] Trying to Scale Stat with a non scalable stat \"" + targetId.ToString() + "\"");
                }
            } else {
                Debug.Log("[RPGStats] Trying to Scale Stat for \"" + targetId.ToString() + "\", but RPGStatCollection does not contain that stat");
            }
        }
    }
}
