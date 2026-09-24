using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : CardSpace
{

    public override void HandleCardClick(Card card)
    {
        if (!allowedTurnPhases.Contains(turnManager.currentPhase))
        {
            return;
        }

        cardManager.DrawCard(card, this, turnManager.activePlayer);
        turnManager.StartNextTurnPhase();
    }

    // Cards returned to a deck mid-game (Baby Unicorns back to the Nursery, Rainbow Shitstorm's
    // discard reshuffle) arrive from a stable/hand with that space's layout applied — flip them
    // face-down and reset the transform so they stack like the rest of the pile.
    public override void AddCard(Card card)
    {
        base.AddCard(card);
        card.HideCard();

        RectTransform cardRect = card.GetComponent<RectTransform>();
        if (cardRect != null)
        {
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);
            cardRect.anchoredPosition = Vector2.zero;
            cardRect.localEulerAngles = Vector3.zero;
            cardRect.localScale = Vector3.one;
        }
    }

    // Moves a card to the top of the deck (last in spaceCards, highest sibling index).
    // Both must change together — sibling index controls render order (top = clickable),
    // spaceCards order is what DrawCard uses.
    public void MoveToTop(Card card)
    {
        if (!spaceCards.Contains(card)) return;
        spaceCards.Remove(card);
        spaceCards.Add(card);
        for (int i = 0; i < spaceCards.Count; i++)
            spaceCards[i].transform.SetSiblingIndex(i);
    }
}
