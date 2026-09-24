using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/DowngradeCardData/StickySituationCardData")]
public class StickySituationCardData : DowngradeCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "If this card is in your Stable at the beginning of your turn, skip either your Draw
        // phase or your Action Phase." Mandatory (Downgrade); the affected player picks.
        actions = new List<CardAction>
        {
            new ChooseEffectAction
            {
                chooser = ChooseEffectAction.Chooser.ActivePlayer,
                optionALabel = "Skip your Draw phase",
                optionA = new List<CardAction> { new SkipPhaseAction { kind = TurnSkip.Draw } },
                optionBLabel = "Skip your Action phase",
                optionB = new List<CardAction> { new SkipPhaseAction { kind = TurnSkip.Action } }
            }
        };
    }
}
