using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;
    public Player activePlayer;
    public TurnPhase currentPhase;
    public Card currentEveryTurnCard;
    public bool skipNextDrawPhase = false;

    // Downgrade EVERY_TURN cards — forced on the active player, auto-fire in order, cannot be skipped.
    private List<Card> pendingMandatoryCards = new List<Card>();
    // Unicorn/Upgrade EVERY_TURN cards — player clicks to activate, or presses Skip to bypass the rest.
    private List<Card> pendingChoiceCards = new List<Card>();

    // UI hook (e.g. PhaseIndicator) — whether the Skip button should be interactable right now.
    public bool CanSkipEveryTurnPhase =>
        currentPhase == TurnPhase.EveryTurnSpecial
        && pendingMandatoryCards.Count == 0
        && (CardActionExecutor.Instance == null || CardActionExecutor.Instance.currentPendingAction == PendingActionType.None);

    void Start()
    {
        foreach (Player player in players)
        {
            player.handStable.turnManager = this;
            player.unicornStable.turnManager = this;
        }

        activePlayer = players[0];
        currentPhase = TurnPhase.Draw;
    }

    public void StartNextTurnPhase(SpecialActionType specialActionType = SpecialActionType.NONE)
    {
        switch (currentPhase)
        {
            case TurnPhase.Draw:
                currentPhase = TurnPhase.Action;
                break;

            case TurnPhase.Action:
                if (specialActionType == SpecialActionType.IMMEDIATE)
                {
                    currentPhase = TurnPhase.ImmediateSpecial;
                }
                else
                {
                    AdvanceToNextPlayerTurn();
                }
                break;

            case TurnPhase.ImmediateSpecial:
                AdvanceToNextPlayerTurn();
                break;

            case TurnPhase.EveryTurnSpecial:
                currentEveryTurnCard = null;
                AdvanceEveryTurnSpecial();
                break;
        }
    }

    // Called both to kick off the phase and to chain to the next card once the current one's
    // action queue finishes (via CardActionExecutor.ExecuteNextAction -> StartNextTurnPhase).
    private void AdvanceEveryTurnSpecial()
    {
        if (pendingMandatoryCards.Count > 0)
        {
            ActivateNextMandatoryCard();
            return;
        }

        if (pendingChoiceCards.Count == 0)
        {
            bool skip = skipNextDrawPhase;
            skipNextDrawPhase = false;
            currentPhase = skip ? TurnPhase.Action : TurnPhase.Draw;
        }
        // else: stay in EveryTurnSpecial — player must click remaining choice cards or press Skip
    }

    private void ActivateNextMandatoryCard()
    {
        Card card = pendingMandatoryCards[0];
        pendingMandatoryCards.RemoveAt(0);
        currentEveryTurnCard = card;
        // Actions are responsible for skipping themselves silently when impossible
        // (e.g. DiscardCardAction with an empty hand) — no guard needed here.
        CardActionExecutor.Instance.ExecuteActions(card.cardData.actions, card);
    }

    public bool TryActivateEveryTurnCard(Card card)
    {
        // Mandatory Downgrade effects must resolve before any optional card can be triggered.
        if (pendingMandatoryCards.Count > 0) return false;
        if (!pendingChoiceCards.Contains(card)) return false;
        pendingChoiceCards.Remove(card);
        currentEveryTurnCard = card;
        CardActionExecutor.Instance.ExecuteActions(card.cardData.actions, card);
        return true;
    }

    public void SkipEveryTurnPhase()
    {
        if (pendingMandatoryCards.Count > 0)
        {
            Debug.LogWarning("Cannot skip: mandatory Downgrade effects must resolve first.");
            return;
        }

        if (CardActionExecutor.Instance != null
            && CardActionExecutor.Instance.currentPendingAction != PendingActionType.None)
        {
            Debug.LogWarning("Cannot skip while a card action is awaiting input.");
            return;
        }

        pendingChoiceCards.Clear();
        currentEveryTurnCard = null;
        bool skip = skipNextDrawPhase;
        skipNextDrawPhase = false;
        currentPhase = skip ? TurnPhase.Action : TurnPhase.Draw;
    }

    private void AdvanceToNextPlayerTurn()
    {
        SwitchToNextPlayer();
        pendingMandatoryCards.Clear();
        pendingChoiceCards.Clear();
        CollectEveryTurnCards(activePlayer.downgradeStable, pendingMandatoryCards);
        CollectEveryTurnCards(activePlayer.unicornStable, pendingChoiceCards);
        CollectEveryTurnCards(activePlayer.upgradeStable, pendingChoiceCards);

        if (pendingMandatoryCards.Count > 0)
        {
            currentPhase = TurnPhase.EveryTurnSpecial;
            ActivateNextMandatoryCard();
        }
        else if (pendingChoiceCards.Count > 0)
        {
            currentPhase = TurnPhase.EveryTurnSpecial;
        }
        else
        {
            currentPhase = TurnPhase.Draw;
        }
    }

    private void CollectEveryTurnCards(CardSpace cardSpace, List<Card> destination)
    {
        foreach (Card card in cardSpace.spaceCards)
        {
            if (card.cardData.specialActionType == SpecialActionType.EVERY_TURN
                && card.cardData.actions != null
                && card.cardData.actions.Count > 0)
            {
                destination.Add(card);
            }
        }
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
