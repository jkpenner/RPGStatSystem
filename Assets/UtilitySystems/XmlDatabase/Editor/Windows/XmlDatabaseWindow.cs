using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace UtilitySystems.XmlDatabase.Editor {
    public abstract class XmlDatabaseWindow<DatabaseAssetType> 
        : EditorWindow where DatabaseAssetType 
        : class, IXmlDatabaseAsset, new() {

        protected abstract AbstractXmlDatabase<DatabaseAssetType> GetDatabaseInstance();
        

        protected abstract void DisplayAssetGUI(DatabaseAssetType asset);

        protected bool isFilterOptionsVisible;
        protected string searchText;

        protected bool ShowSearchOption { get { return true; } }
        protected bool ShowFilterOption { get { return true; } }

        private int _selectedAssetId = -1;
        public int SelectedAssetId {
            get {
                return _selectedAssetId;
            }
            set {
                if (_selectedAssetId != value) {
                    _selectedAssetId = value;
                    EditorGUI.FocusTextInControl(string.Empty);
                }
            }
        }

        private Queue<Action> actionQueue = null;
        public Queue<Action> ActionQueue {
            get {
                if (actionQueue == null) {
                    actionQueue = new Queue<Action>();
                }
                return actionQueue;
            }
        }

        private GUIStyle toggleButtonStyle;
        public virtual GUIStyle ToggleButtonStyle {
            get {
                if (toggleButtonStyle == null) {
                    // Create custom style for stat buttons
                    toggleButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                    toggleButtonStyle.alignment = TextAnchor.MiddleLeft;
                }
                return toggleButtonStyle;
            }
        }

        protected virtual void DisplaySaveButton() {
            if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60))) {
                if (EditorUtility.DisplayDialog("Save Database", "Save current data to the XML document?", "Save", "Cancel")) {
                    GetDatabaseInstance().SaveAssets();
                }
            }
        }

        protected virtual void DisplayLoadButton() {
            if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(60))) {
                if (EditorUtility.DisplayDialog("Load Database", "Loading values from the database will override all unsaved changes in the editor.", "Load", "Cancel")) {
                    GetDatabaseInstance().LoadDatabase();
                }
            }
        }

        protected virtual DatabaseAssetType CreateDefaultAsset() {
            var defaultAsset = new DatabaseAssetType();
            defaultAsset.Id = GetDatabaseInstance().GetNextHighestId();
            return defaultAsset;
        }

        protected virtual void OnAddNewAssetClick() {
            var newAsset = CreateDefaultAsset();
            SelectedAssetId = newAsset.Id;
            GetDatabaseInstance().Add(newAsset);
        }

        protected virtual void OnRemoveAssetClick(int id) {
            ActionQueue.Enqueue(() => {
                GetDatabaseInstance().Remove(id);
                EditorGUI.FocusTextInControl(string.Empty);
            });
        }

        protected void InvokeActionQueue() {
            while (ActionQueue.Count > 0) {
                ActionQueue.Dequeue().Invoke();
            }
        }

        protected bool DoesAssetMatchSearchString(DatabaseAssetType asset, string searchText) {
            return
                asset.Name.ToLower().Contains(searchText.ToLower()) ||
                asset.GetType().Name.ToLower().Contains(searchText.ToLower()) ||
                asset.Id.ToString().Contains(searchText);
        }
    }
}
