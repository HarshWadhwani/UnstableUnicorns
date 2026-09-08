using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeighCardData : CardData
{
    public NeighType neighType;

    public override void OnEnable()
    {
        base.OnEnable();
        cardType = CardType.NEIGH;
        specialActionType = SpecialActionType.IMMEDIATE;
    }

    // Neigh Means Neigh (NeighType.Final) states "This card cannot be Neigh'd" — playing it
    // immediately ends the counter-chain. Every other Neigh can itself be countered.
    public bool CanBeNeighed => neighType != NeighType.Final;
}
