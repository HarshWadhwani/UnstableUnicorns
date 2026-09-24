using UnityEngine;

// Moves one Baby Unicorn from the active player's Unicorn stable back to the Nursery. No prompt —
// every Baby Unicorn shares one CardData, so which one goes is irrelevant. Skips silently if the
// player has none. Used by Safe Sex (inside ForEachPlayerAction).
[System.Serializable]
public class ReturnBabyToNurseryAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player player = context.activePlayer;
        CardSpace stable = player.unicornStable;
        Card baby = stable.spaceCards.Find(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BABY);

        if (baby == null)
        {
            Debug.Log($"{player.name} has no Baby Unicorn to return to the Nursery.");
            return;
        }

        context.cardManager.MoveCard(baby, stable, context.deckManager.nursery);
        Debug.Log($"{player.name} returned {baby.name} to the Nursery.");
    }
}
