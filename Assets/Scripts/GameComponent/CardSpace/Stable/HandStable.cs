using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandStable : Stable
{
    public float fanTotalAngle = 140f;

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
        // A Neigh contest is open: only the current responder may act, and only by playing a
        // Neigh from their hand (or pressing the Skip/Pass button, handled in PhaseIndicator).
        if (NeighManager.Instance != null && NeighManager.Instance.AwaitingResponse)
        {
            if (player == NeighManager.Instance.Responder && card.cardData is NeighCardData)
            {
                NeighManager.Instance.SubmitNeigh(card, this);
            }
            else
            {
                Debug.LogWarning($"Waiting for {NeighManager.Instance.Responder.name} to play a Neigh or pass.");
            }
            return;
        }

        if (CardActionExecutor.Instance != null && CardActionExecutor.Instance.currentPendingAction != PendingActionType.None)
        {
            if (player != turnManager.activePlayer)
            {
                Debug.LogWarning("Card does not belong to the active player.");
                return;
            }

            if (CardActionExecutor.Instance.currentPendingAction == PendingActionType.DiscardCard ||
                CardActionExecutor.Instance.currentPendingAction == PendingActionType.GiveCard)
            {
                CardActionExecutor.Instance.ExecutePendingAction(card);
                PositionCardsInStable();
                Debug.Log("Hand stable is not starting next turn phase");
                return;
            }

            if (CardActionExecutor.Instance.currentPendingAction == PendingActionType.PlayCardFromHand)
            {
                CardType? typeFilter = CardActionExecutor.Instance.pendingPlayCardTypeFilter;
                if (typeFilter.HasValue && card.cardData.cardType != typeFilter.Value)
                {
                    Debug.LogWarning($"Must select a {typeFilter.Value} card.");
                    return;
                }

                UnicornType? subtypeFilter = CardActionExecutor.Instance.pendingPlayCardSubtypeFilter;
                if (subtypeFilter.HasValue
                    && (!(card.cardData is UnicornCardData u) || u.unicornType != subtypeFilter.Value))
                {
                    Debug.LogWarning($"Must select a {subtypeFilter.Value} unicorn.");
                    return;
                }

                CardActionExecutor.Instance.ExecutePendingAction(card);
                PositionCardsInStable();
                Debug.Log("Hand stable is not starting next turn phase");
                return;
            }
        }

        if (allowedTurnPhases.Contains(turnManager.currentPhase))
        {
            bool isCardPlayed = PlayCard(card);
            if (isCardPlayed)
            {
                PositionCardsInStable();

                // A Neigh contest opened on this play — NeighManager owns resolution and the
                // turn-phase advance from here. Don't advance the phase ourselves.
                if (NeighManager.Instance != null && NeighManager.Instance.ContestPending)
                {
                    Debug.Log("Play is being contested by a Neigh — deferring phase advance.");
                    return;
                }

                Debug.Log("Hand stable is starting next turn phase");
                // Synchronous IMMEDIATE actions complete before we get here, leaving no pending
                // action. In that case, treat as NONE so we don't park in ImmediateSpecial.
                bool hasPendingAction = CardActionExecutor.Instance != null &&
                                        CardActionExecutor.Instance.currentPendingAction != PendingActionType.None;
                SpecialActionType effectiveType = hasPendingAction
                    ? card.cardData.specialActionType
                    : SpecialActionType.NONE;
                turnManager.StartNextTurnPhase(effectiveType);
            }
        }
    }

    private bool PlayCard(Card card)
    {
        if (player != turnManager.activePlayer)
        {
            Debug.LogWarning("player is not active");
            return false;
        }
        //TODO 12-15 : follow this trail to find how to set current pending action for card executor correctly
        return cardManager.PlayCardForCurrentPlayer(card, this);
    }

    protected override void PositionCardsInStable()
    {
        int displayCount = Mathf.Min(spaceCards.Count, 7);
        RectTransform stableRect = GetComponent<RectTransform>();

        float cardSlotAngle = fanTotalAngle / displayCount;

        for (int i = 0; i < displayCount; i++)
        {
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();

            float openSlot = i * cardSlotAngle;
            float zRotation = stableRect.localEulerAngles.z + 70 - (cardSlotAngle / 2) - openSlot;

            var xPosition = stableRect.anchoredPosition.x + GetXPositionOffsetValue(displayCount, i);
            var yPosition = GetYPositionOffsetValue(displayCount, i);

            cardRect.anchoredPosition = new Vector2(xPosition, yPosition);
            cardRect.localEulerAngles = new Vector3(stableRect.localEulerAngles.x, stableRect.localEulerAngles.y, zRotation);
        }
    }

    private int GetXPositionOffsetValue(int numberOfCards, int currentCardIndex)
    {
        var xPositionOffset = (currentCardIndex - (numberOfCards / 2)) * 30;
        if (numberOfCards % 2 == 0)
        {
            xPositionOffset += 15;
        }

        return xPositionOffset;
    }

    private static int GetYPositionOffsetValue(int numberOfCards, int currentCardIndex)
    {
        List<int> values = new List<int>();

        switch (numberOfCards)
        {
            case 1:
                values.Add(0);
                break;
            case 2:
                values.AddRange(new int[] { 0, 0 });
                break;
            case 3:
                values.AddRange(new int[] { 0, 15, 0 });
                break;
            case 4:
                values.AddRange(new int[] { 0, 25, 25, 0 });
                break;
            case 5:
                values.AddRange(new int[] { 0, 30, 40, 30, 0 });
                break;
            case 6:
                values.AddRange(new int[] { 0, 30, 45, 45, 30, 0 });
                break;
            case 7:
                values.AddRange(new int[] { 0, 30, 50, 55, 50, 30, 0 });
                break;
            case 8:
                values.AddRange(new int[] { 0, 30, 55, 60, 60, 55, 30, 0 });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(numberOfCards), "numberOfCards must be between 1 and 8");
        }

        // Return the value at the currentCard position in the list
        if (currentCardIndex >= 0 && currentCardIndex < values.Count)
        {
            return values[currentCardIndex];
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(currentCardIndex), "currentCard must be within the list range");
        }
    }
}
