using System.Collections.Generic;
using UnityEngine;

// Discards the active player's whole hand, no prompt. With redrawSameCount, they then draw as many
// cards as they discarded (Unicorn Acid Trip). The source card is skipped — a Magic card is still
// in its caster's hand while its effect runs (CardManager.ResolvePlay fires it before discarding
// it), and it shouldn't discard or count itself. Used inside ForEachPlayerAction.
[System.Serializable]
public class DiscardHandAction : CardAction
{
    public bool redrawSameCount = false;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player player = context.activePlayer;
        CardSpace hand = player.handStable;
        List<Card> toDiscard = hand.spaceCards.FindAll(c => c != context.sourceCard);

        foreach (Card card in toDiscard)
        {
            context.cardManager.MoveCard(card, hand, context.discardPile);
        }
        Debug.Log($"{player.name} discarded their hand ({toDiscard.Count} card(s)).");

        if (!redrawSameCount) return;

        int drawn = 0;
        for (; drawn < toDiscard.Count && context.playDeck.spaceCards.Count > 0; drawn++)
        {
            Card topCard = context.playDeck.spaceCards[context.playDeck.spaceCards.Count - 1];
            context.cardManager.DrawCard(topCard, context.playDeck, player);
        }
        Debug.Log($"{player.name} drew {drawn} card(s) back.");
    }
}
