using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/HorrifyingImpalingCardData")]
public class HorrifyingImpalingCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new DestroyCardAction
            {
                destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer,
                targetStable = DestroyCardAction.TargetStable.Unicorn,
                numberOfCards = 1
            },
            new DiscardCardAction
            {
                targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer,
                selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                numberOfCards = 1
            },
            new DiscardCardAction
            {
                targetPlayer = DiscardCardAction.TargetPlayer.Opponent,
                selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                numberOfCards = 1
            }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer != null && opponentPlayer.unicornStable.spaceCards.Count > 0;
    }
}
