using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/FleshEatingUnicornCardData")]
public class FleshEatingUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new DiscardCardAction
            {
                targetPlayer = DiscardCardAction.TargetPlayer.Opponent,
                selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                numberOfCards = 2
            }
        };
    }
}
