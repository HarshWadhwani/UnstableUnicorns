using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandStable : Stable
{
    [Tooltip("Total spread, in degrees, across a full 7-card hand. Fewer cards fan proportionally less; one card is held straight up.")]
    public float fanTotalAngle = 90f;

    [Tooltip("Horizontal gap between neighbouring cards in the fan, in canvas units.")]
    public float fanCardSpacing = 56f;

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
        int n = Mathf.Min(spaceCards.Count, 7);
        if (n == 0) return;

        RectTransform stableRect = GetComponent<RectTransform>();
        float baseZ = stableRect.localEulerAngles.z;
        float baseX = stableRect.anchoredPosition.x;

        // One card sits straight up. Each extra card opens the fan by `step` degrees, up to
        // fanTotalAngle across a full 7-card hand — so the arc grows and shrinks with the hand.
        float step = fanTotalAngle / 6f;
        float mid = (n - 1) / 2f;                 // 0 for one card, 3 for seven
        float yArc = 0.9f * step * (n - 1);       // how far the outer cards dip; 0 for a lone card

        for (int i = 0; i < n; i++)
        {
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();

            float offset = i - mid;                        // -mid .. +mid  (0 for a lone card)
            float frac = mid > 0f ? offset / mid : 0f;     // -1 .. +1

            float angle = -offset * step;                  // left card tilts CCW, right card CW
            float x = baseX + offset * fanCardSpacing;
            float y = -yArc * frac * frac;                 // centre card highest, edges dip down

            cardRect.anchoredPosition = new Vector2(x, y);
            cardRect.localEulerAngles = new Vector3(
                stableRect.localEulerAngles.x, stableRect.localEulerAngles.y, baseZ + angle);
        }
    }
}
