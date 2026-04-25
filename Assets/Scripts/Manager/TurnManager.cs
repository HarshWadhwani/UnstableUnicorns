using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;
    public Player activePlayer;
    public TurnPhase currentPhase;

    // Start is called before the first frame update
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
                currentPhase = TurnPhase.Draw;
                break;
        }
    }

    private void AdvanceToNextPlayerTurn()
    {
        SwitchToNextPlayer();
        List<CardAction> everyTurnActions = CollectEveryTurnActions();
        if (everyTurnActions.Count > 0)
        {
            currentPhase = TurnPhase.EveryTurnSpecial;
            CardActionExecutor.Instance.ExecuteActions(everyTurnActions, null);
        }
        else
        {
            currentPhase = TurnPhase.Draw;
        }
    }

    private List<CardAction> CollectEveryTurnActions()
    {
        List<CardAction> actions = new List<CardAction>();
        CollectEveryTurnActionsFromSpace(activePlayer.unicornStable, actions);
        CollectEveryTurnActionsFromSpace(activePlayer.upgradeStable, actions);
        CollectEveryTurnActionsFromSpace(activePlayer.downgradeStable, actions);
        return actions;
    }

    private void CollectEveryTurnActionsFromSpace(CardSpace cardSpace, List<CardAction> actions)
    {
        foreach (Card card in cardSpace.spaceCards)
        {
            if (card.cardData.specialActionType == SpecialActionType.EVERY_TURN)
            {
                actions.AddRange(card.cardData.actions);
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
