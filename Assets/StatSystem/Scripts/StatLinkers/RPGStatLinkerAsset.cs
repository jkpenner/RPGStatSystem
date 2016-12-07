using System;
using System.Xml;
using UnityEngine;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public abstract class RPGStatLinkerAsset : IXmlOnSaveAsset, IXmlOnLoadAsset {
        public int linkedStatType;

        public abstract RPGStatLinker CreateInstance();

        public abstract RPGStatLinker CreateInstance(RPGStat linkedStat);

        public virtual void OnSaveAsset(XmlWriter writer) {
            writer.SetAttr("LinkedStatType", linkedStatType);
        }

        public virtual void OnLoadAsset(XmlReader reader) {
            linkedStatType = reader.GetAttrInt("LinkedStatType", 0);
        }
    }
}