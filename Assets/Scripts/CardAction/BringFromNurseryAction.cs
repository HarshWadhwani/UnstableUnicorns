using UnityEngine;

[System.Serializable]
public class BringFromNurseryAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Deck nursery = context.deckManager.nursery;

        if (nursery.spaceCards.Count == 0)
        {
            Debug.Log("Nursery is empty. Skipping BringFromNurseryAction.");
            return;
        }

        Card babyUnicorn = nursery.spaceCards[0];
        babyUnicorn.RevealCard();
        context.cardManager.MoveCard(babyUnicorn, nursery, context.activePlayer.unicornStable);
        context.activePlayer.unicornStable.CheckWinCondition();
        Debug.Log($"{context.activePlayer.name} brought {babyUnicorn.name} from the Nursery into their Stable.");
    }
}
