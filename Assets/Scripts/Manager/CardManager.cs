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
        Player opponent = turnManager.players.FirstOrDefault(p => p != turnManager.activePlayer);
        if (!card.cardData.CanPlay(turnManager.activePlayer, opponent))
        {
            Debug.LogWarning($"Cannot play {card.cardData.cardNameVariations[0]}: play conditions not met.");
            return false;
        }

        if (card.cardData.specialActionType == SpecialActionType.IMMEDIATE)
        {
            card.cardData.TriggerSpecialAction(card);
        }

        switch (card.cardData.cardType)
        {
            case CardType.UNICORN:
                MoveCard(card, handStable, turnManager.activePlayer.unicornStable);
                turnManager.activePlayer.unicornStable.CheckWinCondition();
                return true;
            case CardType.UPGRADE:
                MoveCard(card, handStable, turnManager.activePlayer.upgradeStable);
                return true;
            case CardType.DOWNGRADE:
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

    public void MoveCard(Card card, CardSpace oldCardSpace, CardSpace newCardSpace)
    {
        oldCardSpace.RemoveCard(card);
        newCardSpace.AddCard(card);
    }
}
