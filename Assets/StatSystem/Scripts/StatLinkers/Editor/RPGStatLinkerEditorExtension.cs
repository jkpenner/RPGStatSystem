using UnityEngine;
using UnityEditor;
using System;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatLinkerEditorExtension : EditorExtension {
        public override bool CanHandleType(Type type) {
            return typeof(RPGStatLinkerAsset).IsAssignableFrom(type);
        }

        public override void OnGUI(object asset) {
            var link = asset as RPGStatLinkerAsset;
            if (link != null) {
                var statType = RPGStatTypeDatabase.Instance.Get(link.linkedStatType);
                string displayText = string.Empty;
                if (statType != null) {
                    displayText = statType.Name;
                } else if (link.linkedStatType <= 0) {
                    displayText = "Type Not Set";
                } else {
                    displayText = "Error Type not Found";
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Linked Stat", displayText);
                if (GUILayout.Button("Change Type", EditorStyles.miniButtonMid, GUILayout.Width(100))) {
                    XmlDatabaseEditorUtility.ShowContext(RPGStatTypeDatabase.Instance, (statTypeAsset) => {
                        link.linkedStatType = statTypeAsset.Id;
                        //EditorUtility.SetDirty(RPGStatCollectionDatabase.Instance);
                        //EditorWindow.FocusWindowIfItsOpen<RPGStatCollectionWindow>();
                    }, typeof(RPGStatTypeWindow));
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
