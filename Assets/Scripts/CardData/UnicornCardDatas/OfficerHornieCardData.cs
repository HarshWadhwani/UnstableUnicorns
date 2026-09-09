using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/OfficerHornieCardData")]
public class OfficerHornieCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new LookAndTakeFromHandAction
            {
                targetPlayer = LookAndTakeFromHandAction.TargetPlayer.Opponent,
                destination = LookAndTakeFromHandAction.Destination.DiscardPile,
                cardTypeFilter = CardType.UNICORN
            }
        };
    }
}
