using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornStable : Stable
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public override void HandleCardClick(Card card)
    {
        if (CardActionExecutor.Instance != null && CardActionExecutor.Instance.currentPendingAction == PendingActionType.DestroyCard)
        {
            Debug.Log("player " + player);
            Debug.Log("turnManager.activePlayer " + turnManager.activePlayer);
            if (player == turnManager.activePlayer)
            {
                Debug.LogWarning("Cannot destroy your own cards.");
                return;
            }

            CardActionExecutor.Instance.ExecutePendingAction(card);
            PositionCardsInStable();
        }
    }

    public void CheckWinCondition()
    {
        if (spaceCards.Count == maxCardsInStable)
        {
            Debug.Log("Player wins!");
        }
    }
}

