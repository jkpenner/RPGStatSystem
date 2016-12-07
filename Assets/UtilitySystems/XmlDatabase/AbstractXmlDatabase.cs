using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace UtilitySystems.XmlDatabase {
    /// <summary>
    /// Abstract Xml Database with that can create and save a database to an xml file.
    /// </summary>
    public abstract class AbstractXmlDatabase<T> where T : class, IXmlDatabaseAsset {

        /// <summary>
        /// Creates a instance of an asset given a string of the asset type
        /// </summary>
        public abstract T CreateAssetOfType(string type);

        public virtual string[] GetListOfAssetTypes() { return null; }

        /// <summary>
        /// The path to the database file within the StreamingAssets folder.
        /// Example: Folder1/DatabaseFolder/
        /// </summary>
        public abstract string DatabasePath { get; }

        /// <summary>
        /// The name of the database file. Example: database.xml
        /// </summary>
        public abstract string DatabaseName { get; }

        /// <summary>
        /// Dictionary containing all loaded assets with their ids as keys
        /// </summary>
        protected Dictionary<int, T> AssetDict { get; private set; }

        public Dictionary<int, T>.ValueCollection GetAssets() {
            return AssetDict.Values;
        }

        public AbstractXmlDatabase() {
            AssetDict = new Dictionary<int, T>();
        }

        /// <summary>
        /// Add an asset to the database
        /// </summary>
        public void Add(T obj) {
            if (obj != null) {
                if (AssetDict.ContainsKey(obj.Id) == false) {
                    AssetDict.Add(obj.Id, obj);
                } else {
                    Debug.LogWarningFormat("[{0}]: Attempting to add asset {1} with Id {2}, but id is already assigned to {3}. " +
                        "Asset will not be added to the database.", DatabaseName, obj.Name, obj.Id, AssetDict[obj.Id]);
                }
            }
        }

        public T Get(int assetId) {
            return Get(assetId, true);
        }

        /// <summary>
        /// Get the asset with the given id
        /// </summary>
        public T Get(int assetId, bool tryToLoadAsset) {
            T asset;
            if (TryGet(assetId, out asset, tryToLoadAsset)) {
                return asset;
            }
            return default(T);
        }

        /// <summary>
        /// Checks the Instance of the database for an asset with the given Id. Also can
        /// try to load the asset from the database file if no asset is initially found.
        /// </summary>
        /// <param name="assetId">Id of asset</param>
        /// <param name="asset">Assigned to if asset is found</param>
        /// <param name="loadAssetIfMissing">Try to load asset from file if not currently loaded</param>
        /// <returns>If an asset with the passed Id was found</returns>
        public bool TryGet(int assetId, out T asset, bool loadAssetIfMissing = false) {
            // Check if asset is already in the database
            if (AssetDict.ContainsKey(assetId)) {
                asset = AssetDict[assetId];
                return true;
            }

            // Attempt to load the asset, if not in database
            if (loadAssetIfMissing == true) {
                asset = LoadAsset(assetId);
                // If asset was loaded successfully
                // add the asset to the database
                if (asset != null) {
                    Add(asset);
                }
            } else {
                asset = null;
            }

            // Was an asset found
            return asset != null;
        }

        /// <summary>
        /// Get the Next Id from the asset dict after the highest id value.
        /// </summary>
        public int GetNextHighestId() {
            int maxId = 0;
            foreach (var asset in AssetDict.Values) {
                if (asset.Id > maxId) {
                    maxId = asset.Id;
                }
            }
            return maxId + 1;
        }


        /// <summary>
        /// Gets the first available id from the asset dict.
        /// </summary>
        public int GetFirstAvailableId() {
            if (GetAssetCount() <= 0) {
                return 1;
            } else {
                int targetId = 1;
                bool foundUsableId = false;
                while (!foundUsableId) {
                    foundUsableId = true;
                    foreach (var asset in AssetDict.Values) {
                        if (asset.Id == targetId) {
                            foundUsableId = false;
                            targetId++;
                            break;
                        }
                    }
                }
                return targetId;
            }
        }

        /// <summary>
        /// Get the number of assets in the database
        /// </summary>
        /// <returns></returns>
        public int GetAssetCount() {
            return AssetDict.Count;
        }

        /// <summary>
        /// Remove the asset with the given id
        /// </summary>
        public void Remove(int id) {
            if (AssetDict.ContainsKey(id)) {
                AssetDict.Remove(id);
            }
        }

        /// <summary>
        /// Replaces the asset at the given index with a different asset
        /// </summary>
        public void Set(int id, T obj) {
            if (AssetDict.ContainsKey(id)) {
                AssetDict[id] = obj;
                obj.Id = id;
            } else {
                obj.Id = id;
                AssetDict.Add(id, obj);
            }
        }

        /// <summary>
        /// Reads a single asset from the xml reader
        /// </summary>
        private T ReadAsset(XmlReader reader) {
            // If the reader is not at an Asset element return the default value
            if (reader.IsStartElement("Asset") == false) return default(T);

            // If the asset does not have any attributes return;
            if (reader.HasAttributes == false) {
                Debug.LogWarningFormat("[{0}]: Found Asset with no assigned attributes.", this.DatabaseName);
                return default(T);
            }

            // Assign default values to read asset values
            int id = -1;
            string name = string.Empty;
            string type = string.Empty;

            // Check if the assest has a id value, Id is a required attribute
            if (reader["Id"] != null) {
                id = int.Parse(reader.GetAttribute("Id"));
            } else {
                Debug.LogErrorFormat("[{0}]: Asset found with no Id assigned. Id attribute is required!", this.DatabaseName);
                return default(T);
            }

            // Check if the asset has a type assigned, Type is a required attribute
            if (reader["Type"] != null) {
                type = reader.GetAttribute("Type");
            } else {
                Debug.LogErrorFormat("[{0}]: Asset found with no Type assigned. Type attribute is required!", this.DatabaseName);
                return default(T);
            }

            // Check if the asset has a name assigned, Name is not a required attribute
            if (reader["Name"] != null) {
                name = reader.GetAttribute("Name");
            } else {
                Debug.LogWarningFormat("[{0}]: Found Asset with no Name assigned.", this.DatabaseName);
            }

            // Attempt to create an instance of the read type
            var asset = CreateAssetOfType(type);
            if (asset == null) {
                Debug.LogErrorFormat("[{0}]: Found Asset with an unhandled type of {1}, assigned to " +
                    "asset with an Id of {2}.", this.DatabaseName, type, id);
                return default(T);
            }

            // Assign the read Id and Name values
            asset.Id = id;
            asset.Name = name;

            // If the asset element is empty return the created asset.
            // Note: Assets can add additional attributes to the Asset 
            // start element, but this attributes will never be loaded.
            if (reader.IsEmptyElement == true) return asset;

            //XmlSerializer serializer = new XmlSerializer(asset.GetType());
            //
            //var deserializedAsset = (XmlDatabaseAsset)serializer.Deserialize(reader.ReadSubtree());

            // read the asset until we reach the end element
            while (reader.Read()) {
                // Stop reading the asset when we reach the Asset end element
                if (reader.Name == "Asset" && reader.NodeType == XmlNodeType.EndElement) {
                    break;
                }

                // Check if we are reading an element of the asset
                if (reader.NodeType == XmlNodeType.Element) {
                    // Make the object load it's data
                    asset.OnLoadAsset(reader);
                }
            }

            // Return the asset as the database's asset type
            return asset as T;
        }
        
        /// <summary>
        /// Loads all asset into the database.
        /// Clears any assets already in the database.
        /// </summary>
        public void LoadDatabase() {
            LoadAssetsIntoDatabase(true);
        }

        /// <summary>
        /// Loads all assets into the database. Has 
        /// option to clear assets already in the database.
        /// 
        /// defaultly does not override values within the database. If
        /// overrideValues is true and a item is loaded with the same
        /// id the new item overrides the old item.
        /// </summary>
        public void LoadAssetsIntoDatabase(bool clear, bool overrideValues = false) {
            // Remove previous asset from database if clear is true
            if (clear == true) {
                AssetDict.Clear();
                AssetDict = LoadAllAssets();
            } else {
                var assetsToLoad = LoadAllAssets();
                foreach (var asset in assetsToLoad) {
                    if (AssetDict.ContainsKey(asset.Key)) {
                        if (overrideValues == true) {
                            AssetDict[asset.Key] = asset.Value;
                        }
                    } else {
                        AssetDict.Add(asset.Key, asset.Value);
                    }
                }

            }
        }

        /// <summary>
        /// Finds the asset with the given id in the database's xml file,
        /// then loads and returns the asset if found. Does not add loaded asset
        /// to the database instance.
        /// </summary>
        public T LoadAsset(int assetId) {
            CreateDatabaseIfMissing();

            // Get the database file stream
            using (Stream stream = GetFileStreamToLoad()) {

                // If no file is found return the default value
                if (stream == null) {
                    Debug.LogFormat("[{0}]: Could not load asset. No load stream found.", DatabaseName);
                    return default(T);
                }

                // ToDo: Decrypt the database file if needed

                // Get a xml reader from the file stream
                using (XmlReader reader = XmlReader.Create(stream)) {
                    // Read through the xml file till we find an asset element
                    while (reader.Read()) {
                        if (reader.IsStartElement("Asset")) {
                            // Check if asset has an Id value
                            if (reader["Id"] != null) {
                                // If the asset element has a matching id, read the asset
                                if (int.Parse(reader.GetAttribute("Id")) == assetId) {
                                    return ReadAsset(reader);
                                }
                            } else {
                                Debug.LogErrorFormat("[{0}]: Asset found with no Id assigned. Id attribute is required!", this.DatabaseName);
                                return default(T);
                            }
                        }
                    }
                }
            }

            // No asset was found in the database
            return default(T);
        }

        /// <summary>
        /// Loads all assets from the database's xml file and returns a
        /// dictionary with all loaded assets with keys set to the assets' id.
        /// Does not add loaded asset to the database instance.
        /// </summary>
        public Dictionary<int, T> LoadAllAssets() {
            CreateDatabaseIfMissing();

            Dictionary<int, T> loadedAssets = new Dictionary<int, T>();

            // Get the database file stream
            using (Stream stream = GetFileStreamToLoad()) {
                if (stream == null) {
                    Debug.LogFormat("[{0}]: Could not load assets. No load stream found.", DatabaseName);
                    return null;
                }

                // ToDo: Decrypt the database file if needed

                // Get a xml reader from the file stream
                using (XmlReader reader = XmlReader.Create(stream)) {
                    // Read through the xml file till we find an asset element
                    while (reader.Read()) {
                        if (reader.IsStartElement("Asset")) {
                            // Read the asset and add it to the database
                            var asset = ReadAsset(reader);
                            if (asset != null) {
                                if (loadedAssets.ContainsKey(asset.Id)) {
                                    Debug.LogWarningFormat("[{0}]: Asset {1} has Id {2}, but id is already assigned to {3}",
                                        DatabaseName, asset.Name, asset.Id, loadedAssets[asset.Id]);
                                } else {
                                    loadedAssets.Add(asset.Id, asset);
                                }
                            } else {
                                Debug.LogWarningFormat("[{0}]: Read Invalid Asset", this.DatabaseName);
                            }
                        }
                    }
                }
            }

            return loadedAssets;
        }


        /// <summary>
        /// Saves all assets in the database to the database xml file,
        /// overrides the old values within the database's xml.
        /// </summary>
        public void SaveAssets() {
            CreateDatabaseIfMissing();

            // Create xml writer settings
            var settings = new XmlWriterSettings();
            settings.Indent = true;

            // Create an xml file at the database path
            using (var stream = File.Create(GetDatabaseFullPath())) {
                using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Assets");

                    // Write All Assets in the database to file
                    foreach (var asset in AssetDict.Values) {
                        if (asset == null) {
                            Debug.LogWarningFormat("[{0}]: Asset is null. Skipping", DatabaseName);
                            continue;
                        }

                        // Write the start Asset element and attributes
                        writer.WriteStartElement("Asset");
                        writer.WriteAttributeString("Id", asset.Id.ToString());
                        writer.WriteAttributeString("Name", asset.Name);

                        // Check if the perferred type string is empty, if so
                        // then use the name of the asset's type as the value.
                        if (string.IsNullOrEmpty(asset.PerferredTypeString)) {
                            writer.WriteAttributeString("Type", asset.GetType().Name);
                        } else {
                            writer.WriteAttributeString("Type", asset.PerferredTypeString);
                        }

                        // Make the object save it's values
                        asset.OnSaveAsset(writer);

                        // Write the end Asset Element
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }

            // ToDo: Encrypt the database file if needed

#if UNITY_EDITOR
            // Editor only: Save the asset and refresh the editor
            // to display the newly created asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Checks if the passed string matches the typeparam's Name or
        /// if the string matches the typeparam's Name without an Asset
        /// postfix, if the typeparam contains a posfix Asset.
        /// 
        /// Examples: If U is ItemAsset, the method will return true if
        /// the passed string is 'ItemAsset' or 'Item'.
        /// </summary>
        protected bool DoesStringMatchType<U>(string value) where U : IXmlDatabaseAsset {
            string assetTypeName = typeof(U).Name;

            // Check if the value matches the passed type's name
            if (assetTypeName == value) {
                return true;
            }

            // Check if the type's name has an Asset postfix
            if (assetTypeName.Contains("Asset")) {
                return DoesStringMatchTypeWithout<U>(value, "Asset");
            }

            return false;
        }

        /// <summary>
        /// Replaces any occurances of the strToRemove from the passed value,
        /// then checks if the typeparam's name is equal to the passed value.
        /// </summary>
        protected bool DoesStringMatchTypeWithout<U>(string value, string strToRemove) {
            var valueToCheck = typeof(U).Name.Replace(strToRemove, "");
            return valueToCheck == value;
        }

        /// <summary>
        /// Get the database path based off the Streaming Assets folder in the Application's database 
        /// </summary>
        protected string GetDatabaseFullPath() {
            return string.Format(@"{0}/StreamingAssets/{1}{2}", Application.dataPath, DatabasePath, DatabaseName);
        }

        /// <summary>
        /// Get the file stream to read the xml from
        /// </summary>
        protected Stream GetFileStreamToLoad() {
            return File.OpenRead(GetDatabaseFullPath());
        }

        /// <summary>
        /// Check if the database exists. If the database is missing
        /// create a new xml file with default values.
        /// </summary>
        private void CreateDatabaseIfMissing() {
            if (!File.Exists(GetDatabaseFullPath())) {
                //Debug.LogFormat(@"[{0}]: No database found at {1}. Creating a new empty database file.", DatabaseName, GetDatabaseFullPath());
                Directory.CreateDirectory(string.Format("{0}/StreamingAssets/{1}", Application.dataPath, DatabasePath));

                // Write the default xml file
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                using (Stream newFileStream = File.Create(GetDatabaseFullPath())) {
                    //using (var stringWriter = new StringWriter()) {
                    using (XmlWriter writer = XmlWriter.Create(newFileStream, settings)) {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Assets");

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    //File.WriteAllText(GetDatabaseFullPath(), stringWriter.ToString());
                }

#if UNITY_EDITOR
                // Editor only: Save the asset and refresh the editor
                // to display the newly created asset
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
        }
    }
}