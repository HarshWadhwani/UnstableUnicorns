using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeighCardData : CardData
{
    public NeighType neighType;

    public override void OnEnable()
    {
        cardType = CardType.NEIGH;
        specialActionType = SpecialActionType.IMMEDIATE;
        afterAction = AfterAction.DISCARD;
    }
}
