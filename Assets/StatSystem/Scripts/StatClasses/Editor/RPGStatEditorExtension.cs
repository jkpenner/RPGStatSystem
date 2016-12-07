using UnityEditor;
using System;
using RPGSystems.StatSystem.Database;
using RPGSystems.Utility.Editor;
using UtilitySystem.XmlDatabase.Editor;
using UnityEngine;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatEditorExtension : IEditorExtension {
        public bool CanHandleType(Type type) {
            return typeof(RPGStatAsset).IsAssignableFrom(type);
        }

        public void OnGUI(object asset) {
            RPGStatAsset stat = asset as RPGStatAsset;
            stat.StatBaseValue = EditorGUILayout.IntField("Stat Base Value", stat.StatBaseValue);

            GUILayout.BeginHorizontal();
            var category = RPGStatCategoryDatabase.Instance.Get(stat.StatCategoryId);
            if (category == null) {
                EditorGUILayout.LabelField("Stat Category", "None");
            } else {
                EditorGUILayout.LabelField("Stat Category", category.Name);
            }

            if (GUILayout.Button("Set Category", GUILayout.Width(100))) {
                XmlDatabaseEditorUtility.ShowContext(RPGStatCategoryDatabase.Instance, (categoryAsset) => {
                    stat.StatCategoryId = categoryAsset.Id;
                }, typeof(RPGStatCategoryWindow));
            }
            GUILayout.EndHorizontal();
        }
    }
}
