using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Text;
using RPGSystems.StatSystem.Database;

namespace RPGSystems.StatSystem.Editor {
    [InitializeOnLoad]
    public class RPGStatTypeGenerator : MonoBehaviour {
        const string defaultGenerationPath = "Assets/RPGSystems/Stats/Databases/StatType/";
        const string defaultFilename = "RPGStatType.cs";

        /// <summary>
        /// Check if the file already exists, if no file is found
        /// create a new path in the folder above where the 
        /// RPGStatTypeGenerator script is located. Then write
        /// the database to the file.
        /// </summary>
        static public void CheckAndGenerateFile() {
            // Get the file path for the RPGStatType.cs
            string assetPath = GetAssetPathForFile(defaultFilename);

            // If the assetPath is empty no file exists, generate new file.
            if (string.IsNullOrEmpty(assetPath)) {
                string genAssetPath = GetAssetPathForFile("RPGStatTypeGenerator.cs");
                assetPath = genAssetPath.Replace("Editor/RPGStatTypeGenerator.cs", defaultFilename);
            }

            // Write current RPGStatTypeDatabase to the file
            WriteStatTypesToFile(assetPath);
        }

        /// <summary>
        /// Gets the asset path for a file that contains a string.
        /// returns the first result found.
        /// </summary>
        static string GetAssetPathForFile(string name) {
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string assetPath in assetPaths) {
                if (assetPath.Contains(name)) {
                    return assetPath;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the framework for the RPGStatType enum along with
        /// all the values from the RPGStatTypeDatabase.
        /// </summary>
        static public void WriteStatTypesToFile(string filePath) {
            using (StreamWriter file = File.CreateText(filePath)) {
                file.WriteLine("/// <summary>");
                file.WriteLine("/// GENERATED FILE! ANY EDITS WILL BE LOST ON NEXT GENERATION!\n///");
                file.WriteLine("/// Generated Enum that can be used to easily select");
                file.WriteLine("/// a StatType from the StatTypeDatabase. The value assigned");
                file.WriteLine("/// to each enum key is the Id of that statType within the");
                file.WriteLine("/// RPGStatTypeDatabase.");
                file.WriteLine("/// </summary>");


                file.WriteLine("namespace RPGSystems.StatSystem {");
                file.WriteLine("\tpublic enum RPGStatType {");
                file.WriteLine("\t\tNone = 0,");

                foreach(var asset in RPGStatTypeDatabase.Instance.GetAssets()) {
                    string statName = asset.Name.Replace(" ", string.Empty);
                    if (string.IsNullOrEmpty(statName)) {
                        statName = string.Format("StatType{0}", asset.Id.ToString());
                    }

                    file.WriteLine(string.Format("\t\t{0} = {1},",
                        statName, asset.Id));
                }

                file.WriteLine("\t}\n}");
            }
            AssetDatabase.Refresh();
        }
    }
}
