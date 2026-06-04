using UnityEngine;

[System.Serializable]
public class TakeFromDiscardAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        if (context.discardPile.spaceCards.Count == 0)
        {
            Debug.Log("Discard pile is empty. Skipping TakeFromDiscardAction.");
            return;
        }

        Card topCard = context.discardPile.spaceCards[context.discardPile.spaceCards.Count - 1];
        topCard.RevealCard();
        context.cardManager.MoveCard(topCard, context.discardPile, context.activePlayer.handStable);
        Debug.Log($"{context.activePlayer.name} drew {topCard.name} from the discard pile.");
    }
}
