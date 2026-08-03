using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/HomicidalPsychocornCardData")]
public class HomicidalPsychocornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        actions = new List<CardAction>
        {
            new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.ActivePlayer, selectionMode = DiscardCardAction.SelectionMode.Random, numberOfCards = 999 },
            new DestroyCardAction { destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer, targetStable = DestroyCardAction.TargetStable.Unicorn, numberOfCards = 1 }
        };
    }
}
