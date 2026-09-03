using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/UnicornDancerCardData")]
public class UnicornDancerCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new DrawCardAction { numberOfCards = 1 },
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 1 }
        };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return DeckManager.Instance.playDeck.spaceCards.Count > 0;
    }
}
