using UtilitySystems;

namespace RPGSystems.StatSystem.Database {
    public class RPGStatDatabase : Singleton<RPGStatDatabase> {
        private RPGStatTypeDatabase _statTypes;
        private RPGStatCategoryDatabase _statCategories;
        private RPGStatCollectionDatabase _statCollections;

        private void Awake() {
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }

        static public RPGStatTypeDatabase StatTypes {
            get {
                if (Instance != null) {
                    if (Instance._statTypes == null) {
                        Instance._statTypes = new RPGStatTypeDatabase();
                        Instance._statTypes.LoadDatabase();
                    }
                    return Instance._statTypes;
                }
                return null;
            }
        }

        static public RPGStatCategoryDatabase StatCategories {
            get {
                if (Instance != null) {
                    if (Instance._statCategories == null) {
                        Instance._statCategories = new RPGStatCategoryDatabase();
                        Instance._statCategories.LoadDatabase();
                    }
                    return Instance._statCategories;
                }
                return null;
            }
        }

        static public RPGStatCollectionDatabase StatCollections {
            get {
                if (Instance != null) {
                    if (Instance._statCollections == null) {
                        Instance._statCollections = new RPGStatCollectionDatabase();
                        Instance._statCollections.LoadDatabase();
                    }
                    return Instance._statCollections;
                }
                return null;
            }
        }
    }
}
