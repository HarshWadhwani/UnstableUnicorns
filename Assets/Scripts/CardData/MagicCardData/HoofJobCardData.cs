using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/HoofJobCardData")]
public class HoofJobCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new LookAndTakeFromHandAction
            {
                targetPlayer = LookAndTakeFromHandAction.TargetPlayer.Opponent,
                destination = LookAndTakeFromHandAction.Destination.ActivePlayerHand,
                cardTypeFilter = null
            }
        };
    }

    // "Look at another player's hand" — needs a non-empty hand to look at.
    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer != null && opponentPlayer.handStable.spaceCards.Count > 0;
    }
}
