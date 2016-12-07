using System;
using System.Xml;
using UnityEngine;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem.Database {
    public class StatTypeAsset : XmlDatabaseAsset {
        public string NameShort { get; set; }
        public string Description { get; set; }
        public Sprite Icon { get; set; }

        public StatTypeAsset() : base() {
            NameShort = string.Empty;
            Description = string.Empty;
            Icon = null;
        }

        public StatTypeAsset(int id) : base(id) {
            NameShort = string.Empty;
            Description = string.Empty;
            Icon = null;
        }

        public StatTypeAsset(int id, string name) : base (id, name) {
            NameShort = name;
            Description = string.Empty;
            Icon = null;
        }

        public StatTypeAsset(int id, string name, string shortName, string description, Sprite icon) : base(id, name) {
            NameShort = shortName;
            Description = description;
            Icon = icon;
        }

        public override void OnSaveAsset(XmlWriter writer) {
            writer.WriteElementString("NameShort", NameShort);
            writer.WriteElementString("Description", Description);

            writer.WriteStartElement("Icon");
            writer.WriteAttributeString("Path", GetAssetResourcePath(Icon));
            writer.WriteEndElement();
        }

        public override void OnLoadAsset(XmlReader reader) {
            switch (reader.Name) {
                case "NameShort":
                    NameShort = reader.ReadElementContentAsString();
                    break;
                case "Description":
                    Description = reader.ReadElementContentAsString();
                    break;
                case "Icon":
                    Icon = Resources.Load<Sprite>(reader.GetStringAttribute("Path", ""));
                    break;
            }   
        }
    }
}