using UnityEngine;
using UnityEditor;

namespace UtilitySystems.XmlDatabase.Editor {
    public abstract class XmlDatabaseWindowComplexExt<DatabaseAssetType>
        : XmlDatabaseWindowComplex<DatabaseAssetType>
        where DatabaseAssetType : class, IXmlDatabaseAsset, new() {

        private IEditorExtension[] extensions;
        protected abstract IEditorExtension[] GetExtensions();

        protected virtual void OnEnable() {
            extensions = GetExtensions();
            foreach (var extension in extensions) {
                extension.OnEnable();
            }
        }

        protected virtual void OnDisable() {
            foreach (var extension in extensions) {
                extension.OnDisable();
            }
            extensions = null;
        }

        protected override void DisplayAssetGUI(DatabaseAssetType asset) {
            GUILayout.BeginHorizontal("Box");
            GUILayout.Label(string.Format("Id: {0} Name:", asset.Id), GUILayout.Width(EditorGUIUtility.labelWidth));
            asset.Name = EditorGUILayout.TextField(asset.Name);
            GUILayout.EndHorizontal();

            foreach (var extension in extensions) {
                if (extension.CanHandleType(asset.GetType())) {
                    extension.OnGUI(asset);
                }
            }
        }
    }
}
