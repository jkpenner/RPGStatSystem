using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    public class RPGAttributeAsset : RPGStatModifiableAsset, IStatLinkableAsset {
        public List<RPGStatLinkerAsset> StatLinkers { get; private set; }

        public override RPGStat CreateInstance() {
            return new RPGAttribute(this);
        }

        public RPGAttributeAsset() : base() {
            StatLinkers = new List<RPGStatLinkerAsset>();
        }

        public override void OnSaveAsset(XmlWriter writer) {
            base.OnSaveAsset(writer);

            foreach (RPGStatLinkerAsset asset in StatLinkers) {
                writer.WriteStartElement("StatLinker");
                writer.WriteAttributeString("Type", asset.GetType().Name);
                

                asset.OnSaveAsset(writer);

                writer.WriteEndElement();
            }
        }

        public override void OnLoadAsset(XmlReader reader) {
            base.OnLoadAsset(reader);

            switch(reader.Name) {
                case "StatLinker":
                    string statAssetType = reader.GetAttrString("Type", "");

                    RPGStatLinkerAsset statLinker = RPGStatLinkerUtility.CreateAsset(statAssetType);
                    if (statLinker != null) {
                        statLinker.OnLoadAsset(reader);
                        StatLinkers.Add(statLinker);
                    } else {
                        Debug.LogErrorFormat("[StatLinker]: Could not create linker of type {1}", statAssetType);
                    }
                    break;
                default:
                    
                    if (StatLinkers.Count > 0) {
                        StatLinkers[StatLinkers.Count - 1].OnLoadAsset(reader);
                    }
                    break;
            }
        }
    }
}
