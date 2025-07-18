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
}
