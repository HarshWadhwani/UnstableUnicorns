using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayDeck : CardSpace
{
    public List<CardData> cardScriptableObjects;

    public override void HandleCardClick(Card card)
    {
        turnManager.MoveCardBetweenDecks(card);
        RemoveCardFromCurrentStable(card);
    }
}
