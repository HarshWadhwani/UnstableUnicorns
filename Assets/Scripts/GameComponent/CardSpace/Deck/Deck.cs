using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : CardSpace
{

    public override void HandleCardClick(Card card)
    {
        turnManager.MoveCardBetweenDecks(card);
        RemoveCardFromCurrentStable(card);
    }
}
