using RPGSystems.StatSystem;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour {
    [SerializeField]
    private int _id = 0;

    private bool _isDead = false;

    [SerializeField]
    private Entity _target = null;
    [SerializeField]
    private bool _isTargetable = true;

    private RPGStatCollection _statCollection;

    private RPGVital _health;
    private RPGStat _healthRegen;
    [SerializeField]
    private RPGStatType _healthType = RPGStatType.Health;
    [SerializeField]
    private RPGStatType _healthRegenType = RPGStatType.HealthRegen;

    private RPGVital _energy;
    private RPGStat _energyRegen;
    [SerializeField]
    private RPGStatType _energyType = RPGStatType.Energy;
    [SerializeField]
    private RPGStatType _energyRegenType = RPGStatType.EnergyRegen;

    private RPGVital _mana;
    private RPGStat _manaRegen;
    [SerializeField]
    private RPGStatType _manaType = RPGStatType.Mana;
    [SerializeField]
    private RPGStatType _manaRegenType = RPGStatType.ManaRegen;

    [SerializeField]
    private bool _isKillable;

    public int Id {
        get { return _id; }
        set { _id = value; }
    }

    public string Name { get; set; }

    public bool IsTargetable {
        get { return _isTargetable; }
        set { _isTargetable = value; }
    }

    public Entity Target {
        get { return _target; }
        set { _target = value; }
    }

    /// <summary>
    /// Is the entity dead
    /// </summary>
    public bool IsDead {
        get { return _isDead; }
        set { _isDead = value; }
    }

    public RPGStatCollection StatCollection {
        get { return _statCollection; }
    }

    public void OnEnable() {
        _statCollection = GetComponent<RPGStatCollection>();
        if (_statCollection != null) {
            if (_statCollection.IsCollectionSetup == false) {
                _statCollection.SetupCollection();
            }

            if (_statCollection.TryGetStat(_healthType, out _health) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain vital of type {1}",
                    this.name, _healthType.ToString());
            }

            if (_statCollection.TryGetStat(_healthRegenType, out _healthRegen) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain vital of type {1}",
                    this.name, _healthRegenType.ToString());
            }

            if (_statCollection.TryGetStat(_energyType, out _energy) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain vital of type {1}",
                    this.name, _energyType.ToString());
            }

            if (_statCollection.TryGetStat(_energyRegenType, out _energyRegen) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain vital of type {1}",
                    this.name, _energyRegenType.ToString());
            }

            if (_statCollection.TryGetStat(_manaType, out _mana) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain vital of type {1}",
                    this.name, _manaType.ToString());
            }

            if (_statCollection.TryGetStat(_manaRegenType, out _manaRegen) == false) {
                Debug.LogWarningFormat("[{0}]: Collection does not contain stat of type {1}",
                    this.name, _manaRegenType.ToString());
            }
        }
    }

    public void Update() {
        if(IsDead) return;

        if (_health != null && _healthRegen != null && _health.StatValueCurrent != _health.StatValue) {
            _health.StatValueCurrent += _healthRegen.StatValue * Time.deltaTime;
        }

        if (_energy != null && _energyRegen != null && _energy.StatValueCurrent != _energy.StatValue) {
            _energy.StatValueCurrent += _energyRegen.StatValue * Time.deltaTime;
        }

        if (_mana != null && _manaRegen != null && _mana.StatValueCurrent != _mana.StatValue) {
            _mana.StatValueCurrent += _manaRegen.StatValue * Time.deltaTime;
        }
    }

    public void DamageHealth(string sourceId, int amount) {
        if (_health == null) {
            Debug.LogWarningFormat("[{0}]: Took damage, but no kill vital assigned", this.name);
            return;
        }

        if (IsDead == false) {
            _health.StatValueCurrent -= amount;

            if (_health.StatValueCurrent <= 0) {
                Die(sourceId);
                Debug.Log("Dead");
            }
        }
    }

    /// <summary>
    /// Get the current amount of energy
    /// </summary>
    public float GetEnergy() {
        if (_energy != null) {
            return _energy.StatValueCurrent;
        }
        return 0;
    }

    /// <summary>
    /// Modify the current amount of energy
    /// </summary>
    public void ModifyEnergy(float amount) {
        if (_energy != null) {
            _energy.StatValueCurrent += amount;
        }
    }

    /// <summary>
    /// Get the current amount of mana
    /// </summary>
    public float GetMana() {
        if (_mana != null) {
            return _mana.StatValueCurrent;
        }
        return 0;
    }

    /// <summary>
    /// Modify the current amount of energy
    /// </summary>
    public void ModifyMana(float amount) {
        if (_mana != null) {
            _mana.StatValueCurrent += amount;
        }
    }

    public void GiveExp(int amount) {
        StatCollection.ModifyExp(amount);
    }

    private void Die(string sourceId) {
        IsDead = true;

        if (_health != null) {
            _health.StatValueCurrent = 0;
        }

        OnDeath(sourceId);

        if (EventManager.Instance != null) {
            EventManager.Instance.OnEntityKilled.Invoke(this);
        }

        this.gameObject.SetActive(false);
    }


    public void Respawn(Vector3 position, Quaternion identity) {
        this.transform.position = position;
        this.transform.rotation = identity;

        this.gameObject.SetActive(true);

        IsDead = false;

        if (_health != null) {
            _health.SetCurrentValueToMax();
        }

        OnRespawn();

        if (EventManager.Instance != null) {
            EventManager.Instance.OnEntityRespawn.Invoke(this);
        }
    }

    protected virtual void OnDeath(string sorceId) { }
    protected virtual void OnRespawn() { }
}
