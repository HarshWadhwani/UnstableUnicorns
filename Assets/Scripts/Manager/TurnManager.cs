using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;
    public Player activePlayer;
    public TurnPhase currentPhase;
    public Card currentEveryTurnCard;

    private List<Card> everyTurnCardsPending = new List<Card>();

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
                if (everyTurnCardsPending.Count == 0)
                {
                    currentPhase = TurnPhase.Draw;
                }
                // else: stay in EveryTurnSpecial — player must click remaining cards or press Skip
                break;
        }
    }

    public bool TryActivateEveryTurnCard(Card card)
    {
        if (!everyTurnCardsPending.Contains(card)) return false;
        everyTurnCardsPending.Remove(card);
        currentEveryTurnCard = card;
        CardActionExecutor.Instance.ExecuteActions(card.cardData.actions, card);
        return true;
    }

    public void SkipEveryTurnPhase()
    {
        everyTurnCardsPending.Clear();
        currentEveryTurnCard = null;
        currentPhase = TurnPhase.Draw;
    }

    private void AdvanceToNextPlayerTurn()
    {
        SwitchToNextPlayer();
        everyTurnCardsPending.Clear();
        CollectEveryTurnCards(activePlayer.unicornStable);
        CollectEveryTurnCards(activePlayer.upgradeStable);
        CollectEveryTurnCards(activePlayer.downgradeStable);

        if (everyTurnCardsPending.Count > 0)
        {
            currentPhase = TurnPhase.EveryTurnSpecial;
        }
        else
        {
            currentPhase = TurnPhase.Draw;
        }
    }

    private void CollectEveryTurnCards(CardSpace cardSpace)
    {
        foreach (Card card in cardSpace.spaceCards)
        {
            if (card.cardData.specialActionType == SpecialActionType.EVERY_TURN
                && card.cardData.actions != null
                && card.cardData.actions.Count > 0)
            {
                everyTurnCardsPending.Add(card);
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
