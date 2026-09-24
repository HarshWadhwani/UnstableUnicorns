using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/SextraTerrestrialUnicornCardData")]
public class SextraTerrestrialUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        // "When this card enters your Stable, you may return a card in another player's Stable to
        // their hand." Like other IMMEDIATE "you may"s, it runs whenever there's a target.
        actions = new List<CardAction>
        {
            new ReturnToHandAction
            {
                whose = ReturnToHandAction.Whose.OtherPlayers,
                targetStable = ReturnToHandAction.TargetStable.Any
            }
        };
    }
}
