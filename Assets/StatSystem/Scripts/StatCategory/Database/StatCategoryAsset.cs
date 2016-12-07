using System;
using System.Xml;
using UnityEngine;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem.Database {
    public class StatCategoryAsset : XmlDatabaseAsset {

        public StatCategoryAsset() : base() {}
        public StatCategoryAsset(int id) : base(id) {}

        public override void OnSaveAsset(XmlWriter writer) {

        }

        public override void OnLoadAsset(XmlReader reader) {

        }
    }
}