using System;
using UnityEngine;
using UnityEditor;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatEditorExtension : EditorExtension {
        public override bool CanHandleType(Type type) {
            return typeof(RPGStatAsset).IsAssignableFrom(type);
        }

        public override void OnGUI(object asset) {
            RPGStatAsset stat = asset as RPGStatAsset;
            stat.StatBaseValue = EditorGUILayout.IntField("Stat Base Value", stat.StatBaseValue);

            GUILayout.BeginHorizontal();
            var category = RPGStatCategoryDatabase.Instance.Get(stat.StatCategoryId);
            if (category == null) {
                EditorGUILayout.LabelField("Stat Category", "None");
            } else {
                EditorGUILayout.LabelField("Stat Category", category.Name);
            }

            if (GUILayout.Button("Set Category", EditorStyles.miniButtonMid, GUILayout.Width(100))) {
                RPGStatCategoryDatabase.Instance.LoadDatabase();
                XmlDatabaseEditorUtility.ShowContext(RPGStatCategoryDatabase.Instance, (categoryAsset) => {
                    stat.StatCategoryId = categoryAsset.Id;
                }, typeof(RPGStatCategoryWindow));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            if (stat.StatScaler != null) {
                EditorGUILayout.LabelField("Stat Scaler", stat.StatScaler.GetType().Name);
            } else {
                EditorGUILayout.LabelField("Stat Scaler", "No Scaler Set");
            }

            if (GUILayout.Button("Set", EditorStyles.toolbarButton, GUILayout.Width(40))) {
                RPGStatScalerDatabase.Instance.LoadDatabase();
                XmlDatabaseEditorUtility.GetGenericMenu(RPGStatScalerEditorUtility.GetNames(), (selectedIndex) => {
                    stat.StatScaler = RPGStatScalerEditorUtility.CreateAsset(selectedIndex);
                }).ShowAsContext();
            }

            if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(60))) {
                stat.StatScaler = null;
            }
            GUILayout.EndHorizontal();

            if (stat.StatScaler != null) {
                foreach (var extension in RPGStatScalerEditorUtility.GetExtensions()) {
                    if (extension.CanHandleType(stat.StatScaler.GetType())) {
                        extension.OnGUI(stat.StatScaler);
                    }
                }
            } else {
                GUILayout.Label("No Stat Scaler selected. Stat will not scale with level",
                    EditorStyles.centeredGreyMiniLabel);
            }
        }
    }
}
