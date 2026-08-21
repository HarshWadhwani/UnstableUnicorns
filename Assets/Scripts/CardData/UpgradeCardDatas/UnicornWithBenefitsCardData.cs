using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/UnicornWithBenefitsCardData")]
public class UnicornWithBenefitsCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new PlayCardFromHandAction { cardType = CardType.UNICORN, targetSubtype = UnicornType.BASIC }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC);
    }
}
