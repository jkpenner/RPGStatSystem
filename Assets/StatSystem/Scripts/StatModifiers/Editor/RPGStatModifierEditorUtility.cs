using UnityEngine;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    /// <summary>
    /// Helpful methods that are used for editor scripts that
    /// work with RPGStatModifer.
    /// 
    /// When adding a new RPGStatModifer extension follow these steps:
    /// 1) Add the Display name of the new modifier to the GetNames method.
    /// 2) Add a CreateInstance call in CreateAsset for the new modifier, the
    /// case number should be the index of the new Display name in GetNames.
    /// 3) If there is an editor extension related add it to the GetExtensions.
    /// Order of extension effects in which order the extensions are displayed.
    /// </summary>
    public class RPGStatModifierEditorUtility : MonoBehaviour {
        /// <summary>
        /// Gets an array containing all extensions that
        /// can apply to a RPGStatModifer.
        /// </summary>
        /// <returns></returns>
        static public IEditorExtension[] GetExtensions() {
            return new IEditorExtension[] {
                
            };
        }

        /// <summary>
        /// Creates an instance of the RPGStatModifer Asset. The index 
        /// relates to the position of the asset's name within the array
        /// gotten from GetName() method.
        /// </summary>
        static public RPGStatModifierAsset CreateAsset(int index) {
            switch (index) {
                case 0: return new RPGStatModBaseAddAsset();
                case 1: return new RPGStatModBasePercentAsset();
                case 2: return new RPGStatModTotalAddAsset();
                case 3: return new RPGStatModTotalPercentAsset();
                default: return null;
            }
        }

        /// <summary>
        /// Gets an array of all the names of each RPGStatModifer type.
        /// </summary>
        static public string[] GetNames() {
            return new string[] {
            "Base Add",
            "Base Percent",
            "Total Add",
            "Total Percent",
        };
        }
    }
}
