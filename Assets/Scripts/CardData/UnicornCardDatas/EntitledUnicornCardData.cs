using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/EntitledUnicornCardData")]
public class EntitledUnicornCardData : UnicornCardData
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
                destination = LookAndTakeFromHandAction.Destination.ActivePlayerHand,
                cardTypeFilter = CardType.UNICORN
            }
        };
    }
}
