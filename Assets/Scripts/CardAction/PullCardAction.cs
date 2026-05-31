using UnityEngine;

[System.Serializable]
public class PullCardAction : CardAction
{
    public int numberOfCards = 1;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        CardSpace opponentHand = context.opponentPlayer.handStable;
        CardSpace activeHand = context.activePlayer.handStable;

        if (opponentHand.spaceCards.Count == 0)
        {
            Debug.Log($"{context.opponentPlayer.name} has no cards in hand to pull.");
            return;
        }

        int cardsToPull = Mathf.Min(numberOfCards, opponentHand.spaceCards.Count);
        for (int i = 0; i < cardsToPull; i++)
        {
            int randomIndex = Random.Range(0, opponentHand.spaceCards.Count);
            Card card = opponentHand.spaceCards[randomIndex];
            context.cardManager.MoveCard(card, opponentHand, activeHand);
            Debug.Log($"Pulled {card.name} from {context.opponentPlayer.name}'s hand.");
        }
    }
}
