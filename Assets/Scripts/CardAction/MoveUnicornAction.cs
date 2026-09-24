using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// "Move a Unicorn card from <a Stable> to any other player's Stable." Two steps: the active player
// clicks a Unicorn, then — if more than one player could receive it — picks the destination
// (ChoosePlayer). A Unicorn can never move to its own owner or to the mover. With 2 players there
// is always exactly one destination, so step 2 never shows.
// returnAtEndOfTurn makes it a loan: TurnManager.ScheduleAtEndOfTurn brings it back when the
// mover's turn ends, if it's still where it was left.
// Used by Dominatrix Whip (AnyPlayer) and Unicorn Cuckold (Self, returnAtEndOfTurn).
[System.Serializable]
public class MoveUnicornAction : CardAction
{
    public enum Source { Self, AnyPlayer }

    public Source source = Source.AnyPlayer;
    public bool returnAtEndOfTurn = false;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        List<Player> sources = EligibleSources(executor, context.activePlayer);
        if (sources.Count == 0)
        {
            Debug.Log("MoveUnicornAction: no Unicorn can be moved anywhere. Skipping.");
            return;
        }

        executor.pendingMoveSourcePlayers = sources;
        executor.pendingMoveMover = context.activePlayer;
        executor.pendingMoveReturnAtEndOfTurn = returnAtEndOfTurn;
        executor.PromptPlayerToSelectCards(context.activePlayer, null, null, 1, PendingActionType.MoveUnicorn);
    }

    // Players whose Unicorn stable has a card AND at least one legal destination for it.
    public List<Player> EligibleSources(CardActionExecutor executor, Player mover)
    {
        IEnumerable<Player> candidates = source == Source.Self
            ? new[] { mover }
            : executor.turnManager.players;

        return candidates
            .Where(p => p.unicornStable.spaceCards.Count > 0
                        && executor.GetMoveDestinations(p, mover).Count > 0)
            .ToList();
    }
}
