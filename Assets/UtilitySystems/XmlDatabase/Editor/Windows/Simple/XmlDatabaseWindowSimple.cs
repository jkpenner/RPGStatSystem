using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace UtilitySystems.XmlDatabase.Editor {
    abstract public class XmlDatabaseWindowSimple<DatabaseAssetType> 
        : XmlDatabaseWindow<DatabaseAssetType> where DatabaseAssetType 
        : class, IXmlDatabaseAsset, new() {

        private Vector2 selectorScroll = Vector2.zero;
        
        public void OnGUI() {
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

            GUILayout.BeginVertical();

            // Scroll view for the listed assets
            selectorScroll = GUILayout.BeginScrollView(selectorScroll, false, true);

            // List all the assets in the database
            GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));

            var database = GetDatabaseInstance();
            foreach (var asset in database.GetAssets()) {
                if (asset != null) {
                    if (ShowSearchOption && string.IsNullOrEmpty(searchText) == false) {
                        if (!DoesAssetMatchSearchString(asset, searchText)) {
                            continue;
                        }
                    }

                    DisplayAssetHeaderGUI(asset.Id, asset);

                    if (SelectedAssetId == asset.Id) {
                        GUILayout.BeginVertical("Box");
                        DisplayAssetGUI(asset);
                        GUILayout.EndVertical();
                    }
                }
            }

            if (database.GetAssetCount() == 0) {
                GUILayout.Label("No assets in database.\nClick 'Add New' to create an asset.", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            DisplayGUIFooter();

            GUILayout.Space(2);
            GUILayout.EndVertical();

            InvokeActionQueue();
        }

        private void DisplayAssetHeaderGUI(int id, DatabaseAssetType asset) {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(string.Format("Id: {0}", asset.Id.ToString("D3")), GUILayout.Width(60));

            var clicked = GUILayout.Toggle(asset.Id == SelectedAssetId, asset.Name, ToggleButtonStyle);
            if (clicked != (asset.Id == SelectedAssetId)) {
                if (clicked) {
                    SelectedAssetId = asset.Id;
                } else {
                    SelectedAssetId = -1;
                }
            }

            if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(30))) {
                OnRemoveAssetClick(asset.Id);
            }
            GUILayout.EndHorizontal();
        }

        public virtual void DisplayGUIFooter() {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New", EditorStyles.toolbarButton)) {
                OnAddNewAssetClick();
            }

            DisplayLoadButton();
            DisplaySaveButton();
            
            GUILayout.EndHorizontal();
        }
    }
}
