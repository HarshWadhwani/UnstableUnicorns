using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnicornCardData : CardData
{
    public UnicornType unicornType;

    public override void OnEnable()
    {
        cardType = CardType.UNICORN;
        specialActionType = SpecialActionType.NONE;
        afterAction = AfterAction.PLACE_IN_STABLE;
    }
}
