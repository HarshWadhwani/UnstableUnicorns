using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnicornCardData : CardData
{
    public UnicornType unicornType;

    public override void OnEnable()
    {
        cardType = CardType.NEIGH;
        afterAction = AfterAction.PLACE_IN_STABLE;
    }
}
