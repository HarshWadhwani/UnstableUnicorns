using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStable : Stable
{

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
    }

    protected override void PositionCardsInStable()
    {
        if (spaceCards.Count == 0) return;

        RectTransform firstCardRect = spaceCards[0].cardFront.GetComponent<RectTransform>();
        float cardWidth = firstCardRect.rect.width;
        float overlap = 0.5f * cardWidth; // 20% overlap between cards

        // Compute the base position: center of rightmost card is at (right edge - 2 * cardWidth)
        RectTransform stableRect = GetComponent<RectTransform>();
        float rightEdgeX = stableRect.rect.width;
        Debug.Log(rightEdgeX);
        Debug.Log(cardWidth);
        float startingX = rightEdgeX - (cardWidth);

        for (int i = 0; i < spaceCards.Count; i++)
        {
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();

            // Set anchors and pivot to left-center to make math easier
            cardRect.anchorMin = new Vector2(0, 0.5f);
            cardRect.anchorMax = new Vector2(0, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);  // center pivot
            cardRect.localScale = Vector3.one;
            cardRect.localEulerAngles = Vector3.zero;

            // Compute position
            float xOffset = -i * overlap;
            float xPos = startingX + xOffset;

            cardRect.anchoredPosition = new Vector2(xPos, 0);
        }
    }
}
