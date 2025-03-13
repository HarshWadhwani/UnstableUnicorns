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
        if (allowedTurnPhases.Contains(turnManager.currentPhase))
        {
            bool isCardplayed = PlayCard(card);
            if (isCardplayed)
            {
                PositionCardsInStable();
                turnManager.StartNextTurnPhase();
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
        
        return cardManager.PlayCardForCurrentPlayer(card, this);
    }

    protected override void PositionCardsInStable()
    {
        RectTransform stableRect = GetComponent<RectTransform>();

        float cardSlotAngle = fanTotalAngle / spaceCards.Count;

        for (int i = 0; i < spaceCards.Count; i++)
        {
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();
            
            float openSlot = i * cardSlotAngle;
            float zRotation = stableRect.localEulerAngles.z + 70 - (cardSlotAngle / 2) - openSlot;

            var xPosition = stableRect.anchoredPosition.x + GetXPositionOffsetValue(spaceCards.Count, i);

            var yPosition = GetYPositionOffsetValue(spaceCards.Count, i);

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
