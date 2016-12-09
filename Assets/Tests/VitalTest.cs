using UnityEngine;
using System.Collections;
using RPGSystems.StatSystem;
using System;

public class VitalTest : MonoBehaviour {
    public Entity entity;
    public RPGStatCollection collection;

	void OnEnable () {
        entity = GetComponent<Entity>();
	    collection = GetComponent<RPGStatCollection>();

        // If the collection is not setup, trigger setup
        if (collection.IsCollectionSetup == false) {
            collection.SetupCollection();
        }

        // Get the stat with id of RPGStatType.Health of type RPGVital
        RPGVital health;
        if(collection.TryGetStat(RPGStatType.Health, out health)) {
            // Connect to the Health stat's events
            health.AddValueListener(OnHealthValueChange);
            health.AddCurrentValueListener(OnHealthCurrentValueChange);

            // Add modifier to Health stat, then update stat's modifier value
            health.AddModifier(new RPGStatModBaseAdd(10.5f));
            health.UpdateModifiers();

            // Update the Health stat's current value to max
            health.SetCurrentValueToMax();
        }
	}

    void OnDisable() {
        // Get the stat with id of RPGStatType.Health of type RPGVital
        RPGVital health;
        if (collection.TryGetStat(RPGStatType.Health, out health)) {
            // Remove connections to the Health stat's events
            health.RemoveValueListener(OnHealthValueChange);
            health.RemoveCurrentValueListener(OnHealthCurrentValueChange);
        }
    }

    public void Update() {
        // Get the stat with id of RPGStatType.Health of type RPGVital
        RPGVital health;
        if (collection.TryGetStat(RPGStatType.Health, out health)) {
            // Subtract 100 from the Health stat's current value
            //entity.TakeDamage("", 100);
        } else {
            Debug.Log("No health");
        }

        //entity.GiveExp(100);

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Damaging Health");
            entity.DamageHealth("", 100);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("Damaging Energy");
            entity.ModifyEnergy(-100);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Debug.Log("Damaging Mana");
            entity.ModifyMana(-10);
        }
    }

    /// <summary>
    /// Called whenever the Health stat's value is changed
    /// </summary>
    private void OnHealthValueChange(IStatValue iStatValue) {
        Debug.LogFormat("Health value has changed to {0}", iStatValue.StatValue);
    }

    /// <summary>
    /// Called whenever the Health stat's current value is changed
    /// </summary>
    private void OnHealthCurrentValueChange(IStatValueCurrent iStatValueCurrent) {
        Debug.LogFormat("Health's current value changed to {0}", iStatValueCurrent.StatValueCurrent);
    }
}
