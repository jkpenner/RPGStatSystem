using UnityEngine;
using UnityEditor;

namespace UtilitySystems.XmlDatabase.Editor {
    public abstract class XmlDatabaseWindowComplex<DatabaseAssetType> 
        : XmlDatabaseWindow<DatabaseAssetType> where DatabaseAssetType 
        : class, IXmlDatabaseAsset, new(){

        private float selectorWidth = 200;
        private Vector2 selectorScroll = Vector2.zero;

        private void OnGUI() {
            if (ShowSearchOption || ShowFilterOption) {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                GUILayout.FlexibleSpace();
                if (ShowSearchOption) {
                    GUILayout.Label("Search", GUILayout.Width(80));
                    searchText = EditorGUILayout.TextField(searchText,
                        EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));

                    if (GUILayout.Button("X", EditorStyles.toolbarButton, GUILayout.Width(24))) {
                        searchText = string.Empty;
                        GUI.FocusControl(null);
                    }
                }
                if (ShowFilterOption) {
                    isFilterOptionsVisible = GUILayout.Toggle(isFilterOptionsVisible, "Filters",
                        EditorStyles.toolbarButton, GUILayout.Width(80));
                }
                GUILayout.EndHorizontal();

                if (ShowFilterOption && isFilterOptionsVisible) {
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("Help");
                    GUILayout.EndVertical();

                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(selectorWidth));
            DisplaySelector();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            DisplayContent();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            DisplayFooter();

            InvokeActionQueue();
        }

        protected virtual void OnDisplayDatabaseAssets(int selectedIndex) {
            var database = GetDatabaseInstance();

            foreach (var asset in database.GetAssets()) {
                if (ShowSearchOption && string.IsNullOrEmpty(searchText) == false) {
                    if (!DoesAssetMatchSearchString(asset, searchText)) {
                        continue;
                    }
                }

                bool isVisible = GUILayout.Toggle(asset.Id == SelectedAssetId,
                    string.Format("[{0}]: {1}", asset.Id.ToString("D4"),
                    (string.IsNullOrEmpty(asset.Name) ? "Unassigned Object" : asset.Name)),
                    ToggleButtonStyle);

                if (SelectedAssetId == asset.Id || isVisible) {
                    SelectedAssetId = asset.Id;
                }
            }
        }

        protected virtual void OnDisplayFooter() {
            DisplaySaveButton();
            DisplayLoadButton();

            GUILayout.FlexibleSpace();

            GUILayout.Label(string.Format("Assets: {0}",
                GetDatabaseInstance().GetAssetCount().ToString("D3")),
                EditorStyles.centeredGreyMiniLabel);
        }

        private void DisplaySelector() {
            // Scroll view for the listed assets
            selectorScroll = GUILayout.BeginScrollView(selectorScroll, false, true);

            // List all the assets in the database
            GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));

            OnDisplayDatabaseAssets(SelectedAssetId);

            if (GetDatabaseInstance().GetAssetCount() <= 0) {
                GUILayout.Label("No Assets in Database\nPress '+' to add an asset", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private void DisplayContent() {
            GUILayout.BeginVertical();
            var asset = GetDatabaseInstance().Get(SelectedAssetId);
            if (asset != null) {
                DisplayAssetGUI(asset);
            }
            GUILayout.EndVertical();
        }

        

        private void DisplayFooter() {
            // Show the add and remove selected buttons
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width((selectorWidth - 21) / 2))) {
                OnAddNewAssetClick();
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width((selectorWidth - 21) / 2))) {
                OnRemoveAssetClick(SelectedAssetId);
            }

            GUILayout.Label("", EditorStyles.toolbarButton, GUILayout.Width(15));

            OnDisplayFooter();

            GUILayout.EndHorizontal();

            GUILayout.Space(2);
        }
    }
}
