using UnityEngine;

[System.Serializable]
public class DestroyCardAction : CardAction
{
    public enum DestroyerPlayer { ActivePlayer, Opponent }

    public DestroyerPlayer destroyer = DestroyerPlayer.ActivePlayer;
    public int numberOfCards = 1;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player destroyingPlayer = destroyer == DestroyerPlayer.ActivePlayer
            ? context.activePlayer
            : context.opponentPlayer;

        Player targetPlayer = destroyer == DestroyerPlayer.ActivePlayer
            ? context.opponentPlayer
            : context.activePlayer;

        int totalCards = targetPlayer.unicornStable.spaceCards.Count
                       + targetPlayer.upgradeStable.spaceCards.Count
                       + targetPlayer.downgradeStable.spaceCards.Count;

        if (totalCards == 0)
        {
            Debug.Log($"{targetPlayer.name} has no cards in any stable to destroy.");
            return;
        }

        Debug.Log($"{destroyingPlayer.name} must choose {numberOfCards} card(s) to destroy from any of {targetPlayer.name}'s stables.");
        executor.PromptPlayerToSelectAndDestroyCards(destroyingPlayer, context.discardPile, numberOfCards);
    }
}
