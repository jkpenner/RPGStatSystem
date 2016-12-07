using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    /// <summary>
    /// Helpful methods that are used for editor scripts that
    /// work with RPGStats.
    /// 
    /// When adding a new RPGStat extension follow these steps:
    /// 1) Add the Display name of the new Stat to the GetNames method.
    /// 2) Add a CreateInstance call in CreateAsset for the new Stat, the
    /// case number should be the index of the new Display name in GetNames.
    /// 3) If there is an editor extension related add it to the GetExtensions.
    /// Order of extension effects in which order the extensions are displayed.
    /// </summary>
    static public class RPGStatEditorUtility {
        /// <summary>
        /// Gets an array containing all extensions that
        /// can apply to a Stat.
        /// </summary>
        /// <returns></returns>
        static public IEditorExtension[] GetExtensions() {
            return new IEditorExtension[] {
                new RPGStatEditorExtension(),
                new RPGStatModifierEditorExtension(),
                new RPGAttributeEditorExtension(),
                new RPGVitalEditorExtension()
            };
        }

        /// <summary>
        /// Creates an instance of the RPGStat Asset. The index 
        /// relates to the position of the asset's name within the array
        /// gotten from GetName() method.
        /// </summary>
        static public RPGStatAsset CreateAsset(int index) {
            switch (index) {
                case 0: return new RPGStatAsset();
                case 1: return new RPGAttributeAsset();
                case 2: return new RPGVitalAsset();
                case 3: return new RPGStatModifiableAsset();
                default: return null;
            }
        }

        /// <summary>
        /// Gets an array of all the names of each RPGStat type.
        /// </summary>
        static public string[] GetNames() {
            return new string[] {
                "Stat",
                "Attribute",
                "Vital",
                "Modifiable",
            };
        }
    }
}
