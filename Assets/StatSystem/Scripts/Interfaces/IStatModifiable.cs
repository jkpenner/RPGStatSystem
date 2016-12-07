namespace RPGSystems.StatSystem {
    /// <summary>
    /// Allows the stat to use modifiers
    /// </summary>
    public interface IStatModifiable {
        int StatModifierValue { get; }

        int GetModifierCount();
        RPGStatModifier GetModifierAt(int index);

        void AddModifier(RPGStatModifier mod);
        void RemoveModifier(RPGStatModifier mod);
        void ClearModifiers();
        void UpdateModifiers();
    }
}
