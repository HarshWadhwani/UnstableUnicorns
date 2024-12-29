using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase
{
    Special,
    Draw,
    Action
}

public static class TurnPhaseGrouping
{
    public static readonly HashSet<TurnPhase> ActionsForDrawingFromDeck = new()
    {
        TurnPhase.Draw,
        TurnPhase.Action
    };
}