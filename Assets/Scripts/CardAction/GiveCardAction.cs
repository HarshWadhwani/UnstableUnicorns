using UnityEngine;

[System.Serializable]
public class GiveCardAction : CardAction
{
    public enum TargetPlayer { ActivePlayer, Opponent }
    
    public TargetPlayer giver = TargetPlayer.ActivePlayer;
    public int numberOfCards = 1;
    
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player givingPlayer = giver == TargetPlayer.ActivePlayer 
            ? context.activePlayer 
            : context.opponentPlayer;
        
        Player receivingPlayer = giver == TargetPlayer.ActivePlayer 
            ? context.opponentPlayer 
            : context.activePlayer;
        
        CardSpace sourceStable = givingPlayer.handStable;
        CardSpace destinationStable = receivingPlayer.handStable;
        
        if (sourceStable.spaceCards.Count == 0)
        {
            Debug.Log($"{givingPlayer.name} has no cards in hand to give.");
            return;
        }
        
        Debug.Log($"{givingPlayer.name} must choose {numberOfCards} card(s) to give to {receivingPlayer.name}.");
        executor.PromptPlayerToSelectAndGiveCards(givingPlayer, sourceStable, destinationStable, numberOfCards);
    }
}
