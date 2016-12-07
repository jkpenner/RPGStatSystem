using System;
using System.Xml;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public class RPGStatLinkerBasicAsset : RPGStatLinkerAsset {
        public float Ratio { get; set; }

        public override void OnSaveAsset(XmlWriter writer) {
            base.OnSaveAsset(writer);

            writer.SetAttr("Ratio", Ratio);
        }

        public override void OnLoadAsset(XmlReader reader) {
            base.OnLoadAsset(reader);

            Ratio = reader.GetAttrFloat("Ratio", 0f);
        }

        public override RPGStatLinker CreateInstance() {
            return new RPGStatLinkerBasic(this);
        }

        public override RPGStatLinker CreateInstance(RPGStat linkedStat) {
            return new RPGStatLinkerBasic(this, linkedStat);
        }
    }
}