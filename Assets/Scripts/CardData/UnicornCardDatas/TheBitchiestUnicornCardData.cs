using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/TheBitchiestUnicornCardData")]
public class TheBitchiestUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.Opponent, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 1 }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC);
    }
}
