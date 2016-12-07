using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Text;
using RPGSystems.StatSystem.Database;

namespace RPGSystems.StatSystem.Editor {
    [InitializeOnLoad]
    public class RPGStatCategoryGenerator : MonoBehaviour {
        const string defaultGenerationPath = "Assets/RPGSystems/Stats/Databases/StatCategory/";
        const string defaultFilename = "RPGStatCategory.cs";

        /// <summary>
        /// Check if the file already exists, if no file is found
        /// create a new path in the folder above where the 
        /// RPGStatCategoryGenerator script is located. Then write
        /// the database to the file.
        /// </summary>
        static public void CheckAndGenerateFile() {
            // Get the file path for the RPGStatCategory.cs
            string assetPath = GetAssetPathForFile(defaultFilename);

            // If the assetPath is empty no file exists, generate new file.
            if (string.IsNullOrEmpty(assetPath)) {
                string genAssetPath = GetAssetPathForFile("RPGStatCategoryGenerator.cs");
                assetPath = genAssetPath.Replace("Editor/RPGStatCategoryGenerator.cs", defaultFilename);
            }

            // Write current RPGStatCategoryDatabase to the file
            WriteStatCategorysToFile(assetPath);
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
        /// Writes the framework for the RPGStatCategory enum along with
        /// all the values from the RPGStatCategoryDatabase.
        /// </summary>
        static public void WriteStatCategorysToFile(string filePath) {
            using (StreamWriter file = File.CreateText(filePath)) {
                file.WriteLine("/// <summary>");
                file.WriteLine("/// GENERATED FILE! ANY EDITS WILL BE LOST ON NEXT GENERATION!\n///");
                file.WriteLine("/// Generated Enum that can be used to easily select");
                file.WriteLine("/// a StatCategory from the StatCategoryDatabase. The value assigned");
                file.WriteLine("/// to each enum key is the Id of that StatCategory within the");
                file.WriteLine("/// RPGStatCategoryDatabase.");
                file.WriteLine("/// </summary>");


                file.WriteLine("namespace RPGSystems.StatSystem {");
                file.WriteLine("\tpublic enum RPGStatCategory {");

                foreach(var asset in RPGStatCategoryDatabase.Instance.GetAssets()) {
                    string statName = asset.Name.Replace(" ", string.Empty);
                    if (string.IsNullOrEmpty(statName)) {
                        statName = string.Format("StatCategory{0}", asset.Id.ToString());
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
