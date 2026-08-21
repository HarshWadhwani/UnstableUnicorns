using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayCardFromHandAction : CardAction
{
    public CardType cardType;
    // Null = any subtype. Only meaningful when cardType == UNICORN.
    public UnicornType? targetSubtype = null;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        CardSpace hand = context.activePlayer.handStable;

        bool MatchesType(Card c) =>
            c.cardData.cardType == cardType
            && (!targetSubtype.HasValue || (c.cardData is UnicornCardData u && u.unicornType == targetSubtype.Value));

        if (!hand.spaceCards.Any(MatchesType))
        {
            Debug.Log($"{context.activePlayer.name} has no {cardType} cards in hand to play.");
            return;
        }

        Debug.Log($"{context.activePlayer.name} may choose a {cardType} card from hand to bring into their Stable.");
        executor.pendingPlayCardTypeFilter = cardType;
        executor.pendingPlayCardSubtypeFilter = targetSubtype;
        executor.PromptPlayerToSelectCards(context.activePlayer, hand, null, 1, PendingActionType.PlayCardFromHand);
    }
}
