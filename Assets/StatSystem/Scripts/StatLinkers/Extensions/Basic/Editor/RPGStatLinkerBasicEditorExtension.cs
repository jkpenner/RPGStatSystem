using UnityEngine;
using UnityEditor;
using System;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem {
    public class RPGStatLinkerBasicEditorExtension : EditorExtension {
        public override bool CanHandleType(Type type) {
            return typeof(RPGStatLinkerBasicAsset).IsAssignableFrom(type);
        }

        public override void OnGUI(object asset) {
            var link = asset as RPGStatLinkerBasicAsset;
            if (link != null) {
                link.Ratio = EditorGUILayout.FloatField("Ratio", link.Ratio);
            }
        }
    }
}
