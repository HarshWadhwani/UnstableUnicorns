using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BukkakecornCardData")]
public class BukkakecornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 3 },
            new StealUnicornAction()
        };
    }

    // Guards against a partial effect: DiscardCardAction doesn't cap numberOfCards to hand size,
    // so without this a hand of 1-2 cards would leave the discard prompt stuck waiting for a
    // card that doesn't exist.
    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer.unicornStable.spaceCards.Count >= 1 && activePlayer.handStable.spaceCards.Count >= 3;
    }
}
