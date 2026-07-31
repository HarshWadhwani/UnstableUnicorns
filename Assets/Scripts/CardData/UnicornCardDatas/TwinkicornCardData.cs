using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/TwinkicornCardData")]
public class TwinkicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new SearchDeckForCardAction { targetCardDataType = typeof(BearDaddyUnicornCardData) }
        };
    }
}
