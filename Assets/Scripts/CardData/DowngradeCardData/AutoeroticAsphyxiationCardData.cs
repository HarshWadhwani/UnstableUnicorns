using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/DowngradeCardData/AutoeroticAsphyxiationCardData")]
public class AutoeroticAsphyxiationCardData : DowngradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new DiscardCardAction
            {
                targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer,
                selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                numberOfCards = 1
            }
        };
    }
}
