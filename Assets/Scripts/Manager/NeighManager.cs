using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Owns the "play a Neigh out of turn to cancel another player's card" interrupt.
///
/// Flow:
///  1. CardManager.PlayCardForCurrentPlayer calls TryBeginContest before resolving a hand play.
///     If the other player holds a Neigh (and this is a plain Action-phase play), a contest opens:
///     the contested card stays in the caster's hand, AwaitingResponse goes true, and the Skip
///     button doubles as a Pass control (see PhaseIndicator).
///  2. The responder either plays a Neigh from hand (SubmitNeigh) or passes (Pass).
///     Each Neigh can itself be Neigh'd by the other player, alternating, until someone passes
///     or a NeighType.Final ("cannot be Neigh'd", i.e. Neigh Means Neigh) ends the chain.
///  3. ResolveContest settles it: an even number of Neighs on the stack => the contested card
///     resolves normally; odd => it goes to the discard pile and its effect never fires.
///     Every still-"live" Neigh in the chain (the top one, and every second one below it) that is
///     NeighType.ForceOpponentDiscard (Neigh, Motherfucker!) makes the owner of the card it
///     cancelled discard one card. Those discards run first, then the contested card is resolved
///     or discarded, then the turn phase advances.
///
/// Created at runtime by CardActionExecutor (AddComponent) so no scene wiring is needed; it reads
/// its manager references from CardActionExecutor.Instance.
/// </summary>
public class NeighManager : MonoBehaviour
{
    public static NeighManager Instance { get; private set; }

    private TurnManager turnManager;
    private CardManager cardManager;

    // --- contest state ---
    private Card contestedCard;
    private HandStable contestedSourceHand;
    private Player contestCaster;     // played the contested card (stays turnManager.activePlayer throughout)
    private Player contestOpponent;   // the other player — first eligible responder
    private readonly List<Card> neighStack = new List<Card>();

    /// <summary>The player whose turn it is to respond (play a Neigh or pass).</summary>
    public Player Responder { get; private set; }

    /// <summary>True while a click on a Neigh (or the Pass button) is expected.</summary>
    public bool AwaitingResponse { get; private set; }

    /// <summary>True from the moment a contest opens until it is fully resolved (covers the
    /// post-cancel discard step, during which AwaitingResponse is already false).</summary>
    public bool ContestPending { get; private set; }

    public string ContestedCardName => contestedCard != null ? contestedCard.name : string.Empty;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        CardActionExecutor executor = CardActionExecutor.Instance;
        if (executor != null)
        {
            turnManager = executor.turnManager;
            cardManager = executor.cardManager;
        }
    }

    /// <summary>
    /// Opens a contest if this play can be Neigh'd right now. Returns false (play resolves as
    /// normal) when: the mechanic isn't ready, it isn't a plain Action-phase hand play, the card
    /// is itself a Neigh, there's no opponent, or the opponent holds no Neigh.
    /// </summary>
    public bool TryBeginContest(Card card, HandStable sourceHand)
    {
        if (turnManager == null || cardManager == null) return false;
        if (ContestPending) return false;
        if (turnManager.currentPhase != TurnPhase.Action) return false;

        // Not a direct hand play (e.g. A Little Side Hustle bringing in an Upgrade mid-effect).
        if (CardActionExecutor.Instance != null
            && CardActionExecutor.Instance.currentPendingAction != PendingActionType.None)
            return false;

        if (card.cardData.cardType == CardType.NEIGH) return false;

        Player caster = turnManager.activePlayer;
        Player opponent = OtherPlayer(caster);
        if (opponent == null || !HandHasNeigh(opponent)) return false;

        contestedCard = card;
        contestedSourceHand = sourceHand;
        contestCaster = caster;
        contestOpponent = opponent;
        neighStack.Clear();
        Responder = opponent;
        AwaitingResponse = true;
        ContestPending = true;

        Debug.Log($"[Neigh] {opponent.name} may Neigh {caster.name}'s {card.name}.");
        return true;
    }

    /// <summary>The responder plays a Neigh from their hand.</summary>
    public void SubmitNeigh(Card neighCard, HandStable responderHand)
    {
        if (!AwaitingResponse) return;
        NeighCardData neighData = neighCard.cardData as NeighCardData;
        if (neighData == null) return;

        cardManager.MoveCard(neighCard, responderHand, cardManager.discardPile);
        responderHand.RepositionCards();
        neighStack.Add(neighCard);
        Debug.Log($"[Neigh] {Responder.name} plays {neighCard.name} (chain depth {neighStack.Count}).");

        if (!neighData.CanBeNeighed)
        {
            Debug.Log($"[Neigh] {neighCard.name} cannot be Neigh'd — chain ends.");
            ResolveContest();
            return;
        }

        Player next = OtherPlayer(Responder);
        if (next != null && HandHasNeigh(next))
        {
            Responder = next;
            Debug.Log($"[Neigh] {next.name} may counter-Neigh.");
        }
        else
        {
            ResolveContest();
        }
    }

    /// <summary>The responder declines to Neigh (Skip/Pass button).</summary>
    public void Pass()
    {
        if (!AwaitingResponse) return;
        Debug.Log($"[Neigh] {Responder.name} passes.");
        ResolveContest();
    }

    private void ResolveContest()
    {
        AwaitingResponse = false;

        int n = neighStack.Count;
        bool contestedCardCancelled = (n % 2) == 1;

        // A Neigh is "live" if nothing live sits directly on top of it — i.e. its distance from
        // the top of the stack is even (top itself, top-2, top-4, ...).
        var postCancelActions = new List<CardAction>();
        for (int k = n - 1; k >= 0; k--)
        {
            int distanceFromTop = (n - 1) - k;
            if (distanceFromTop % 2 != 0) continue; // dead — was itself countered

            if (neighStack[k].cardData is NeighCardData nd && nd.neighType == NeighType.ForceOpponentDiscard)
            {
                // Owner of the card this Neigh cancelled: the contested card (caster) for k == 0,
                // otherwise the player who played neighStack[k - 1].
                Player target = (k == 0) ? contestCaster : PlayerOfNeigh(k - 1);
                postCancelActions.Add(new DiscardCardAction
                {
                    targetPlayer = (target == contestCaster)
                        ? DiscardCardAction.TargetPlayer.ActivePlayer
                        : DiscardCardAction.TargetPlayer.Opponent,
                    selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                    numberOfCards = 1
                });
            }
        }

        Debug.Log($"[Neigh] Resolving: {n} Neigh(s) played, contested card "
                  + (contestedCardCancelled ? "CANCELLED" : "RESOLVES")
                  + $", {postCancelActions.Count} forced discard(s).");

        System.Action finish = () =>
        {
            if (contestedCardCancelled)
            {
                cardManager.MoveCard(contestedCard, contestedSourceHand, cardManager.discardPile);
                contestedSourceHand.RepositionCards();
                Card cancelled = contestedCard;
                ClearContestState();
                Debug.Log($"[Neigh] {cancelled.name} was Neigh'd — discarded, effect skipped.");
                turnManager.StartNextTurnPhase(SpecialActionType.NONE);
            }
            else
            {
                Card resolving = contestedCard;
                HandStable sourceHand = contestedSourceHand;
                ClearContestState();
                cardManager.ResolvePlay(resolving, sourceHand);
                sourceHand.RepositionCards();

                bool hasPending = CardActionExecutor.Instance != null
                    && CardActionExecutor.Instance.currentPendingAction != PendingActionType.None;
                turnManager.StartNextTurnPhase(
                    hasPending ? resolving.cardData.specialActionType : SpecialActionType.NONE);
            }
        };

        if (postCancelActions.Count > 0)
        {
            // Runs during TurnPhase.Action, so CardActionExecutor won't auto-advance the phase
            // when the queue drains — our callback does the rest.
            CardActionExecutor.Instance.ExecuteActions(postCancelActions, contestedCard, finish);
        }
        else
        {
            finish();
        }
    }

    // Player who played neighStack[k]: the opponent responds first (k == 0), then it alternates.
    private Player PlayerOfNeigh(int k) => (k % 2 == 0) ? contestOpponent : contestCaster;

    private void ClearContestState()
    {
        contestedCard = null;
        contestedSourceHand = null;
        contestCaster = null;
        contestOpponent = null;
        neighStack.Clear();
        Responder = null;
        ContestPending = false;
    }

    private Player OtherPlayer(Player p) => turnManager.players.FirstOrDefault(x => x != p);

    private static bool HandHasNeigh(Player p) =>
        p.handStable.spaceCards.Any(c => c.cardData is NeighCardData);
}
