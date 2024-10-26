using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;
    public Player activePlayer;

    // Start is called before the first frame update
    void Start()
    {
        activePlayer = players[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCardFromDeck(Card drawnCard)
    {
        drawnCard.RevealCard();
        activePlayer.handStable.AddCardToStable(drawnCard); 
    }
}
