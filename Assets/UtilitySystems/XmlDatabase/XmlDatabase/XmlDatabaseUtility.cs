using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

static public class XmlDatabaseUtility {
    static public string GetAssetResourcePath(UnityEngine.Object asset) {
#if UNITY_EDITOR
        if (asset == null) { return string.Empty; }

        string dir = AssetDatabase.GetAssetPath(asset);

        // Remove the extension of the asset
        dir = dir.Split(new char[] { '.' })[0];

        bool foundResources = false;
        string newDir = "";
        var folders = dir.Split(new char[] { '\\', '/' });
        for (int i = 0; i < folders.Length; i++) {
            if (foundResources == true) {
                newDir += folders[i];
                if (i != folders.Length - 1) {
                    newDir += "/";
                }
            }

            if (folders[i] == "Resources") {
                foundResources = true;
            }
        }

        if (foundResources == false) {
            Debug.LogErrorFormat("Asset {0} is not placed into a resources folder will not load correctly", asset.name);
        }

        return newDir;
#else
            return string.Empty;
#endif
    }
}
