using System.Collections.Generic;
using UnityEngine;

// "Each player (including you) must ..." Runs `actions` once per player, in seat order starting
// with the current "you" (context.activePlayer). On each pass the template runs under a context
// rebound to that player — activePlayer = them, opponentPlayer = the next player in seat order —
// so any existing action targeting ActivePlayer means "this player" with no changes. The caster
// stays available as context.caster.
//
// All passes are spliced onto the front of the queue at once, so they run back to back and any
// prompt inside a pass pauses the whole loop exactly like a top-level prompt does. Template
// action instances are shared between passes, so they must not keep per-run state in fields.
[System.Serializable]
public class ForEachPlayerAction : CardAction
{
    public List<CardAction> actions = new List<CardAction>();

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        List<Player> players = context.turnManager.players;
        if (players == null || players.Count == 0 || actions.Count == 0) return;

        int start = Mathf.Max(0, players.IndexOf(context.activePlayer));
        var bound = new List<(CardAction, CardActionContext)>();

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[(start + i) % players.Count];
            Player next = players.Count > 1 ? players[(start + i + 1) % players.Count] : null;
            CardActionContext playerContext = context.ForPlayer(player, next);

            foreach (CardAction action in actions)
            {
                bound.Add((action, playerContext));
            }
        }

        Debug.Log($"ForEachPlayerAction: running {actions.Count} action(s) for each of {players.Count} player(s).");
        executor.PrependActions(bound);
    }
}
