using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowngradeStable : Stable
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

        var firstCardRect = spaceCards[0].cardFront.GetComponent<RectTransform>();
        var cardWidth = firstCardRect.rect.width;
        var overlap = 0.5f * cardWidth; // 20% overlap between cards

        // Compute the base position: center of rightmost card is at (right edge - (cardWidth / 2))
        var leftEdgeX = 0f;
        var startingX = leftEdgeX + (cardWidth / 2);

        for (var i = 0; i < spaceCards.Count; i++)
        {
            var cardRect = spaceCards[i].GetComponent<RectTransform>();

            // Set anchors and pivot to left-center to make math easier
            cardRect.anchorMin = new Vector2(0, 0.5f);
            cardRect.anchorMax = new Vector2(0, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);  // center pivot
            cardRect.localScale = Vector3.one;
            cardRect.localEulerAngles = Vector3.zero;

            // Compute position
            var xOffset = i * overlap;
            var xPos = startingX + xOffset;

            cardRect.anchoredPosition = new Vector2(xPos, 0);
        }
    }
}
