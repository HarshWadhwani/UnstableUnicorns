using UnityEngine;

// "Look at another player's hand. Choose a card and add it to your hand" (Hoof Job / Entitled
// Unicorn), or "... move it to the discard pile" (Officer Hornie). Every hand already renders
// face-up in this build, so there's no reveal step — the effect is purely a click-to-pick from
// the target player's HandStable.
//
// Mirrors DestroyCardAction's shape: the prompt records the target player on the executor
// (pendingTakeFromHandTargetPlayer) rather than relying on turnManager.activePlayer, which is
// temporarily reassigned to the chooser while the prompt is open. HandStable.HandleCardClick
// has the matching branch (checked before its active-player guard, since the clicked hand
// belongs to the target, not the chooser).
[System.Serializable]
public class LookAndTakeFromHandAction : CardAction
{
    public enum TargetPlayer { Opponent, ActivePlayer }
    public enum Destination { ActivePlayerHand, DiscardPile }

    public TargetPlayer targetPlayer = TargetPlayer.Opponent;
    public Destination destination = Destination.ActivePlayerHand;
    // null = any card type is eligible; otherwise only cards of this CardType can be chosen.
    public CardType? cardTypeFilter = null;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player chooser = context.activePlayer;
        Player target = targetPlayer == TargetPlayer.Opponent
            ? context.opponentPlayer
            : context.activePlayer;

        if (target == null || target.handStable.spaceCards.Count == 0)
        {
            Debug.Log("LookAndTakeFromHandAction: target has no cards in hand. Skipping.");
            return;
        }

        // "You may" — if a type filter is set and nothing in the hand matches, skip silently.
        if (cardTypeFilter.HasValue
            && !target.handStable.spaceCards.Exists(c => c.cardData.cardType == cardTypeFilter.Value))
        {
            Debug.Log($"LookAndTakeFromHandAction: {target.name} holds no {cardTypeFilter.Value} card. Skipping.");
            return;
        }

        CardSpace dest = destination == Destination.DiscardPile
            ? (CardSpace)context.discardPile
            : chooser.handStable;

        Debug.Log($"{chooser.name} looks at {target.name}'s hand and picks 1 "
                  + $"{(cardTypeFilter.HasValue ? cardTypeFilter.Value.ToString() : "card")} -> {destination}.");

        executor.pendingTakeFromHandTargetPlayer = target;
        executor.pendingTakeFromHandTypeFilter = cardTypeFilter;
        // source = null: resolved from the clicked card's cardSpace (the target player's hand).
        executor.PromptPlayerToSelectCards(chooser, null, dest, 1, PendingActionType.TakeFromHand);
    }
}
