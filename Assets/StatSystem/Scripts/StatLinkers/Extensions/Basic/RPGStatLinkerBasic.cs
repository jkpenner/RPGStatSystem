using UnityEngine;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Basic implementation of a RPGStatLinker. Returns a percentage 
    /// of the Linked Stat
    /// </summary>
    public class RPGStatLinkerBasic : RPGStatLinker {
        /// <summary>
        /// The Ratio of the linked stat to use
        /// </summary>
        private float _ratio;

        /// <summary>
        /// Gets the Ratio value. Read Only
        /// </summary>
        public float Ratio {
            get { return _ratio; }
        }

        /// <summary>
        /// returns the ratio of the linked stat as the linker's value
        /// </summary>
        public override int GetValue() {
            return (int)(LinkedStat.StatValue * _ratio);
        }

        public RPGStatLinkerBasic(RPGStatLinkerBasicAsset asset) : base(asset) {
            this._ratio = asset.Ratio;
        }

        public RPGStatLinkerBasic(RPGStatLinkerBasicAsset asset, RPGStat linkedStat) : base(asset, linkedStat) {
            this._ratio = asset.Ratio;
        }
    }
}
