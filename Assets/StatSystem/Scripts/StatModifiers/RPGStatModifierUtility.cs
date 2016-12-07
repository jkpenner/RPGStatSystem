namespace RPGSystems.StatSystem {
    static public class RPGStatModifierUtility {
        static public RPGStatModifierAsset CreateAsset(string type) {
            if (typeof(RPGStatModBaseAdd).Name == type) {
                return new RPGStatModBaseAddAsset();
            } else if (typeof(RPGStatModBasePercentAsset).Name == type) {
                return new RPGStatModBasePercentAsset();
            } else if (typeof(RPGStatModTotalAddAsset).Name == type) {
                return new RPGStatModTotalAddAsset();
            } else if (typeof(RPGStatModTotalPercentAsset).Name == type) {
                return new RPGStatModTotalPercentAsset();
            }
            return null;
        }
    }
}