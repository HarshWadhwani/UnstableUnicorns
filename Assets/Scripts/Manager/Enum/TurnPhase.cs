using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase
{
    ImmediateSpecial,  // 0 — was Special, renamed in place
    Draw,              // 1 — preserved
    Action,            // 2 — preserved
    EveryTurnSpecial   // 3 — new, added at end
}