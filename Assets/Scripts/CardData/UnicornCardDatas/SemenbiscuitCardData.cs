using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/SemenbiscuitCardData")]
public class SemenbiscuitCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new SacrificeCardAction { targetStable = SacrificeCardAction.TargetStable.Downgrade, sacrificeAll = true }
        };
    }
}
