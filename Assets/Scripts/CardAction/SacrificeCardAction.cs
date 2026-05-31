using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SacrificeCardAction : CardAction
{
    public enum TargetStable { Unicorn, Upgrade, Downgrade, Any }

    public TargetStable targetStable = TargetStable.Downgrade;
    public bool sacrificeAll = true;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player activePlayer = context.activePlayer;

        if (!sacrificeAll)
        {
            Debug.LogWarning("SacrificeCardAction: PlayerChooses mode is not yet implemented.");
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
