using System;
using UnityEngine;

namespace RPGSystems.StatSystem.Database {
    static public class RPGStatUtility {
        static public RPGStatAsset CreateAssetOfType(string statAssetType) {
            if (typeof(RPGStatAsset).Name == statAssetType) {
                return new RPGStatAsset();
            } else if (typeof(RPGAttributeAsset).Name == statAssetType) {
                return new RPGAttributeAsset();
            } else if (typeof(RPGStatModifiable).Name == statAssetType) {
                return new RPGStatModifiableAsset();
            } else if (typeof(RPGVitalAsset).Name == statAssetType) {
                return new RPGVitalAsset();
            }
            return null;
        }
    }
}