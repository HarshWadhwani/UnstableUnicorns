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

    public void MoveCardBetweenDecks(Card drawnCard)
    {
        if (TurnPhaseGrouping.ActionsForDrawingFromDeck.Contains(currentPhase))
        {
            DrawCardFromDeck(drawnCard);
            StartNextTurnPhase();
        }
    }

    private void DrawCardFromDeck(Card drawnCard)
    {
        drawnCard.RevealCard();
        activePlayer.handStable.AddCardToStable(drawnCard);
    }

    public void PlaceCardInStable(Card drawnCard)
    {
        if (currentPhase == TurnPhase.Action)
        {
            activePlayer.unicornStable.AddCardToStable(drawnCard);
            activePlayer.unicornStable.CheckWinCondition();
            StartNextTurnPhase();
        }
    }

    private void StartNextTurnPhase()
    {
        if (currentPhase == TurnPhase.Draw) 
        { 
            currentPhase = TurnPhase.Action; 
        }
        else if (currentPhase == TurnPhase.Action) 
        { 
            currentPhase = TurnPhase.Draw; 
            SwitchToNextPlayer(); }
    }

    public void SwitchToNextPlayer()
    {
        var activePlayerIndex = players.IndexOf(activePlayer);
        var newActivePlayerIndex = activePlayerIndex + 1;
        if (newActivePlayerIndex == players.Count)
        {
            newActivePlayerIndex = 0;
        }
        activePlayer = players[newActivePlayerIndex];
    }
}
