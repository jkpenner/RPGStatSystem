using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UtilitySystems.XmlDatabase;
using System;
using System.Xml;

namespace RPGSystems.StatSystem.Database {
    public class RPGStatCollectionAsset : XmlDatabaseAsset {
        public List<RPGStatAsset> Stats { get; private set; }

        public RPGStatCollectionAsset() : base() { Stats = new List<RPGStatAsset>(); }
        public RPGStatCollectionAsset(int id) : base(id) { Stats = new List<RPGStatAsset>(); }

        public override void OnSaveAsset(XmlWriter writer) {
            foreach (var stat in Stats) {
                writer.WriteStartElement("Stat");
                writer.SetAttr("AssetType", stat.GetType().Name);
                stat.OnSaveAsset(writer);
                writer.WriteEndElement();
            }
        }

        public override void OnLoadAsset(XmlReader reader) {
            switch (reader.Name) {
                case "Stat":
                    // Get the asset initial values
                    string statAssetType = reader.GetAttrString("AssetType", "");

                    // Create an instance of the stat asset
                    var asset = RPGStatUtility.CreateAssetOfType(statAssetType);
                    if (asset != null) {
                        Stats.Add(asset);
                        // Let the asset load some values
                        Stats[Stats.Count - 1].OnLoadAsset(reader);
                    }
                    break;
                default:
                    if (Stats.Count > 0) {
                        // If element is not handled, it must be handled by
                        // a stat asset. Pass the reader to the most recent Stat.
                        Stats[Stats.Count - 1].OnLoadAsset(reader);
                    }
                    break;
            }
        }
    }
}
