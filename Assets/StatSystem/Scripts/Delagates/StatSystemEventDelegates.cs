namespace RPGSystems.StatSystem {
    /// <summary>
    /// Delegate that passes a RPGStat when activated
    /// </summary>
    public delegate void RPGStatEvent(RPGStat stat);

    /// <summary>
    /// Delegate that passes a IStatCurrentValue when activated
    /// </summary>
    public delegate void CurrentValueEvent(IStatCurrentValue iCurrent);
}
