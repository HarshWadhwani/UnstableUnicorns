using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicCardData : CardData
{

    public override void OnEnable()
    {
        cardType = CardType.MAGIC;
        afterAction = AfterAction.DISCARD;
        specialActionType = SpecialActionType.IMMEDIATE;
    }
}
