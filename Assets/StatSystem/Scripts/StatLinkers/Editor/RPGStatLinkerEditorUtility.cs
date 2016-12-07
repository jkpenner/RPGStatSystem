using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    /// <summary>
    /// Helpful methods that are used for editor scripts that
    /// work with RPGStatLinkers.
    /// 
    /// When adding a new RPGStatLinker extension follow these steps:
    /// 1) Add the Display name of the new linker to the GetNames method.
    /// 2) Add a CreateInstance call in CreateAsset for the new linker, the
    /// case number should be the index of the new Display name in GetNames.
    /// 3) If there is an editor extension related add it to the GetExtensions.
    /// Order of extension effects in which order the extensions are displayed.
    /// </summary>
    static public class RPGStatLinkerEditorUtility {
        /// <summary>
        /// Gets an array containing all extensions that
        /// can apply to a StatLinker.
        /// </summary>
        /// <returns></returns>
        static public IEditorExtension[] GetExtensions() {
            return new IEditorExtension[] {
                new RPGStatLinkerEditorExtension(),
                new RPGStatLinkerBasicEditorExtension(),
            };
        }

        /// <summary>
        /// Creates an instance of the RPGStatLinker Asset. The index 
        /// relates to the position of the asset's name within the array
        /// gotten from GetName() method.
        /// </summary>
        static public RPGStatLinkerAsset CreateAsset(int index) {
            switch (index) {
                case 0: return new RPGStatLinkerBasicAsset(); ;
                default: return null;
            }
        }

        /// <summary>
        /// Gets an array of all the names of each RPGStatLinker type.
        /// </summary>
        static public string[] GetNames() {
            return new string[] {
                "Ratio",
            };
        }
    }
}