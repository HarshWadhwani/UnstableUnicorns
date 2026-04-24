using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/FuckMarryKillCardData")]
public class FuckMarryKillCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        
        actions = new List<CardAction>
        {
            new DiscardCardAction
            {
                targetPlayer = DiscardCardAction.TargetPlayer.Opponent,
                selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                numberOfCards = 1
            },
            new GiveCardAction
            {
                giver = GiveCardAction.TargetPlayer.ActivePlayer,
                numberOfCards = 1
            },
            new DestroyCardAction
            {
                destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer,
                targetStable = DestroyCardAction.TargetStable.Unicorn,
                numberOfCards = 1
            }
        };
    }
}

