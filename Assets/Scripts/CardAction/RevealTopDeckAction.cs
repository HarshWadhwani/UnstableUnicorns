using UnityEngine;

[System.Serializable]
public class RevealTopDeckAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        if (context.playDeck.spaceCards.Count == 0)
        {
            Debug.Log("Play deck is empty. Skipping RevealTopDeckAction.");
            return;
        }

        Card topCard = context.playDeck.spaceCards[context.playDeck.spaceCards.Count - 1];
        topCard.RevealCard();

        if (topCard.cardData.cardType == CardType.UNICORN)
        {
            context.cardManager.MoveCard(topCard, context.playDeck, context.activePlayer.unicornStable);
            context.activePlayer.unicornStable.CheckWinCondition();
            Debug.Log($"Revealed {topCard.name} — a Unicorn card, brought into {context.activePlayer.name}'s Stable.");
        }
        else
        {
            context.cardManager.MoveCard(topCard, context.playDeck, context.activePlayer.handStable);
            Debug.Log($"Revealed {topCard.name} — added to {context.activePlayer.name}'s hand.");
        }
    }
}
