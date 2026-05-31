using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/UnicornEnemaCardData")]
public class UnicornEnemaCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new SacrificeCardAction
            {
                targetStable = SacrificeCardAction.TargetStable.Downgrade,
                sacrificeAll = true
            }
        };
    }
}
