using UnityEngine;
using System.Collections;

namespace RPGSystems.StatSystem {
    public class RPGStatScalerUtility : MonoBehaviour {
        static public RPGStatScalerAsset CreateAsset(string type) {
            switch (type) {
                case "RPGStatScalerLinearAsset": return new RPGStatScalerLinearAsset();
            }
            return null;
        }
    }
}
