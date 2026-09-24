using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// "Choose a player. That player ..." The active player picks another player (auto-picked when
// there's only one — always, with 2 players); `then` runs with context.opponentPlayer rebound to
// the chosen player, so any existing Opponent-targeting action acts on them unchanged.
// Skips silently if there's no other player. Used by Unicorn Hangover.
[System.Serializable]
public class ChoosePlayerAction : CardAction
{
    public string title = "Choose a player";
    public List<CardAction> then = new List<CardAction>();

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player chooser = context.activePlayer;
        List<Player> candidates = context.turnManager.players.Where(p => p != chooser).ToList();

        if (candidates.Count == 0)
        {
            Debug.Log("ChoosePlayerAction: no other player to choose. Skipping.");
            return;
        }

        if (candidates.Count == 1)
        {
            executor.PrependActions(then, context.ForPlayer(chooser, candidates[0]));
            return;
        }

        executor.PromptPlayerChoice(chooser, title, candidates,
            chosen => executor.PrependActions(then, context.ForPlayer(chooser, chosen)));
    }
}
