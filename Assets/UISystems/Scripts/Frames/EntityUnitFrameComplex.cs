using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RPGSystems.StatSystem;
using System;

public class EntityUnitFrameComplex : MonoBehaviour {
    public Entity attachedEntity;

    public Text playerLevel;
    public Text playerName;
    public Text playerStatus;

    public RPGStatType healthBarType = RPGStatType.None;
    private RPGVital healthVital;
    public RPGVitalUI healthBar;

    public RPGStatType manaBarType = RPGStatType.None;
    private RPGVital manaVital;
    public RPGVitalUI manaBar;

    public RPGStatType energyBarType = RPGStatType.None;
    private RPGVital energyVital;
    public RPGVitalUI energyBar;

    public RPGVitalUI experienceBar;

    public void OnEnable() {
        if (attachedEntity.StatCollection.TryGetStat(healthBarType, out healthVital)) {
            healthVital.AddValueListener(OnHealthValueChange);
            healthVital.AddCurrentValueListener(OnHealthCurrentValueChange);
        }

        if (attachedEntity.StatCollection.TryGetStat(manaBarType, out manaVital)) {
            manaVital.AddValueListener(OnManaValueChange);
            manaVital.AddCurrentValueListener(OnManaCurrentValueChange);
        }

        if (attachedEntity.StatCollection.TryGetStat(energyBarType, out energyVital)) {
            energyVital.AddValueListener(OnEnergyValueChange);
            energyVital.AddCurrentValueListener(OnEnergyCurrentValueChange);
        }
    }

    public void OnDisable() {
        if (healthVital != null) {
            healthVital.AddValueListener(OnHealthValueChange);
            healthVital.AddCurrentValueListener(OnHealthCurrentValueChange);
        }

        if (manaVital != null) {
            manaVital.AddValueListener(OnManaValueChange);
            manaVital.AddCurrentValueListener(OnManaCurrentValueChange);
        }


        if (energyVital != null) {
            energyVital.AddValueListener(OnEnergyValueChange);
            energyVital.AddCurrentValueListener(OnEnergyCurrentValueChange);
        }
    }

    private void OnHealthValueChange(IStatValue iStatValue) {
        healthBar.SetActiveFill(healthVital.StatValueCurrent, healthVital.StatValue);
    }

    private void OnHealthCurrentValueChange(IStatValueCurrent iStatValueCurrent) {
        healthBar.SetActiveFill(healthVital.StatValueCurrent, healthVital.StatValue);
    }

    private void OnEnergyValueChange(IStatValue iStatValue) {
        energyBar.SetActiveFill(energyVital.StatValueCurrent, energyVital.StatValue);
    }

    private void OnEnergyCurrentValueChange(IStatValueCurrent iStatValueCurrent) {
        energyBar.SetActiveFill(energyVital.StatValueCurrent, energyVital.StatValue);
    }

    private void OnManaValueChange(IStatValue iStatValue) {
        manaBar.SetActiveFill(manaVital.StatValueCurrent, manaVital.StatValue);
    }

    private void OnManaCurrentValueChange(IStatValueCurrent iStatValueCurrent) {
        manaBar.SetActiveFill(manaVital.StatValueCurrent, manaVital.StatValue);
    }

    public void Update() {
        if (attachedEntity != null) {
            playerLevel.text = string.Format("Level: {0}", attachedEntity.StatCollection.Level.ToString("D2"));
            playerName.text = attachedEntity.name;

            if (attachedEntity.IsDead) {
                playerStatus.text = "Dead";
            } else {
                playerStatus.text = "";
            }

            experienceBar.SetActiveFill(attachedEntity.StatCollection.CurrentExp, 
                attachedEntity.StatCollection.RequiredExp);
        } else {
            playerLevel.text = string.Format("Level: {0}", "--");
            playerName.text = "Not Attached";
            playerStatus.text = "Missing";

            healthBar.SetActiveFill(0, 1);
            manaBar.SetActiveFill(0, 1);
            energyBar.SetActiveFill(0, 1);

            experienceBar.SetActiveFill(0, 1);
        }
    }
}
