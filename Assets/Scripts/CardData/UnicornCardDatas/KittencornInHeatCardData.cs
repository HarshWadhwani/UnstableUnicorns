using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/KittencornInHeatCardData")]
public class KittencornInHeatCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        actions = new List<CardAction>
        {
            new BringFromNurseryAction()
        };
    }
}
