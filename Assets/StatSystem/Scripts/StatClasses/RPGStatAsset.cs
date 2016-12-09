using System.Xml;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    public class RPGStatAsset : IXmlOnSaveAsset, IXmlOnLoadAsset {
        public int StatBaseValue { get; set; }
        public int StatCategoryId { get; set; }
        public int AssignedStatId { get; set; }
        public RPGStatScalerAsset StatScaler { get; set; }

        public virtual RPGStat CreateInstance() {
            return new RPGStat(this);
        }

        public virtual void OnSaveAsset(XmlWriter writer) {
            writer.SetAttr("BaseValue", StatBaseValue);
            writer.SetAttr("CategoryId", StatCategoryId);
            writer.SetAttr("AssignedStatId", AssignedStatId);

            if (StatScaler != null) {
                writer.WriteStartElement("StatScaler");
                writer.SetAttr("Type", StatScaler.GetType().Name);
                StatScaler.OnSaveAsset(writer);
                writer.WriteEndElement();
            }
        }

        public virtual void OnLoadAsset(XmlReader reader) {
            switch (reader.Name) {
                case "Stat":
                    StatBaseValue = reader.GetAttrInt("BaseValue", 0);
                    StatCategoryId = reader.GetAttrInt("CategoryId", 0);
                    AssignedStatId = reader.GetAttrInt("AssignedStatId", 0);
                    break;
                case "StatScaler":
                    var type = reader.GetAttrString("Type", "");
                    StatScaler = RPGStatScalerUtility.CreateAsset(type);
                    if (StatScaler != null) {
                        StatScaler.OnLoadAsset(reader);
                    }
                    break;
            }
        }
    }
}