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
        
        foreach (Player player in players)
        {
            player.handStable.turnManager = this;
        }

        activePlayer = players[0];
        currentPhase = TurnPhase.Draw;
    }

    public void StartNextTurnPhase()
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

    private void SwitchToNextPlayer()
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
