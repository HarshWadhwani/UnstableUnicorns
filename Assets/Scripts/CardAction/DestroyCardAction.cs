using UnityEngine;

[System.Serializable]
public class DestroyCardAction : CardAction
{
    public enum TargetStable { Unicorn, Upgrade, Downgrade, Any }
    public enum DestroyerPlayer { ActivePlayer, Opponent }
    
    public DestroyerPlayer destroyer = DestroyerPlayer.ActivePlayer;
    public TargetStable targetStable = TargetStable.Any;
    public int numberOfCards = 1;
    
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player destroyingPlayer = destroyer == DestroyerPlayer.ActivePlayer 
            ? context.activePlayer 
            : context.opponentPlayer;
        
        Player targetPlayer = destroyer == DestroyerPlayer.ActivePlayer 
            ? context.opponentPlayer 
            : context.activePlayer;
        
        CardSpace targetCardSpace = GetTargetStable(targetPlayer, targetStable);
        
        if (targetCardSpace == null)
        {
            Debug.LogError($"Invalid target stable specified for DestroyCardAction.");
            return;
        }
        
        if (targetCardSpace.spaceCards.Count == 0)
        {
            Debug.Log($"{targetPlayer.name} has no cards in {targetStable} stable to destroy.");
            return;
        }
        
        Debug.Log($"{destroyingPlayer.name} must choose {numberOfCards} card(s) to destroy from {targetPlayer.name}'s {targetStable} stable.");
        executor.PromptPlayerToSelectAndDestroyCards(destroyingPlayer, targetCardSpace, context.discardPile, numberOfCards);
    }
    
    private CardSpace GetTargetStable(Player player, TargetStable stable)
    {
        switch (stable)
        {
            case TargetStable.Unicorn:
                return player.unicornStable;
            case TargetStable.Upgrade:
                return player.upgradeStable;
            case TargetStable.Downgrade:
                return player.downgradeStable;
            case TargetStable.Any:
                Debug.LogWarning("TargetStable.Any is not yet implemented. Defaulting to Unicorn stable.");
                return player.unicornStable;
            default:
                return null;
        }
    }
}
