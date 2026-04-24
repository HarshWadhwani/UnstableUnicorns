using UnityEngine;

public class CardActionContext
{
    public Player activePlayer;
    public Player opponentPlayer;
    public Card sourceCard;
    public TurnManager turnManager;
    public CardManager cardManager;
    public DiscardPile discardPile;

    public CardActionContext(Player activePlayer, Player opponentPlayer, Card sourceCard, 
                            TurnManager turnManager, CardManager cardManager, DiscardPile discardPile)
    {
        this.activePlayer = activePlayer;
        this.opponentPlayer = opponentPlayer;
        this.sourceCard = sourceCard;
        this.turnManager = turnManager;
        this.cardManager = cardManager;
        this.discardPile = discardPile;
    }
}
