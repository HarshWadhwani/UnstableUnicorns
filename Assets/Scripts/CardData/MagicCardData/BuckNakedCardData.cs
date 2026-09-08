using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/BuckNakedCardData")]
public class BuckNakedCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new DestroyCardAction
            {
                destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer,
                targetStable = DestroyCardAction.TargetStable.Upgrade,
                destroyAll = true
            }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer != null && opponentPlayer.upgradeStable.spaceCards.Count > 0;
    }
}
