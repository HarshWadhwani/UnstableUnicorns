using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class CardSpace : MonoBehaviour
{
    public Card cardPrefab;
    public List<Card> spaceCards;

    public TurnManager turnManager;
    public CardManager cardManager;

    public List<TurnPhase> allowedTurnPhases; 

    public abstract void HandleCardClick(Card card);

    public void RemoveCard(Card card)
    {
        if (spaceCards.Contains(card))
        {
            spaceCards.Remove(card);
        }
    }

    public virtual void AddCard(Card card)
    {
        spaceCards.Add(card);
        card.cardSpace = this;
        card.transform.SetParent(transform, false);
    }
}
