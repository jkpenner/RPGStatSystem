using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public class RPGStatModTotalPercentAsset : RPGStatModifierAsset {
        public override RPGStatModifier CreateInstance() {
            return Internal_CreateInstance<RPGStatModTotalPercent>();
        }

        /* Do not need to implement, RPGStatModTotalPercent does not have any data set
        protected override T Internal_CreateInstance<T>() {
            var mod = base.Internal_CreateInstance<T>() as RPGStatModTotalPercent;
            // Add any initialization code here

            return mod as T;
        }
        */
    }
}
