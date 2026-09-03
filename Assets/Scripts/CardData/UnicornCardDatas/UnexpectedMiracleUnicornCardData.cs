using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/UnexpectedMiracleUnicornCardData")]
public class UnexpectedMiracleUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 1 },
            new BringFromNurseryAction()
        };
    }
}
