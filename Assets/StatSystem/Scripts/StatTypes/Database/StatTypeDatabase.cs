using System;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem.Database {
    public class RPGStatTypeDatabase : AbstractXmlDatabase<StatTypeAsset> {
        static private RPGStatTypeDatabase _instance = null;
        static public RPGStatTypeDatabase Instance {
            get {
                if (_instance == null) {
                    _instance = new RPGStatTypeDatabase();
                    _instance.LoadDatabase();
                }
                return _instance;
            }
        }

        public override string DatabaseName { get { return @"StatTypeDatabase.xml"; } }
        public override string DatabasePath { get { return @"Databases/StatSystem/"; } }

        public override StatTypeAsset CreateAssetOfType(string type) {
            if (type == typeof(StatTypeAsset).Name) {
                return new StatTypeAsset(this.GetNextHighestId());
            }
            return null;
        }
    }
}
