using System.Collections.Generic;
using UnityEngine;

public enum  PendingActionType
{
    None,
    DiscardCard,
    GiveCard,
    DestroyCard
}

public class CardActionExecutor : MonoBehaviour
{
    public static CardActionExecutor Instance { get; private set; }

    public TurnManager turnManager;
    public CardManager cardManager;
    public DiscardPile discardPile;

    public PendingActionType currentPendingAction = PendingActionType.None;
    public CardSpace pendingSourceStable;
    public CardSpace pendingDestinationStable;
    public int pendingCardsRemaining;
    
    private Player originalActivePlayer;
    private Queue<CardAction> actionQueue = new Queue<CardAction>();
    private CardActionContext currentContext;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple CardActionExecutor instances detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void ExecuteActions(List<CardAction> actions, Card sourceCard)
    {
        if (actions == null || actions.Count == 0)
        {
            Debug.Log("No actions to execute.");
            return;
        }

        actionQueue.Clear();
        currentContext = CreateContext(sourceCard);

        foreach (CardAction action in actions)
        {
            if (action != null)
            {
                actionQueue.Enqueue(action);
            }
        }

        ExecuteNextAction();
    }

    private void ExecuteNextAction()
    {
        if (actionQueue.Count == 0)
        {
            Debug.Log("All actions completed.");
            if (turnManager.currentPhase == TurnPhase.ImmediateSpecial ||
                turnManager.currentPhase == TurnPhase.EveryTurnSpecial)
            {
                turnManager.StartNextTurnPhase();
            }
            return;
        }

        CardAction nextAction = actionQueue.Dequeue();
        nextAction.Execute(this, currentContext);
    }

    private CardActionContext CreateContext(Card sourceCard)
    {
        Player activePlayer = turnManager.activePlayer;
        Player opponentPlayer = GetOpponentPlayer(activePlayer);

        return new CardActionContext(
            activePlayer,
            opponentPlayer,
            sourceCard,
            turnManager,
            cardManager,
            discardPile
        );
    }

    private Player GetOpponentPlayer(Player activePlayer)
    {
        foreach (Player player in turnManager.players)
        {
            if (player != activePlayer)
            {
                return player;
            }
        }
        return null;
    }

    public void PromptPlayerToSelectAndDiscardCards(Player player, CardSpace sourceStable, CardSpace discardPile, int numberOfCards)
    {
        Debug.Log($"Prompting {player.name} to select {numberOfCards} card(s) from their hand to discard.");
        
        originalActivePlayer = turnManager.activePlayer;
        turnManager.activePlayer = player;
        
        currentPendingAction = PendingActionType.DiscardCard;
        pendingSourceStable = sourceStable;
        pendingDestinationStable = discardPile;
        pendingCardsRemaining = numberOfCards;
    }

    public void PromptPlayerToSelectAndGiveCards(Player givingPlayer, CardSpace sourceStable, CardSpace destinationStable, int numberOfCards)
    {
        Debug.Log($"Prompting {givingPlayer.name} to select {numberOfCards} card(s) to give.");
        
        originalActivePlayer = turnManager.activePlayer;
        turnManager.activePlayer = givingPlayer;
        
        currentPendingAction = PendingActionType.GiveCard;
        pendingSourceStable = sourceStable;
        pendingDestinationStable = destinationStable;
        pendingCardsRemaining = numberOfCards;
    }

    public void PromptPlayerToSelectAndDestroyCards(Player destroyingPlayer, CardSpace discardPile, int numberOfCards)
    {
        Debug.Log($"Prompting {destroyingPlayer.name} to select {numberOfCards} card(s) to destroy.");

        originalActivePlayer = turnManager.activePlayer;
        turnManager.activePlayer = destroyingPlayer;

        currentPendingAction = PendingActionType.DestroyCard;
        pendingSourceStable = null;
        pendingDestinationStable = discardPile;
        pendingCardsRemaining = numberOfCards;
    }

    public void ExecutePendingAction(Card card)
    {
        if (currentPendingAction == PendingActionType.None)
        {
            Debug.LogWarning("No pending action to execute.");
            return;
        }

        CardSpace source = pendingSourceStable ?? card.cardSpace;
        cardManager.MoveCard(card, source, pendingDestinationStable);
        pendingCardsRemaining--;

        Debug.Log($"Executed {currentPendingAction} on {card.name}. {pendingCardsRemaining} card(s) remaining.");

        if (pendingCardsRemaining <= 0)
        {
            ClearPendingAction();
        }
    }

    private void ClearPendingAction()
    {
        Debug.Log($"Pending action {currentPendingAction} completed. Restoring active player to {originalActivePlayer.name}.");
        
        turnManager.activePlayer = originalActivePlayer;
        currentPendingAction = PendingActionType.None;
        pendingSourceStable = null;
        pendingDestinationStable = null;
        pendingCardsRemaining = 0;
        originalActivePlayer = null;

        ExecuteNextAction();
    }
}


