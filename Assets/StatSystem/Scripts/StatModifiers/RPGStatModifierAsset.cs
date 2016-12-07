using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System;
using System.Xml;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public abstract class RPGStatModifierAsset : IXmlOnSaveAsset, IXmlOnLoadAsset {
        public int assignedStatId;

        public float value;
        public bool stacks;

        public abstract RPGStatModifier CreateInstance();

		protected virtual T Internal_CreateInstance<T>() where T : RPGStatModifier {
            var mod = System.Activator.CreateInstance<T>() as RPGStatModifier;
            mod.Value = value;
            mod.Stacks = stacks;
            return mod as T;
        }

        public void OnSaveAsset(XmlWriter writer) {
            writer.WriteAttributeString("Value", value.ToString());
            writer.WriteAttributeString("Stacks", stacks.ToString());
        }

        public void OnLoadAsset(XmlReader reader) {
            reader.GetAttrFloat("Value", 0f);
            reader.GetBoolAttribute("Stacks", true);
        }
    }
}