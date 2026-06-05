using UnityEngine;

[System.Serializable]
public class DestroyCardAction : CardAction
{
    public enum DestroyerPlayer { ActivePlayer, Opponent }
    public enum TargetStable { Any, Unicorn }

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

        int totalCards;
        PendingActionType actionType;

        if (targetStable == TargetStable.Unicorn)
        {
            totalCards = targetPlayer.unicornStable.spaceCards.Count;
            actionType = PendingActionType.DestroyUnicornCard;
        }
        else
        {
            totalCards = targetPlayer.unicornStable.spaceCards.Count
                       + targetPlayer.upgradeStable.spaceCards.Count
                       + targetPlayer.downgradeStable.spaceCards.Count;
            actionType = PendingActionType.DestroyCard;
        }

        if (totalCards == 0)
        {
            Debug.Log($"{targetPlayer.name} has no cards in the target stable to destroy.");
            return;
        }

        Debug.Log($"{destroyingPlayer.name} must choose {numberOfCards} card(s) to destroy from {targetPlayer.name}'s stable.");
        executor.pendingDestroyTargetPlayer = targetPlayer;
        executor.PromptPlayerToSelectCards(destroyingPlayer, null, context.discardPile, numberOfCards, actionType);
    }
}
