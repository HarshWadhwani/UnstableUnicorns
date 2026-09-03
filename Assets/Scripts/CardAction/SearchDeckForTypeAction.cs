using UnityEngine;

[System.Serializable]
public class SearchDeckForTypeAction : CardAction
{
    public CardType targetCardType;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Card foundCard = context.playDeck.spaceCards.Find(c => c.cardData.cardType == targetCardType);

        if (foundCard == null)
        {
            Debug.Log($"No {targetCardType} card found in the deck. Skipping SearchDeckForTypeAction.");
            context.deckManager.ShuffleDeck(context.playDeck);
            return;
        }

        foundCard.RevealCard();
        context.cardManager.MoveCard(foundCard, context.playDeck, context.activePlayer.handStable);
        context.deckManager.ShuffleDeck(context.playDeck);
        Debug.Log($"{context.activePlayer.name} found {foundCard.name} in the deck and added it to hand.");
    }
}
