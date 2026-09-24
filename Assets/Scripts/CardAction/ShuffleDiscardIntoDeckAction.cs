using System.Collections.Generic;
using UnityEngine;

// Moves the whole discard pile into the play deck and shuffles it. Any Baby Unicorn found goes to
// the Nursery instead (CardManager.MoveCard already redirects them off the discard pile, so this
// is just a guard). Used by Rainbow Shitstorm.
[System.Serializable]
public class ShuffleDiscardIntoDeckAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        List<Card> cards = new List<Card>(context.discardPile.spaceCards);

        foreach (Card card in cards)
        {
            bool isBaby = card.cardData is UnicornCardData u && u.unicornType == UnicornType.BABY;
            context.cardManager.MoveCard(card, context.discardPile, isBaby ? context.deckManager.nursery : context.playDeck);
        }

        context.deckManager.ShuffleDeck(context.playDeck);
        Debug.Log($"Shuffled {cards.Count} card(s) from the discard pile into the play deck.");
    }
}
