using UnityEngine;
using System.Collections;

public abstract class RPGStatScaler {
    public RPGStatScaler(RPGStatScalerAsset asset) {

    }

    public abstract int GetValue(int level);
}
