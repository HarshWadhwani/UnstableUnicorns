using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/FreeCandyUnicornCardData")]
public class FreeCandyUnicornCardData : UnicornCardData, IReturnsStolenCardOnLeave
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new StealUnicornAction { targetSubtype = UnicornType.BABY }
        };
    }
}
