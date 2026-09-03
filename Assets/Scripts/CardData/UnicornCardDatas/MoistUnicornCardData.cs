using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/MoistUnicornCardData")]
public class MoistUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new SearchDeckForTypeAction { targetCardType = CardType.UNICORN }
        };
    }
}
