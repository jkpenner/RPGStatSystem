using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    static public class RPGStatScalerEditorUtility {
        static public IEditorExtension[] GetExtensions() {
            return new IEditorExtension[] {
            new RPGStatScalerLinearExtension(),
        };
        }

        static public RPGStatScalerAsset CreateAsset(int index) {
            switch (index) {
                case 0: return new RPGStatScalerLinearAsset();
            }
            return null;
        }

        static public string[] GetNames() {
            return new string[] {
                "Linear",
            };
        }
    }
}
