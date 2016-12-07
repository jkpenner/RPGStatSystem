using UnityEngine;
using System.Collections;

namespace RPGSystems.StatSystem {
    public class RPGVitalAsset : RPGAttributeAsset {
        public override RPGStat CreateInstance() {
            return new RPGVital(this);
        }
    }
}
