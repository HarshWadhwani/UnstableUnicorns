using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;
    public Player activePlayer;
    public TurnPhase currentPhase;

    // Start is called before the first frame update
    void Start()
    {
        activePlayer = players[0];
        currentPhase = TurnPhase.Draw;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCardFromDeck(Card drawnCard)
    {
        if (currentPhase == TurnPhase.Draw)
        {
            drawnCard.RevealCard();
            activePlayer.handStable.AddCardToStable(drawnCard);
            currentPhase = TurnPhase.Action;
        }
    }

    public void PlaceCardInStable(Card drawnCard)
    {
        if (currentPhase == TurnPhase.Action)
        {
            activePlayer.unicornStable.AddCardToStable(drawnCard);
            currentPhase = TurnPhase.Draw;
        }
    }
}
