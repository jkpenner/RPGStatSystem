using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    [System.Serializable]
    public class RPGStatModTotalAddAsset : RPGStatModifierAsset {
        public override RPGStatModifier CreateInstance() {
            return Internal_CreateInstance<RPGStatModTotalAdd>();
        }

        /* Do not need to implement, RPGStatModTotalAdd does not have any data set
        protected override T Internal_CreateInstance<T>() {
            var mod = base.Internal_CreateInstance<T>() as RPGStatModTotalAdd;
            // Add any initialization code here

            return mod as T;
        }
        */
    }
}
