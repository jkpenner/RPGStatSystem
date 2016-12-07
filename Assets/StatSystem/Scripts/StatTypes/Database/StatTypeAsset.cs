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

        public override void OnSaveAsset(XmlWriter writer) {
            writer.WriteElementString("NameShort", NameShort);
            writer.WriteElementString("Description", Description);

            writer.WriteStartElement("Icon");
            writer.SetAttr("Path", Icon);
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
                    Icon = reader.GetAttrResource<Sprite>("Path");
                    break;
            }   
        }
    }
}