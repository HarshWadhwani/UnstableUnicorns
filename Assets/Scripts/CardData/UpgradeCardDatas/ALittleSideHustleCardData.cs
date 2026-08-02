using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/ALittleSideHustleCardData")]
public class ALittleSideHustleCardData : UpgradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new PlayCardFromHandAction { cardType = CardType.UPGRADE }
        };
    }
}
