using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Stats.Interfaces {
    interface IStatLevelHandler {
        int GetStatIncreaseForLevel(int level);
    }
}
