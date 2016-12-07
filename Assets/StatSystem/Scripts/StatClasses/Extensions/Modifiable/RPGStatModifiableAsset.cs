using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    public class RPGStatModifiableAsset : RPGStatAsset {
        public override RPGStat CreateInstance() {
            return new RPGStatModifiable(this);
        }

        /* Do not need to implement, RPGStatModifiable does not have any data set
        protected override T Internal_CreateInstance<T>() {
            var stat = base.Internal_CreateInstance<T>() as RPGStatModifiable;
            // Add any initialization code here

            return stat as T;
        }
        */
    }
}
