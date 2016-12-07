using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatCategoryWindow : XmlDatabaseWindowSimple<StatCategoryAsset> {
        private RPGStatCategoryDatabase _database;

        [MenuItem("Window/RPGSystems/Stats/Stat Categories")]
        static public void ShowWindow() {
            var wnd = GetWindow<RPGStatCategoryWindow>();
            wnd.titleContent.text = "Stat Category";
            wnd.Show();
        }

        protected override AbstractXmlDatabase<StatCategoryAsset> GetDatabaseInstance() {
            if (_database == null) {
                _database = new RPGStatCategoryDatabase();
                _database.LoadDatabase();
            }
            return _database;
        }

        protected override void DisplayAssetGUI(StatCategoryAsset asset) {
            GUILayout.BeginVertical("Box");

            //GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(80));
            EditorGUI.indentLevel++;
            asset.Name = EditorGUILayout.TextField(asset.Name);
            EditorGUI.indentLevel--;
            //GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public override void DisplayGUIFooter() {
            base.DisplayGUIFooter();

                if (GUILayout.Button("Generate Enum", EditorStyles.toolbarButton) &&
                    EditorUtility.DisplayDialog("Generate Enum", "Generating Stat Category enum will " +
                    "remove all changes made to the RPGStatCategory script file. Are you sure you want " +
                    "to generate?", "Generate", "Cancel")) {
                    RPGStatCategoryGenerator.CheckAndGenerateFile();
                }
            }
    }
}