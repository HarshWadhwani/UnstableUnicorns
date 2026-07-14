using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/BabyTrapCardData")]
public class BabyTrapCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new StealUnicornAction { targetSubtype = UnicornType.BABY }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer != null
            && opponentPlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BABY);
    }
}
