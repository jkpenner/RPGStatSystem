using UnityEngine;
using UnityEditor;
using System;
using RPGSystems.StatSystem.Database;
using RPGSystems.Utility.Editor;

namespace RPGSystems.StatSystem {
    public class RPGStatLinkerBasicEditorExtension : IEditorExtension {
        public bool CanHandleType(Type type) {
            return typeof(RPGStatLinkerBasicAsset).IsAssignableFrom(type);
        }

        public void OnGUI(object asset) {
            var link = asset as RPGStatLinkerBasicAsset;
            if (link != null) {
                link.Ratio = EditorGUILayout.FloatField("Ratio", link.Ratio);
            }
        }
    }
}
