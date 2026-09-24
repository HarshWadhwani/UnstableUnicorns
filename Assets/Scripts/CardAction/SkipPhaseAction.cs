using UnityEngine;

// Makes a player skip their next Draw phase, Action phase, or whole turn (TurnManager.RequestSkip).
// No prompt. Used by Sticky Situation (ActivePlayer, Draw or Action — via ChooseEffectAction) and
// Unicorn Hangover (Opponent = the player chosen by ChoosePlayerAction, Turn).
[System.Serializable]
public class SkipPhaseAction : CardAction
{
    public enum Target { ActivePlayer, Opponent }

    public Target target = Target.ActivePlayer;
    public TurnSkip kind = TurnSkip.Draw;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player player = target == Target.ActivePlayer ? context.activePlayer : context.opponentPlayer;
        context.turnManager.RequestSkip(player, kind);
    }
}
