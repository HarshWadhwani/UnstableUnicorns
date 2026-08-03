using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public List<string> cardNameVariations;
    public string cardDescriptionText;
    public int instances;
    public CardType cardType;
    public SpecialActionType specialActionType;
    public List<CardAction> actions = new List<CardAction>();

    private int nextNameIndex;

    public virtual void OnEnable()
    {
        nextNameIndex = 0;
    }

    public virtual string NextCardName()
    {
        string cardName = cardNameVariations[nextNameIndex % cardNameVariations.Count];
        nextNameIndex++;
        return cardName;
    }

    public virtual bool CanPlay(Player activePlayer, Player opponentPlayer) => true;

    // Gate for EVERY_TURN choice cards: checked when the player clicks the card in its stable
    // to activate it (TurnManager.TryActivateEveryTurnCard), before its actions run at all.
    // Unlike an individual CardAction skipping itself (e.g. DiscardCardAction on an empty hand),
    // this prevents a partial effect (e.g. discarding fewer than intended, then still stealing).
    public virtual bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer) => true;

    public virtual void TriggerSpecialAction(Card sourceCard)
    {
        if (actions.Count > 0 && CardActionExecutor.Instance != null)
        {
            Debug.Log($"Executing {actions.Count} actions for card: {sourceCard.name}");
            CardActionExecutor.Instance.ExecuteActions(actions, sourceCard);
        }
        else
        {
            Debug.Log($"No actions configured for card: {sourceCard.name}");
        }
    }
}
