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
