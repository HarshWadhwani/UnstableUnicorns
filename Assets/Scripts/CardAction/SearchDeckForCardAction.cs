using System;
using UnityEngine;

[System.Serializable]
public class SearchDeckForCardAction : CardAction
{
    public Type targetCardDataType;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Card foundCard = context.playDeck.spaceCards.Find(c => targetCardDataType.IsInstanceOfType(c.cardData));

        if (foundCard == null)
        {
            Debug.Log($"No {targetCardDataType.Name} found in the deck. Skipping SearchDeckForCardAction.");
            context.deckManager.ShuffleDeck(context.playDeck);
            return;
        }

        foundCard.RevealCard();
        context.cardManager.MoveCard(foundCard, context.playDeck, context.activePlayer.handStable);
        context.deckManager.ShuffleDeck(context.playDeck);
        Debug.Log($"{context.activePlayer.name} found {foundCard.name} in the deck and added it to hand.");
    }
}
