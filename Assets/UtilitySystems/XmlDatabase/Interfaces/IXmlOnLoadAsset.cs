using System.Xml;

namespace UtilitySystems.XmlDatabase {
    public interface IXmlOnLoadAsset {
        void OnLoadAsset(XmlReader reader);
    }
}