using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/FuzzyHoofcuffsCardData")]
public class FuzzyHoofcuffsCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 2 },
            new StealUnicornAction()
        };
    }

    // Guards against a partial effect: DiscardCardAction doesn't cap numberOfCards to hand size,
    // so without this a hand of 0-1 cards would leave the discard prompt stuck waiting for a
    // card that doesn't exist (and the Skip button disables itself while a prompt is pending,
    // so there'd be no way out). Same pattern as Bukkakecorn.
    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer.unicornStable.spaceCards.Count >= 1 && activePlayer.handStable.spaceCards.Count >= 2;
    }
}
