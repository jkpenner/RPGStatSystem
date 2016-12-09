using System.Linq;
using UnityEngine;
using UnityEditor;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;
using System;

namespace RPGSystems.StatSystem.Editor {
    public class RPGStatCollectionWindow : XmlDatabaseWindowComplex<RPGStatCollectionAsset> {
        private Vector2 statSelectionScroll = Vector2.zero;
        private float statSelectionWidth = 200;

        private int _selectedStatIndex = -1;
        public int SelectedStatIndex {
            get {
                return _selectedStatIndex;
            }
            set {
                if (_selectedStatIndex != value) {
                    _selectedStatIndex = value;
                    EditorGUI.FocusTextInControl(string.Empty);
                }
            }
        }

        [MenuItem("Window/RPGSystems/Stats/Stat Collections")]
        static public void ShowWindow() {
            var wnd = GetWindow<RPGStatCollectionWindow>();
            wnd.titleContent.text = "Stat Collections";
            wnd.Show();
        }

        protected override AbstractXmlDatabase<RPGStatCollectionAsset> GetDatabaseInstance() {
            return RPGStatCollectionDatabase.Instance;
        }

        protected override RPGStatCollectionAsset CreateDefaultAsset() {    
            return new RPGStatCollectionAsset(GetDatabaseInstance().GetNextHighestId());
        }

        protected override void DisplayAssetGUI(RPGStatCollectionAsset asset) {
            GUILayout.BeginVertical();

            var selectedCollection = RPGStatCollectionDatabase.Instance.Get(SelectedAssetId);
            if (selectedCollection != null) {

                GUILayout.Label(selectedCollection.Name, EditorStyles.toolbarButton);

                GUILayout.BeginHorizontal();
                GUILayout.Label("ID: " + selectedCollection.Id.ToString("D4") + ", Name ", GUILayout.Width(100));
                selectedCollection.Name = EditorGUILayout.TextField(selectedCollection.Name);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                DisplayStatSelectionContent(selectedCollection);

                if (SelectedStatIndex >= 0 && SelectedStatIndex < selectedCollection.Stats.Count) {
                    DisplaySelectedStatContent(selectedCollection.Stats[SelectedStatIndex]);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        protected override void OnDisplayFooter() {
            DisplayStatSelectionFooter();

            base.OnDisplayFooter();
        }

        private void DisplaySelectedStatContent(RPGStatAsset stat) {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            var statType = RPGStatTypeDatabase.Instance.Get(stat.AssignedStatId, true);

            GUILayout.Label(string.Format("[{0}]: {1}", stat.CreateInstance().GetType().Name, 
                statType == null ? "Stat Type Not Set" : statType.Name));

            if (GUILayout.Button(statType == null ? "Assign Type" : "Change Type", EditorStyles.toolbarButton, GUILayout.Width(100))) {
                XmlDatabaseEditorUtility.ShowContext(RPGStatTypeDatabase.Instance, (statTypeAsset) => {
                    stat.AssignedStatId = statTypeAsset.Id;
                    Debug.Log("Assigning stat to stat type " + statTypeAsset.Id);
                }, typeof(RPGStatTypeWindow));
            }
            GUILayout.EndHorizontal();

            if (stat != null) {
                foreach (var extension in RPGStatEditorUtility.GetExtensions()) {
                    if (extension.CanHandleType(stat.GetType())) {
                        extension.OnGUI(stat);
                    }
                }
            }

            GUILayout.EndVertical();
        }

        private void DisplayStatSelectionContent(RPGStatCollectionAsset asset) {
            GUILayout.BeginVertical(GUILayout.Width(200));

            // Scroll view for the listed assets
            statSelectionScroll = GUILayout.BeginScrollView(statSelectionScroll, false, true);

            var categoryGroups = asset.Stats.GroupBy(stat => {
                var categoryAsset = RPGStatCategoryDatabase.Instance.Get(stat.StatCategoryId, true);
                if (categoryAsset != null) {
                    return stat.StatCategoryId;
                } else {
                    return 0;
                }
            });

            // List all the stats in the collection
            GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));
            foreach (var categoryGroup in categoryGroups) {
                var categoryAsset = RPGStatCategoryDatabase.Instance.Get(
                    categoryGroup.First().StatCategoryId, true);

                if (categoryAsset != null) {
                    GUILayout.Label(categoryAsset.Name, EditorStyles.centeredGreyMiniLabel);
                } else {
                    GUILayout.Label("Not Categorized", EditorStyles.centeredGreyMiniLabel);
                }

                foreach (var stat in categoryGroup) {
                    if (stat != null) {
                        var statType = RPGStatTypeDatabase.Instance.Get(stat.AssignedStatId, true);

                        string displayText;
                        if (statType != null) {
                            displayText = statType.Name;
                        } else if (stat.AssignedStatId <= 0) {
                            displayText = "Stat Type Not Assigned";
                        } else {
                            displayText = "Error Type not Found";
                        }

                        int statIndex = asset.Stats.IndexOf(stat);

                        var select = GUILayout.Toggle(statIndex == SelectedStatIndex, displayText, EditorStyles.toolbarButton);
                        if (select == true) {
                            SelectedStatIndex = statIndex;
                        }
                    }
                }
            }

            if (asset.Stats.Count <= 0) {
                GUILayout.Label("No Stats Created\nClick '+' to add new Stat.", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void DisplayStatSelectionFooter() {
            var selectedCollection = RPGStatCollectionDatabase.Instance.Get(SelectedAssetId, true);
            if (selectedCollection != null) {

                GUILayout.BeginHorizontal(GUILayout.Width(200));
                if (GUILayout.Button("+", EditorStyles.toolbarButton)) {
                    XmlDatabaseEditorUtility.GetGenericMenu(RPGStatEditorUtility.GetNames(), (index) => {
                        var statAsset = RPGStatEditorUtility.CreateAsset(index);
                        selectedCollection.Stats.Add(statAsset);
                    
                        //info.StatAsset.statName = RPGStatTypeDatabase.GetAsset(info.StatTypeId).Name;

                        SelectedStatIndex = selectedCollection.Stats.Count - 1;
                        EditorWindow.FocusWindowIfItsOpen<RPGStatCollectionWindow>();
                    }).ShowAsContext();
                }
                if (GUILayout.Button("-", EditorStyles.toolbarButton) &&
                    EditorUtility.DisplayDialog("Delete Stat", "Are you sure you want to delete the " +
                    "selected stat?", "Delete", "Cancel")) {
                    if (SelectedStatIndex >= 0 && SelectedStatIndex < selectedCollection.Stats.Count) {
                        selectedCollection.Stats.RemoveAt(SelectedStatIndex--);
                        if (SelectedStatIndex == -1 && selectedCollection.Stats.Count > 0) {
                            SelectedStatIndex = 0;
                        }
                    }
                }
                GUILayout.Label("", EditorStyles.toolbarButton, GUILayout.Width(15));
                GUILayout.EndHorizontal();
            }
        }

        
    }
}
