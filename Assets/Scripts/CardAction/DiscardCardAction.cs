using UnityEngine;

[System.Serializable]
public class DiscardCardAction : CardAction
{
    public enum TargetPlayer { ActivePlayer, Opponent }
    public enum SelectionMode { PlayerChooses, Random }
    
    public TargetPlayer targetPlayer = TargetPlayer.Opponent;
    public SelectionMode selectionMode = SelectionMode.PlayerChooses;
    public int numberOfCards = 1;
    
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player target = targetPlayer == TargetPlayer.ActivePlayer 
            ? context.activePlayer 
            : context.opponentPlayer;
        
        CardSpace sourceStable = target.handStable;
        
        if (sourceStable.spaceCards.Count == 0)
        {
            Debug.Log($"{target.name} has no cards in hand to discard.");
            return;
        }
        
        if (selectionMode == SelectionMode.PlayerChooses)
        {
            Debug.Log($"{target.name} must choose {numberOfCards} card(s) to discard from their hand.");
            executor.PromptPlayerToSelectAndDiscardCards(target, sourceStable, context.discardPile, numberOfCards);
        }
        else if (selectionMode == SelectionMode.Random)
        {
            for (int i = 0; i < numberOfCards && sourceStable.spaceCards.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, sourceStable.spaceCards.Count);
                Card cardToDiscard = sourceStable.spaceCards[randomIndex];
                context.cardManager.MoveCard(cardToDiscard, sourceStable, context.discardPile);
                Debug.Log($"Randomly discarded {cardToDiscard.name} from {target.name}'s hand.");
            }
        }
    }
}
