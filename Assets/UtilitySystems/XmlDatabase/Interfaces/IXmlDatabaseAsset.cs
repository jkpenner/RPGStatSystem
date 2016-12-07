using System.Xml;

namespace UtilitySystems.XmlDatabase {
    public interface IXmlDatabaseAsset {
        int Id { get; set; }
        string Name { get; set; }

        string PerferredTypeString { get; set; }

        void OnSaveAsset(XmlWriter writer);
        void OnLoadAsset(XmlReader reader);
    }
}
