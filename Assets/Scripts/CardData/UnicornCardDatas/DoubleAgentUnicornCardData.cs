using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/DoubleAgentUnicornCardData")]
public class DoubleAgentUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new SacrificeCardAction { targetStable = SacrificeCardAction.TargetStable.Unicorn, sacrificeAll = false, targetSubtype = UnicornType.BASIC },
            new PullCardAction { numberOfCards = 1 }
        };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC);
    }
}
