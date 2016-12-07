namespace RPGSystems.StatSystem {
    public interface IStatModiferValueChange {
        void AddValueListener(RPGStatModifierEvent func);
        void RemoveValueListener(RPGStatModifierEvent func);
    }
}
