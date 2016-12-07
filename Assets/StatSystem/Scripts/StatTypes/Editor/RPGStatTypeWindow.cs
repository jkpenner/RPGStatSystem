using UnityEngine;
using UnityEditor;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatTypeWindow : XmlDatabaseWindowSimple<StatTypeAsset> {
        private RPGStatTypeDatabase _database = null;

        [MenuItem("Window/RPGSystems/Stats/Stat Types")]
        static public void ShowWindow() {
            var wnd = GetWindow<RPGStatTypeWindow>();
            wnd.titleContent.text = "Stat Types";
            wnd.Show();
        }

        protected override AbstractXmlDatabase<StatTypeAsset> GetDatabaseInstance() {
            if (_database == null) {
                _database = new RPGStatTypeDatabase();
                _database.LoadDatabase();
            }
            return _database;
        }

        protected override void DisplayAssetGUI(StatTypeAsset asset) {
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();

            asset.Icon = (Sprite)EditorGUILayout.ObjectField(asset.Icon, typeof(Sprite), false,
                GUILayout.Width(72), GUILayout.Height(72));

            GUILayout.BeginVertical();

            //GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(80));
            EditorGUI.indentLevel++;
            asset.Name = EditorGUILayout.TextField(asset.Name);
            EditorGUI.indentLevel--;
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            GUILayout.Label("Short Name", GUILayout.Width(80));
            EditorGUI.indentLevel++;
            asset.NameShort = EditorGUILayout.TextField(asset.NameShort);
            EditorGUI.indentLevel--;
            //GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Description", GUILayout.Width(80));
            asset.Description = EditorGUILayout.TextArea(asset.Description);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public override void DisplayGUIFooter() {
            base.DisplayGUIFooter();

                if (GUILayout.Button("Generate Enum", EditorStyles.toolbarButton) &&
                    EditorUtility.DisplayDialog("Generate Enum", "Generating Stat Type enum will " +
                    "remove all changes made to the RPGStatType script file. Are you sure you want " +
                    "to generate?", "Generate", "Cancel")) {
                    RPGStatTypeGenerator.CheckAndGenerateFile();
                }
            }
    }
}