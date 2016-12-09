using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System.Xml;
using System;

namespace RPGSystems.StatSystem {
    public class RPGStatScalerLinearAsset : RPGStatScalerAsset {
        public RPGStatScalerLinearAsset() : base() { }
        public RPGStatScalerLinearAsset(int id) : base(id) { }

        public float Offset { get; set; }
        public float Slope { get; set; }

        public override RPGStatScaler CreateInstance() {
            return new RPGStatScalerLinear(this);
        }

        public override void OnLoadAsset(XmlReader reader) {
            Slope = reader.GetAttrFloat("Slope", 1);
            Offset = reader.GetAttrFloat("Offset", 0);
        }

        public override void OnSaveAsset(XmlWriter writer) {
            writer.SetAttr("Slope", Slope);
            writer.SetAttr("Offset", Offset);
        }
    }
}
