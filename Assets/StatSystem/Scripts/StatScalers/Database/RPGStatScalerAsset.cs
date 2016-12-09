using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System;
using System.Xml;

namespace RPGSystems.StatSystem {
    public class RPGStatScalerAsset : XmlDatabaseAsset {
        public RPGStatScalerAsset() : base() { }
        public RPGStatScalerAsset(int id) : base(id) { }

        public override void OnLoadAsset(XmlReader reader) { }
        public override void OnSaveAsset(XmlWriter writer) { }

        public virtual RPGStatScaler CreateInstance() {
            return null;
        }
    }
}
