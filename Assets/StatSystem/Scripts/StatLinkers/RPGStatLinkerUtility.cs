using System;

namespace RPGSystems.StatSystem {
    static public class RPGStatLinkerUtility {
        static public RPGStatLinkerAsset CreateAsset(string statAssetType) {
            if (typeof(RPGStatLinkerBasicAsset).Name == statAssetType) {
                return new RPGStatLinkerBasicAsset();
            }
            return null;
        }
    }
}