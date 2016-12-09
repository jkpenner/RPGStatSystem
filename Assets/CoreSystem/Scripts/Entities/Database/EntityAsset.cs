using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System;
using System.Xml;

public class EntityAsset : XmlDatabaseAsset {
    public GameObject Prefab { get; set; }

    public EntityAsset() { }
    public EntityAsset(int id) : base(id) { }

    public override void OnLoadAsset(XmlReader reader) {
        switch (reader.Name) {
            case "AssetParams":
                Prefab = reader.GetAttrResource<GameObject>("Prefab");
                break;
        }
    }

    public override void OnSaveAsset(XmlWriter writer) {
        writer.WriteStartElement("AssetParams");
        writer.SetAttr("Prefab", Prefab);
        writer.WriteEndElement();
    }
}