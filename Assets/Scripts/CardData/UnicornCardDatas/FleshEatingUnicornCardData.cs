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

    // Guards against a partial effect: DiscardCardAction doesn't cap numberOfCards to hand size,
    // so without this an opponent hand of 0-1 cards would leave the discard prompt stuck waiting
    // for a card that doesn't exist. Unlike an EVERY_TURN choice card, this is IMMEDIATE — the
    // active player has no in-game signal stopping them from playing it regardless of the
    // opponent's hand size, so CanPlay is the only available gate.
    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer.handStable.spaceCards.Count >= 2;
    }
}
