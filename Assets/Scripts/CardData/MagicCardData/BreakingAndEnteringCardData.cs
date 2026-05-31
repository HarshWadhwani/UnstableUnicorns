using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/BreakingAndEnteringCardData")]
public class BreakingAndEnteringCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new PullCardAction { numberOfCards = 2 }
        };
    }
}
