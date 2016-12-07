using System.Xml;

namespace UtilitySystems.XmlDatabase {
    public interface IXmlOnSaveAsset {
        void OnSaveAsset(XmlWriter writer);
    }
}