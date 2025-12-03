using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public TurnManager turnManager;
    public DiscardPile discardPile;

    void Start()
    {
        foreach (Player player in turnManager.players)
        {
            player.handStable.cardManager = this;
        }
    }

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
                var opponent = turnManager.players
                    .FirstOrDefault(player => player != turnManager.activePlayer);
                
                if (opponent == null)
                {
                    Debug.Log($"Player {turnManager.activePlayer.name} has no opponent");
                    return false;
                }
                
                MoveCard(card, handStable, opponent.downgradeStable);
                return true;
            case CardType.MAGIC:
            case CardType.NEIGH:
                MoveCard(card, handStable, discardPile);
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
