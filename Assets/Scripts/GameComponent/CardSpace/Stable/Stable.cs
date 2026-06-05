using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stable : CardSpace
{
    public int maxCardsInStable;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleCardClick(Card card)
    {
        if (CardActionExecutor.Instance == null) return;

        if (CardActionExecutor.Instance.currentPendingAction == PendingActionType.DestroyCard)
        {
            if (player != CardActionExecutor.Instance.pendingDestroyTargetPlayer)
            {
                Debug.LogWarning("Must target the opponent's stable.");
                return;
            }

            CardActionExecutor.Instance.ExecutePendingAction(card);
            PositionCardsInStable();
            return;
        }

        if (CardActionExecutor.Instance.currentPendingAction == PendingActionType.DestroyUnicornCard)
        {
            if (player != CardActionExecutor.Instance.pendingDestroyTargetPlayer)
            {
                Debug.LogWarning("Must target the opponent's stable.");
                return;
            }

            if (!(this is UnicornStable))
            {
                Debug.LogWarning("Must target a unicorn card.");
                return;
            }

            CardActionExecutor.Instance.ExecutePendingAction(card);
            PositionCardsInStable();
            return;
        }

        if (CardActionExecutor.Instance.currentPendingAction == PendingActionType.StealCard)
        {
            if (player == turnManager.activePlayer)
            {
                Debug.LogWarning("Cannot steal from your own stable.");
                return;
            }

            CardActionExecutor.Instance.ExecutePendingAction(card);
            PositionCardsInStable();
            return;
        }

        if (turnManager.currentPhase == TurnPhase.EveryTurnSpecial
            && CardActionExecutor.Instance.currentPendingAction == PendingActionType.None
            && player == turnManager.activePlayer)
        {
            turnManager.TryActivateEveryTurnCard(card);
        }
    }

    public override void AddCard(Card card)
    {
        if (spaceCards.Count >= maxCardsInStable)
        {
            Debug.LogWarning("Stable is full. Cannot add more cards.");
            return;
        }

        base.AddCard(card);
        PositionCardsInStable();
    }

    protected virtual void PositionCardsInStable()
    {
        RectTransform stableRect = GetComponent<RectTransform>();
        float cardSlotWidth = stableRect.rect.width / maxCardsInStable;
        float startX = stableRect.anchoredPosition.x - (stableRect.rect.width / 2) + cardSlotWidth / 2;
        float cardRotation = cardPrefab.GetComponent<RectTransform>().localEulerAngles.z;

        for (int i = 0; i < spaceCards.Count; i++)
        {
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector2(startX + i * cardSlotWidth, 0);
            cardRect.localEulerAngles = new Vector3(0, 0, cardRotation);
        }
    }
}
