using UnityEngine;

[System.Serializable]
public class DrawCardAction : CardAction
{
    public int numberOfCards = 1;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (context.playDeck.spaceCards.Count == 0)
            {
                Debug.Log("Play deck is empty. Skipping remaining draws.");
                return;
            }

            Card topCard = context.playDeck.spaceCards[context.playDeck.spaceCards.Count - 1];
            context.cardManager.DrawCard(topCard, context.playDeck, context.activePlayer);
            Debug.Log($"{context.activePlayer.name} drew {topCard.name} from the play deck.");
        }
    }
}
