using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stable : CardSpace
{
    public int maxCardsInStable;

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

    public virtual void AddCardToStable(Card card)
    {
        if (spaceCards.Count < maxCardsInStable)
        {
            AddCard(card);
            PositionCardsInStable();
        }
        else
        {
            Debug.LogWarning("Stable is full. Cannot add more cards.");
        }
    }

    protected virtual void PositionCardsInStable()
    {
        RectTransform stableRect = GetComponent<RectTransform>();
        float stableWidth = stableRect.rect.width;
        float cardSlotWidth = stableWidth / maxCardsInStable;

        float leftMostOpenPosition = stableRect.anchoredPosition.x - (stableWidth / 2) + cardSlotWidth / 2;

        for (int i = 0; i < spaceCards.Count; i++) {

            if (spaceCards.Count > 1 && i > 0) {
                leftMostOpenPosition += cardSlotWidth;
            }
            
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();

            cardRect.anchoredPosition = new Vector2(leftMostOpenPosition, 0);
            cardRect.localEulerAngles = new Vector3(0, 0, cardPrefab.GetComponent<RectTransform>().localEulerAngles.z);
        }
    }
}
