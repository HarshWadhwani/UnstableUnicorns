using System.Linq;
using UnityEngine;

[System.Serializable]
public class StealUnicornAction : CardAction
{
    // Null = any unicorn. Set to restrict which UnicornType can be selected (e.g. BABY for Baby Trap).
    public UnicornType? targetSubtype = null;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        CardSpace opponentUnicornStable = context.opponentPlayer.unicornStable;

        bool MatchesSubtype(Card c) =>
            c.cardData is UnicornCardData u && (!targetSubtype.HasValue || u.unicornType == targetSubtype.Value);

        if (!opponentUnicornStable.spaceCards.Any(MatchesSubtype))
        {
            Debug.Log($"{context.opponentPlayer.name} has no {(targetSubtype.HasValue ? targetSubtype.Value.ToString() : "")} unicorns to steal.");
            return;
        }

        Debug.Log($"{context.activePlayer.name} must choose a unicorn to steal from {context.opponentPlayer.name}'s stable.");
        executor.pendingStealSubtypeFilter = targetSubtype;
        executor.PromptPlayerToSelectCards(
            context.activePlayer,
            null,
            context.activePlayer.unicornStable,
            1,
            PendingActionType.StealCard
        );
    }
}
