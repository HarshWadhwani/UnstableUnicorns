using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stable : CardSpace
{
    public int maxCardsInStable;

    public Card cardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AddCardToStable(Card card)
    {
        if (spaceCards.Count < maxCardsInStable)
        {
            AddCardToSpace(card);
            PositionCardsInStable();
        }
        else
        {
            Debug.LogWarning("Stable is full. Cannot add more cards.");
        }
    }

    public override void HandleCardClick(Card card)
    {
    }

    protected virtual void PositionCardsInStable()
    {


        RectTransform stableRect = GetComponent<RectTransform>();
        float stableWidth = stableRect.rect.width;
        Debug.Log("stableWidth is: " + stableWidth);

        float cardSlotWidth = stableWidth / maxCardsInStable;
        Debug.Log("cardSlotWidth is: " + cardSlotWidth);
        float leftMostOpenPosition = stableRect.anchoredPosition.x - (stableWidth / 2) + cardSlotWidth / 2;
        Debug.Log("leftMostOpenPosition is: " + leftMostOpenPosition);

        Debug.Log("stableCards.Count is: " + spaceCards.Count);
        for (int i = 0; i < spaceCards.Count; i++) {

            if (spaceCards.Count > 1 && i > 0) {
                leftMostOpenPosition += cardSlotWidth;
            }
            
            Debug.Log("leftMostOpenPosition in the loop is: " + leftMostOpenPosition);
            RectTransform cardRect = spaceCards[i].GetComponent<RectTransform>();
            Debug.Log("cardRect is: " + cardRect);
            cardRect.anchoredPosition = new Vector2(leftMostOpenPosition, 0);
            cardRect.localEulerAngles = new Vector3(0, 0, cardPrefab.GetComponent<RectTransform>().localEulerAngles.z);
            Debug.Log("cardRect.anchoredPosition is: " + cardRect.anchoredPosition);
        }



        //foreach (var card in stableCards)
        //{
        //    RectTransform cardRect = card.GetComponent<RectTransform>();
        //    float cardWidth = cardRect.rect.width;
        //    float paddedCardWidth = 

        //    float cardBuffer = (cardSlotWidth - cardRect.rect.width) / 2;
        //    float openSlot = (i - 1) * 2 * cardBuffer + cardRect.rect.width;
        //    float xPos = stableRect.anchoredPosition.x - (stableWidth / 2) carSlotWidth
        //    //float xPos = stableRect.anchoredPosition.x - (stableWidth / 2) + cardBuffer + (cardRect.rect.width / 2) + openSlot;

        //    cardRect.anchoredPosition = new Vector2(xPos, 0);

        //    // Set the card's parent to be the stable, keeping its local scale
        //    card.transform.SetParent(transform, false);
        //}
    }
}
