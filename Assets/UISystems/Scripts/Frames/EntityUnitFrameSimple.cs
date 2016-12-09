using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RPGSystems.StatSystem;
using System;

public class EntityUnitFrameSimple : MonoBehaviour {
    public Entity attachedEntity;

    private CanvasGroup group;

    public Text entityLevel;
    public Text entityName;
    public Text entityStatus;

    public RPGVitalUI healthBar;
    [SerializeField]
    private RPGStatType healthBarType;
    private RPGVital healthVital;

    public void Start() {
        group = this.gameObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    public void OnEnable() {
        RPGVital health;
        if (attachedEntity.StatCollection.TryGetStat(healthBarType, out health)) {
            healthVital = health;
            healthVital.AddValueListener(OnHealthValueChange);
            healthVital.AddCurrentValueListener(OnHealthCurrentValueChange);
        }
    }

    public void OnDisable() {
        if (healthVital != null) {
            healthVital.AddValueListener(OnHealthValueChange);
            healthVital.AddCurrentValueListener(OnHealthCurrentValueChange);
        }
    }

    private void OnHealthValueChange(IStatValue iStatValue) {
        healthBar.SetActiveFill(healthVital.StatValueCurrent, healthVital.StatValue);
    }

    private void OnHealthCurrentValueChange(IStatValueCurrent iStatValueCurrent) {
        healthBar.SetActiveFill(healthVital.StatValueCurrent, healthVital.StatValue);
    }

    public void Update() {
        if (attachedEntity != null) {
            group.alpha = 1f;
            entityLevel.text = string.Format("Level: <b>{0}</b>", attachedEntity.StatCollection.Level.ToString("D2"));
            entityName.text = attachedEntity.name;

            if (attachedEntity.IsDead) {
                entityStatus.text = "Dead";
            } else {
                entityStatus.text = "";
            }

            //if (attachedEntity.StatCollection.TryGetStat(healthBarType, out healthVital)) {
            //    healthBar.SetActiveFill(healthVital.StatValueCurrent, healthVital.StatValue);
            //}

        } else {
            group.alpha = 0f;

            entityLevel.text = string.Format("Level: <b>{0}</b>", "--");
            entityName.text = "Not Attached";
            entityStatus.text = "Missing";

            healthBar.SetActiveFill(0, 1);
        }
    }
}
