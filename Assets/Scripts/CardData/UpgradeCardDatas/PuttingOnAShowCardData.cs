using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/PuttingOnAShowCardData")]
public class PuttingOnAShowCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new SacrificeCardAction { targetStable = SacrificeCardAction.TargetStable.Any, sacrificeAll = false },
            new DestroyCardAction { destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer, targetStable = DestroyCardAction.TargetStable.Any, numberOfCards = 1 }
        };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer.unicornStable.spaceCards.Count > 0
            || opponentPlayer.upgradeStable.spaceCards.Count > 0
            || opponentPlayer.downgradeStable.spaceCards.Count > 0;
    }
}
