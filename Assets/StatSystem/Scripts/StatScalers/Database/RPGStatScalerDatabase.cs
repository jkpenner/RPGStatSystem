using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;
using System;

namespace RPGSystems.StatSystem {
    public class RPGStatScalerDatabase : AbstractXmlDatabase<RPGStatScalerAsset> {
        static private RPGStatScalerDatabase _instance = null;
        static public RPGStatScalerDatabase Instance {
            get {
                if (_instance == null) {
                    _instance = new RPGStatScalerDatabase();
                    _instance.LoadDatabase();
                }
                return _instance;
            }
        }

        public override string DatabaseName { get { return @"StatScalerDatabase.xml"; } }
        public override string DatabasePath { get { return @"Databases/StatSystem/"; } }

        public override RPGStatScalerAsset CreateAssetOfType(string type) {
            if (type == typeof(RPGStatScalerLinearAsset).Name) {
                return new RPGStatScalerLinearAsset(this.GetNextHighestId());
            }
            return null;
        }

        public override string[] GetListOfAssetTypes() {
            return new string[] {
            "RPGStatScalerLinearAsset",
        };
        }
    }
}
