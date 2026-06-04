using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/DumpsterDivingUnicornCardData")]
public class DumpsterDivingUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.SPECIAL;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new TakeFromDiscardAction()
        };
    }
}
