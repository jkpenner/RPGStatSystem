using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public class RPGStatModBaseAddAsset : RPGStatModifierAsset {
        public override RPGStatModifier CreateInstance() {
            return Internal_CreateInstance<RPGStatModBaseAdd>();
        }
    }
}
