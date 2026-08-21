using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SacrificeCardAction : CardAction
{
    public enum TargetStable { Unicorn, Upgrade, Downgrade, Any }

    public TargetStable targetStable = TargetStable.Downgrade;
    public bool sacrificeAll = true;
    public int numberOfCards = 1;
    // Null = any unicorn. Only meaningful when targetStable includes the Unicorn stable.
    public UnicornType? targetSubtype = null;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player activePlayer = context.activePlayer;

        if (!sacrificeAll)
        {
            List<CardSpace> targetStables = GetTargetStables(activePlayer);
            if (!targetStables.Any(s => s.spaceCards.Any(MatchesSubtype)))
            {
                Debug.Log($"{activePlayer.name} has no eligible cards to sacrifice.");
                return;
            }

            executor.pendingSacrificeTargetPlayer = activePlayer;
            executor.pendingSacrificeTargetStable = targetStable;
            executor.pendingSacrificeSubtypeFilter = targetSubtype;
            executor.PromptPlayerToSelectCards(activePlayer, null, context.discardPile, numberOfCards, PendingActionType.SacrificeCard);
            return;
        }

        foreach (CardSpace stable in GetTargetStables(activePlayer))
        {
            List<Card> toSacrifice = new List<Card>(stable.spaceCards);
            if (toSacrifice.Count == 0)
            {
                Debug.Log($"{activePlayer.name} has no cards in {stable.name} to sacrifice.");
                continue;
            }
            foreach (Card card in toSacrifice)
            {
                context.cardManager.MoveCard(card, stable, context.discardPile);
            }
            Debug.Log($"{activePlayer.name} sacrificed {toSacrifice.Count} card(s) from {stable.name}.");
        }
    }

    private bool MatchesSubtype(Card c) =>
        !targetSubtype.HasValue || (c.cardData is UnicornCardData u && u.unicornType == targetSubtype.Value);

    private List<CardSpace> GetTargetStables(Player player)
    {
        switch (targetStable)
        {
            case TargetStable.Unicorn:   return new List<CardSpace> { player.unicornStable };
            case TargetStable.Upgrade:   return new List<CardSpace> { player.upgradeStable };
            case TargetStable.Downgrade: return new List<CardSpace> { player.downgradeStable };
            case TargetStable.Any:       return new List<CardSpace> { player.unicornStable, player.upgradeStable, player.downgradeStable };
            default:                     return new List<CardSpace>();
        }
    }
}
