using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CardSpace : MonoBehaviour
{
    public List<Card> spaceCards;

    public TurnManager turnManager;

    public abstract void HandleCardClick(Card card);

    protected void RemoveCardFromCurrentStable(Card card)
    {
        if (spaceCards.Contains(card))
        {
            spaceCards.Remove(card);
        }
    }

    public void AddCardToSpace(Card card)
    {
        spaceCards.Add(card);
        card.cardSpace = this;
        card.transform.SetParent(transform, false);
    }
}
