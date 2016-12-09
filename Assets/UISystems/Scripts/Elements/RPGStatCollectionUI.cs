using UnityEngine;
using System.Collections.Generic;
using RPGSystems.StatSystem;
using RPGSystems.StatSystem.Database;

public class RPGStatCollectionUI : MonoBehaviour {
    public RPGStatUI statUIPrefab;
    private List<RPGStatUI> uiInstances = new List<RPGStatUI>();

    public RPGStatCollection statCollection;

    public void Start() {
        foreach (var statType in RPGStatDatabase.StatTypes.GetAssets()) {
            var stat = statCollection.GetStat(statType.Id);
            if (stat != null) {
                var statUi = Instantiate(statUIPrefab);
                statUi.name = statType.Name;
                statUi.transform.SetParent(this.transform, false);

                statUi.SetStat(statType, stat);
                uiInstances.Add(statUi);
            }
        }
    }

    public void OnDisable() {
        foreach (var instance in uiInstances) {
            instance.SetStat(null, null);
            Destroy(instance);
        }
        uiInstances.Clear();
    }
}
