using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/ManscapedLlamacornCardData")]
public class ManscapedLlamacornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.Opponent, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 1 }
        };
    }
}
