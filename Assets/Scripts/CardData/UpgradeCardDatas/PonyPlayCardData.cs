using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/PonyPlayCardData")]
public class PonyPlayCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new PullCardAction { numberOfCards = 1, skipDrawPhaseOnSuccess = true }
        };
    }
}
