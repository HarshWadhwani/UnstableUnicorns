using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CardManager : MonoBehaviour
{
    public TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in turnManager.players)
        {
            player.handStable.cardManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard(Card card, CardSpace currentCardSpace, Player player)
    {
        card.RevealCard();
        MoveCard(card, currentCardSpace, player.handStable);
    }

    public bool PlayCardForCurrentPlayer(Card card, HandStable handStable)
    {
        switch (card.cardType)
        {
            case CardType.UNICORN:
                MoveCard(card, handStable, turnManager.activePlayer.unicornStable);
                turnManager.activePlayer.unicornStable.CheckWinCondition();
                return true;
            case CardType.UPGRADE:
                MoveCard(card, handStable, turnManager.activePlayer.upgradeStable);
                return true;
            case CardType.DOWNGRADE:
                MoveCard(card, handStable, turnManager.activePlayer.downgradeStable);
                return true;
            default:
                return false;
        }
    }

    private void MoveCard(Card card, CardSpace oldCardSpace, CardSpace newCardSpace)
    {
        oldCardSpace.RemoveCard(card);
        newCardSpace.AddCard(card);
    }
}
