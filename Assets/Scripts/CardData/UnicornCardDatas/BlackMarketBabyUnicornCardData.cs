using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BlackMarketBabyUnicornCardData")]
public class BlackMarketBabyUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 2 },
            new BringFromNurseryAction()
        };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.handStable.spaceCards.Count >= 2
            && DeckManager.Instance.nursery.spaceCards.Count > 0;
    }
}
