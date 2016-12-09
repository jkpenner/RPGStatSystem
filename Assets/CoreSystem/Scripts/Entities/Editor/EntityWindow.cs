using UnityEditor;
using UnityEngine;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;

public class EntityWindow : XmlDatabaseWindowComplex<EntityAsset> {
    [MenuItem("Window/RPGSystems/Entity/Entity Editor")]
    static public void ShowWindow() {
        EntityWindow wnd = GetWindow<EntityWindow>();
        wnd.titleContent.text = "Entity";
        wnd.Show();
    }


    protected override AbstractXmlDatabase<EntityAsset> GetDatabaseInstance() {
        return EntityDatabase.Instance;
    }

    protected override EntityAsset CreateDefaultAsset() {
        return new EntityAsset(GetDatabaseInstance().GetNextHighestId());
    }

    protected override void DisplayAssetGUI(EntityAsset asset) {
        GUILayout.BeginHorizontal("Box");
        GUILayout.Label("ID: " + asset.Id.ToString("D4") + ", Name ", GUILayout.Width(100));
        asset.Name = EditorGUILayout.TextField(asset.Name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Prefab", GUILayout.Width(80));
        asset.Prefab = (GameObject)EditorGUILayout.ObjectField(asset.Prefab, typeof(GameObject), false);
        GUILayout.EndHorizontal();
    }
}
