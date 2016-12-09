using UnityEngine;
using System.Collections;

namespace RPGSystems.StatSystem {
    public abstract class RPGStatScaler {
        public RPGStatScaler(RPGStatScalerAsset asset) {

        }

        public abstract int GetValue(int level);
    }
}
