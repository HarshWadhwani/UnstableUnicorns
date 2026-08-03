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
}
