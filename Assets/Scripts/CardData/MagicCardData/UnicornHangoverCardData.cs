using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/UnicornHangoverCardData")]
public class UnicornHangoverCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "Force another player to skip their next turn."
        actions = new List<CardAction>
        {
            new ChoosePlayerAction
            {
                title = "Who skips their next turn?",
                then = new List<CardAction>
                {
                    new SkipPhaseAction { target = SkipPhaseAction.Target.Opponent, kind = TurnSkip.Turn }
                }
            }
        };
    }
}
