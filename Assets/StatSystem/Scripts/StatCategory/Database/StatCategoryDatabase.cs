using System;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem.Database {
    public class RPGStatCategoryDatabase : AbstractXmlDatabase<StatCategoryAsset> {
        static private RPGStatCategoryDatabase _instance = null;
        static public RPGStatCategoryDatabase Instance {
            get {
                if (_instance == null) {
                    _instance = new RPGStatCategoryDatabase();
                    _instance.LoadDatabase();
                }
                return _instance;
            }
        }

        public override string DatabaseName { get { return @"StatCategoryDatabase.xml"; } }
        public override string DatabasePath { get { return @"Databases/StatSystem/"; } }

        public override StatCategoryAsset CreateAssetOfType(string type) {
            if (type == typeof(StatCategoryAsset).Name) {
                return new StatCategoryAsset(this.GetNextHighestId());
            }
            return null;
        }
    }
}
