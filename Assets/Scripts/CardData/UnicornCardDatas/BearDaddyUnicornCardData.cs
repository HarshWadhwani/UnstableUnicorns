using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BearDaddyUnicornCardData")]
public class BearDaddyUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new SearchDeckForCardAction { targetCardDataType = typeof(TwinkicornCardData) }
        };
    }
}
