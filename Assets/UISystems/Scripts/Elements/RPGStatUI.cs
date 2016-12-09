using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RPGSystems.StatSystem;
using System;
using RPGSystems.StatSystem.Database;

public class RPGStatUI : MonoBehaviour {
    private RPGStat stat;
    private StatTypeAsset asset;

    public Image statIcon;
    public Text statName;
    public Text statValue;

    public void SetStat(StatTypeAsset asset, RPGStat stat) {
        if (this.stat != stat && this.stat != null) {
            this.stat.RemoveValueListener(OnStatValueChange);
        }

        this.stat = stat;
        this.asset = asset;

        if (this.stat != null) {
            OnStatValueChange(this.stat);
            this.stat.AddValueListener(OnStatValueChange);
        }

        if (this.asset != null) {
            statIcon.sprite = asset.Icon;
            statName.text = asset.Name;
        }
    }

    private void OnStatValueChange(IStatValue iStatValue) {
        statValue.text = iStatValue.StatValue.ToString();
    }
}
