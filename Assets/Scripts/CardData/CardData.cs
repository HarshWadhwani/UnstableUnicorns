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

    public abstract void OnEnable();

    public virtual void TriggerSpecialAction(Card sourceCard)
    {
        if (actions.Count > 0 && CardActionExecutor.Instance != null)
        {
            Debug.Log($"Executing {actions.Count} actions for card: {cardNameVariations[0]}");
            CardActionExecutor.Instance.ExecuteActions(actions, sourceCard);
        }
        else
        {
            Debug.Log($"No actions configured for card: {cardNameVariations[0]}");
        }
    }
}
