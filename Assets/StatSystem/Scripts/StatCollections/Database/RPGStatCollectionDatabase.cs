using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System;

namespace RPGSystems.StatSystem.Database {
    public class RPGStatCollectionDatabase : AbstractXmlDatabase<RPGStatCollectionAsset> {
        static private RPGStatCollectionDatabase _instance = null;
        static public RPGStatCollectionDatabase Instance {
            get {
                if (_instance == null) {
                    _instance = new RPGStatCollectionDatabase();
                    _instance.LoadDatabase();
                }
                return _instance;
            }
        }

        public override string DatabasePath { get { return "Databases/StatSystem/"; } }
        public override string DatabaseName { get { return "StatCollectionDatabase.xml"; } }

        public override RPGStatCollectionAsset CreateAssetOfType(string type) {
            if (typeof(RPGStatCollectionAsset).Name == type) {
                return new RPGStatCollectionAsset();
            }
            return null;
        }
    }
}