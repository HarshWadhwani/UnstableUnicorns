using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/SadomasocornCardData")]
public class SadomasocornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new SacrificeCardAction { targetStable = SacrificeCardAction.TargetStable.Any, sacrificeAll = false },
            new DestroyCardAction { destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer, targetStable = DestroyCardAction.TargetStable.Unicorn, numberOfCards = 1 }
        };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer.unicornStable.spaceCards.Count > 0;
    }
}
