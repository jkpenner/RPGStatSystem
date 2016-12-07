using System.Xml;
using UtilitySystems.XmlDatabase;

namespace RPGSystems.StatSystem {
    public class RPGStatAsset : IXmlOnSaveAsset, IXmlOnLoadAsset {
        public int StatBaseValue { get; set; }
        public int StatCategoryId { get; set; }
        public int AssignedStatId { get; set; }

        public virtual RPGStat CreateInstance() {
            return new RPGStat(this);
        }

        public virtual void OnSaveAsset(XmlWriter writer) {
            writer.SetAttr("BaseValue", StatBaseValue);
            writer.SetAttr("CategoryId", StatCategoryId);
            writer.SetAttr("AssignedStatId", AssignedStatId);
        }

        public virtual void OnLoadAsset(XmlReader reader) {
            switch (reader.Name) {
                case "Stat":
                    StatBaseValue = reader.GetAttrInt("BaseValue", 0);
                    StatCategoryId = reader.GetAttrInt("CategoryId", 0);
                    AssignedStatId = reader.GetAttrInt("AssignedStatId", 0);
                    break;
            }
        }
    }
}