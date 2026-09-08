using System.Collections.Generic;
using UnityEngine;

public enum  PendingActionType
{
    None,
    DiscardCard,
    GiveCard,
    DestroyCard,
    DestroyUnicornCard,
    StealCard,
    PlayCardFromHand,
    SacrificeCard
}

public class CardActionExecutor : MonoBehaviour
{
    public static CardActionExecutor Instance { get; private set; }

    public TurnManager turnManager;
    public CardManager cardManager;
    public DiscardPile discardPile;
    public DeckManager deckManager;

    public PendingActionType currentPendingAction = PendingActionType.None;
    public CardSpace pendingSourceStable;
    public CardSpace pendingDestinationStable;
    public int pendingCardsRemaining;
    public Player pendingDestroyTargetPlayer;
    public UnicornType? pendingStealSubtypeFilter;
    public CardType? pendingPlayCardTypeFilter;
    public UnicornType? pendingPlayCardSubtypeFilter;
    public Player pendingSacrificeTargetPlayer;
    public SacrificeCardAction.TargetStable? pendingSacrificeTargetStable;
    public UnicornType? pendingSacrificeSubtypeFilter;

    private Player originalActivePlayer;
    private Queue<CardAction> actionQueue = new Queue<CardAction>();
    private CardActionContext currentContext;

    // Optional one-shot callback fired when the current action queue drains. When set, it takes
    // over from the built-in "advance the turn phase" behaviour — the caller is then responsible
    // for whatever comes next. Used by NeighManager to sequence a Neigh's post-cancel discard(s)
    // before resolving (or discarding) the contested card. Consumed (nulled) when invoked.
    private System.Action onQueueComplete;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Host the runtime-built managers here so they need no scene wiring — each discovers
            // its own references (NeighManager off this component; BoardChrome off the scene).
            if (NeighManager.Instance == null)
            {
                gameObject.AddComponent<NeighManager>();
            }
            if (BoardChrome.Instance == null)
            {
                gameObject.AddComponent<BoardChrome>();
            }
        }
        else
        {
            Debug.LogWarning("Multiple CardActionExecutor instances detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void ExecuteActions(List<CardAction> actions, Card sourceCard, System.Action onComplete = null)
    {
        if (actions == null || actions.Count == 0)
        {
            Debug.Log("No actions to execute.");
            onComplete?.Invoke();
            return;
        }

        actionQueue.Clear();
        onQueueComplete = onComplete;
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

            if (onQueueComplete != null)
            {
                System.Action cb = onQueueComplete;
                onQueueComplete = null;
                cb();
                return;
            }

            if (turnManager.currentPhase == TurnPhase.ImmediateSpecial ||
                turnManager.currentPhase == TurnPhase.EveryTurnSpecial)
            {
                turnManager.StartNextTurnPhase();
            }
            return;
        }

        CardAction nextAction = actionQueue.Dequeue();
        nextAction.Execute(this, currentContext);

        // Synchronous actions don't set a pending action — chain immediately to the next.
        if (currentPendingAction == PendingActionType.None)
        {
            ExecuteNextAction();
        }
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
            discardPile,
            deckManager.playDeck,
            deckManager
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

    // Pass source = null to defer source resolution to click time (Destroy: source is whichever
    // stable the clicked card lives in). Otherwise source locks the "from" space for the move.
    public void PromptPlayerToSelectCards(Player player, CardSpace source, CardSpace destination, int numberOfCards, PendingActionType actionType)
    {
        Debug.Log($"Prompting {player.name} to select {numberOfCards} card(s) for {actionType}.");

        originalActivePlayer = turnManager.activePlayer;
        turnManager.activePlayer = player;

        currentPendingAction = actionType;
        pendingSourceStable = source;
        pendingDestinationStable = destination;
        pendingCardsRemaining = numberOfCards;
    }

    public void ExecutePendingAction(Card card)
    {
        if (currentPendingAction == PendingActionType.None)
        {
            Debug.LogWarning("No pending action to execute.");
            return;
        }

        if (currentPendingAction == PendingActionType.PlayCardFromHand)
        {
            bool played = cardManager.PlayCardForCurrentPlayer(card, (HandStable)card.cardSpace);
            if (!played)
            {
                Debug.LogWarning($"{card.name} cannot be played right now.");
                return;
            }
        }
        else
        {
            CardSpace source = pendingSourceStable ?? card.cardSpace;

            // Only ever links a Baby Unicorn — a non-Baby-Unicorn steal (e.g. Fuzzy Hoofcuffs)
            // never populates these fields, so it can't trigger the leave-hook in CardManager.
            if (currentPendingAction == PendingActionType.StealCard
                && currentContext.sourceCard.cardData is IReturnsStolenCardOnLeave
                && card.cardData is UnicornCardData stolenUnicorn
                && stolenUnicorn.unicornType == UnicornType.BABY)
            {
                currentContext.sourceCard.linkedBabyUnicorn = card;
                currentContext.sourceCard.linkedBabyUnicornOriginStable = source;
            }

            cardManager.MoveCard(card, source, pendingDestinationStable);
        }

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
        pendingDestroyTargetPlayer = null;
        pendingStealSubtypeFilter = null;
        pendingPlayCardTypeFilter = null;
        pendingPlayCardSubtypeFilter = null;
        pendingSacrificeTargetPlayer = null;
        pendingSacrificeTargetStable = null;
        pendingSacrificeSubtypeFilter = null;
        originalActivePlayer = null;

        ExecuteNextAction();
    }
}


