using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/UnicornSpeedCardData")]
public class UnicornSpeedCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new DrawCardAction { numberOfCards = 1 }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC);
    }
}
