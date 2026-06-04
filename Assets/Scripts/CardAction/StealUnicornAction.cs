using UnityEngine;

[System.Serializable]
public class StealUnicornAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        CardSpace opponentUnicornStable = context.opponentPlayer.unicornStable;

        if (opponentUnicornStable.spaceCards.Count == 0)
        {
            Debug.Log($"{context.opponentPlayer.name} has no unicorns to steal.");
            return;
        }

        Debug.Log($"{context.activePlayer.name} must choose a unicorn to steal from {context.opponentPlayer.name}'s stable.");
        executor.PromptPlayerToSelectCards(
            context.activePlayer,
            null,
            context.activePlayer.unicornStable,
            1,
            PendingActionType.StealCard
        );
    }
}
