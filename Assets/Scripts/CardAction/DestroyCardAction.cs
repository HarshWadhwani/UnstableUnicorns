using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestroyCardAction : CardAction
{
    public enum DestroyerPlayer { ActivePlayer, Opponent }
    public enum TargetStable { Any, Unicorn, Upgrade }

    public DestroyerPlayer destroyer = DestroyerPlayer.ActivePlayer;
    public TargetStable targetStable = TargetStable.Any;
    public int numberOfCards = 1;
    // true = auto-destroy every card in scope, no prompt (mirrors SacrificeCardAction.sacrificeAll).
    // numberOfCards is ignored in this mode. Sacrifice shields are NOT consulted — a mass
    // destroy isn't a targeted one, so there's nothing for a single shield to intercept.
    public bool destroyAll = false;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player destroyingPlayer = destroyer == DestroyerPlayer.ActivePlayer
            ? context.activePlayer
            : context.opponentPlayer;

        Player targetPlayer = destroyer == DestroyerPlayer.ActivePlayer
            ? context.opponentPlayer
            : context.activePlayer;

        if (destroyAll)
        {
            int destroyed = 0;
            foreach (CardSpace stable in GetTargetStables(targetPlayer))
            {
                foreach (Card card in new List<Card>(stable.spaceCards))
                {
                    context.cardManager.MoveCard(card, stable, context.discardPile);
                    destroyed++;
                }
            }
            Debug.Log($"{destroyingPlayer.name} destroyed all {destroyed} card(s) in scope from {targetPlayer.name}'s stable.");
            return;
        }

        int totalCards;
        PendingActionType actionType;

        if (targetStable == TargetStable.Unicorn)
        {
            totalCards = targetPlayer.unicornStable.spaceCards.Count;
            actionType = PendingActionType.DestroyUnicornCard;
        }
        else if (targetStable == TargetStable.Upgrade)
        {
            Debug.LogWarning("DestroyCardAction: interactive targetStable=Upgrade is not supported — set destroyAll=true.");
            return;
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

        // Check if target player has a sacrifice shield card (e.g. Hentaicorn).
        // First matching card auto-sacrifices itself instead of letting the destroyer choose.
        Card shieldCard = FindSacrificeShieldCard(targetPlayer);
        if (shieldCard != null)
        {
            Debug.Log($"[SacrificeShield] {shieldCard.name} intercepts the destroy — sacrificed automatically.");
            context.cardManager.MoveCard(shieldCard, shieldCard.cardSpace, context.discardPile);
            return;
        }

        Debug.Log($"{destroyingPlayer.name} must choose {numberOfCards} card(s) to destroy from {targetPlayer.name}'s stable.");
        executor.pendingDestroyTargetPlayer = targetPlayer;
        executor.PromptPlayerToSelectCards(destroyingPlayer, null, context.discardPile, numberOfCards, actionType);
    }

    private List<CardSpace> GetTargetStables(Player player)
    {
        switch (targetStable)
        {
            case TargetStable.Unicorn: return new List<CardSpace> { player.unicornStable };
            case TargetStable.Upgrade: return new List<CardSpace> { player.upgradeStable };
            case TargetStable.Any:     return new List<CardSpace> { player.unicornStable, player.upgradeStable, player.downgradeStable };
            default:                   return new List<CardSpace>();
        }
    }

    private Card FindSacrificeShieldCard(Player targetPlayer)
    {
        CardSpace[] stables = { targetPlayer.unicornStable, targetPlayer.upgradeStable, targetPlayer.downgradeStable };
        foreach (CardSpace stable in stables)
        {
            foreach (Card card in stable.spaceCards)
            {
                if (card.cardData is ISacrificeShield shield && shield.CanInterceptDestroy(targetStable))
                    return card;
            }
        }
        return null;
    }
}
