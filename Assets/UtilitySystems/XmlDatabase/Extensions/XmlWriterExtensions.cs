using System.Xml;
using UnityEngine;

namespace UtilitySystems.XmlDatabase {
    static public class XmlExtensions {
        static public void SetAttr(this XmlWriter writer, string name, int value) {
           writer.WriteAttributeString(name, value.ToString());
        }

        static public void SetAttr(this XmlWriter writer, string name, float value) {
            writer.WriteAttributeString(name, value.ToString());
        }

        static public void SetAttr(this XmlWriter writer, string name, bool value) {
            writer.WriteAttributeString(name, value.ToString());
        }

        static public void SetAttr(this XmlWriter writer, string name, string value) {
            writer.WriteAttributeString(name, value);
        }

        static public void SetAttr(this XmlWriter writer, string name, GameObject value) {
            writer.WriteAttributeString(name, XmlDatabaseUtility.GetAssetResourcePath(value));
        }
    }
}