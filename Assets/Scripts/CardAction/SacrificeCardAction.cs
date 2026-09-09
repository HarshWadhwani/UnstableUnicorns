using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SacrificeCardAction : CardAction
{
    public enum TargetStable { Unicorn, Upgrade, Downgrade, Any }
    public enum SacrificerPlayer { ActivePlayer, Opponent }

    public TargetStable targetStable = TargetStable.Downgrade;
    // Whose own stables are sacrificed from. Default ActivePlayer (every existing caller);
    // Opponent is used by Sex, Drugs, and Unicorns ("that player must ... SACRIFICE a Unicorn").
    public SacrificerPlayer sacrificer = SacrificerPlayer.ActivePlayer;
    public bool sacrificeAll = true;
    public int numberOfCards = 1;
    // Null = any unicorn. Only meaningful when targetStable includes the Unicorn stable.
    public UnicornType? targetSubtype = null;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player sacrificingPlayer = sacrificer == SacrificerPlayer.ActivePlayer
            ? context.activePlayer
            : context.opponentPlayer;

        if (sacrificingPlayer == null)
        {
            Debug.Log("SacrificeCardAction: no sacrificing player. Skipping.");
            return;
        }

        if (!sacrificeAll)
        {
            List<CardSpace> targetStables = GetTargetStables(sacrificingPlayer);
            if (!targetStables.Any(s => s.spaceCards.Any(MatchesSubtype)))
            {
                Debug.Log($"{sacrificingPlayer.name} has no eligible cards to sacrifice.");
                return;
            }

            executor.pendingSacrificeTargetPlayer = sacrificingPlayer;
            executor.pendingSacrificeTargetStable = targetStable;
            executor.pendingSacrificeSubtypeFilter = targetSubtype;
            executor.PromptPlayerToSelectCards(sacrificingPlayer, null, context.discardPile, numberOfCards, PendingActionType.SacrificeCard);
            return;
        }

        foreach (CardSpace stable in GetTargetStables(sacrificingPlayer))
        {
            List<Card> toSacrifice = new List<Card>(stable.spaceCards);
            if (toSacrifice.Count == 0)
            {
                Debug.Log($"{sacrificingPlayer.name} has no cards in {stable.name} to sacrifice.");
                continue;
            }
            foreach (Card card in toSacrifice)
            {
                context.cardManager.MoveCard(card, stable, context.discardPile);
            }
            Debug.Log($"{sacrificingPlayer.name} sacrificed {toSacrifice.Count} card(s) from {stable.name}.");
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
