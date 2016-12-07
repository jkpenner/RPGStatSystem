using System.Collections.Generic;

namespace RPGSystems.StatSystem {
    /// <summary>
    /// Allows the stat to use stat linkers
    /// </summary>
    public interface IStatLinkableAsset {
        List<RPGStatLinkerAsset> StatLinkers { get; }
    }
}
