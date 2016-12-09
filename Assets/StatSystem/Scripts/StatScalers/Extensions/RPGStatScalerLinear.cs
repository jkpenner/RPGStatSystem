using UnityEngine;
using System.Collections;
using System;

namespace RPGSystems.StatSystem {
    public class RPGStatScalerLinear : RPGStatScaler {
        private float _slope = 0f;
        public float _offset = 0f;

        public RPGStatScalerLinear(RPGStatScalerLinearAsset asset) : base(asset) {
            this._slope = asset.Slope;
            this._offset = asset.Offset;
        }

        public override int GetValue(int level) {
            return Mathf.RoundToInt((_slope * (level - 1)) + _offset);
        }
    }
}