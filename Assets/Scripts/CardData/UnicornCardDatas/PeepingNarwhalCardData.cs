using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/PeepingNarwhalCardData")]
public class PeepingNarwhalCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        // Information-only: every hand already renders face-up, so this just logs the opponents'
        // hands. No cards move.
        actions = new List<CardAction>
        {
            new RevealHandsAction()
        };
    }
}
