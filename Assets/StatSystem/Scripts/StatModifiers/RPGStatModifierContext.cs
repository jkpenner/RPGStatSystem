using System;
using System.Xml;
using UnityEngine;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    public class RPGStatModifierContext : IXmlOnSaveAsset, IXmlOnLoadAsset {
        [SerializeField]
        private int _assignedStatId = -1;

        [SerializeField]
        private RPGStatModifier _statModifier = null;

        public int AssignedStatId {
            get { return _assignedStatId; }
            set { _assignedStatId = value; }
        }

        public RPGStatModifier StatModifier {
            get { return _statModifier; }
            set { _statModifier = value; }
        }

        public RPGStatModifierContext(int assignedStatId, RPGStatModifier modifier) {
            this.AssignedStatId = assignedStatId;
            this.StatModifier = modifier;
        }

        public void OnSaveAsset(XmlWriter writer) {
            
        }

        public void OnLoadAsset(XmlReader reader) {
            switch (reader.Name) {
                case "StatModifier":

                    break;
            }
        }
    }
}