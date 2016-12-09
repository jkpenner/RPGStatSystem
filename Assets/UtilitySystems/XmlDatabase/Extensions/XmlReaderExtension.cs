using UnityEngine;
using System.Xml;

namespace UtilitySystems.XmlDatabase {
    static public class XmlReaderExtension {
        static public string GetAttrString(this XmlReader reader, string name, string defaultValue) {
            string value = reader.GetAttribute(name);
            if (value == string.Empty) {
                return defaultValue;
            }
            return value;
        }

        static public int GetAttrInt(this XmlReader reader, string name, int defaultValue) {
            string strValue = reader.GetAttribute(name);
            if (strValue == string.Empty) {
                return defaultValue;
            }

            int value;
            if (int.TryParse(strValue, out value)) {
                return value;
            }

            return defaultValue;
        }

        static public float GetAttrFloat(this XmlReader reader, string name, float defaultValue) {
            string strValue = reader.GetAttribute(name);
            if (strValue == string.Empty) {
                return defaultValue;
            }

            float value;
            if (float.TryParse(strValue, out value)) {
                return value;
            }

            return defaultValue;
        }

        static public bool GetBoolAttribute(this XmlReader reader, string name, bool defaultValue) {
            string strValue = reader.GetAttribute(name);
            if (strValue == string.Empty) {
                return defaultValue;
            }

            bool value;
            if (bool.TryParse(strValue, out value)) {
                return value;
            }

            return defaultValue;
        }

        static public EnumType GetAttrEnum<EnumType>(this XmlReader reader, string name, EnumType defaultValue) {
            string strValue = reader.GetAttribute(name);
            if (strValue == string.Empty) {
                return defaultValue;
            }

            try {
                var parseValue = System.Enum.Parse(typeof(EnumType), strValue);
                if (System.Enum.IsDefined(typeof(EnumType), parseValue)) {
                    return (EnumType)parseValue;
                }
                return defaultValue;
            } catch {
                Debug.Log("Error occured whil parsing enum value. Returning default value.");
                return defaultValue;
            }
        }

        static public GameObject GetAttrGameObject(this XmlReader reader, string name) {
            string path = GetAttrString(reader, name, "");
            if (!string.IsNullOrEmpty(path)) {
                GameObject go = Resources.Load<GameObject>(path);
                if (go != null) {
                    return go;
                } else {
                    Debug.LogWarning("No GameObject found at path: " + path);
                }
            }
            return null;
        }

        static public T GetAttrResource<T>(this XmlReader reader, string name) where T : UnityEngine.Object{
            string path = GetAttrString(reader, name, "");
            if (!string.IsNullOrEmpty(path)) {
                var go = Resources.Load<T>(path);
                if (go != null) {
                    return go;
                } else {
                    //Debug.LogWarning("No Resource not found at path: " + path);
                }
            }
            return null;
        }
    }
}
