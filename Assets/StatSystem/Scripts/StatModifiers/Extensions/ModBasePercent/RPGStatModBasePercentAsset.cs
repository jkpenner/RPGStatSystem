using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public class RPGStatModBasePercentAsset : RPGStatModifierAsset {
        public override RPGStatModifier CreateInstance() {
            return Internal_CreateInstance<RPGStatModBasePercent>();
        }

        /* Do not need to implement, RPGStatModBasePercent does not have any data set
        protected override T Internal_CreateInstance<T>() {
            var mod = base.Internal_CreateInstance<T>() as RPGStatModBasePercent;
            // Add any initialization code here

            return mod as T;
        }
        */
    }
}
