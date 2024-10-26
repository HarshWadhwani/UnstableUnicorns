using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stable : MonoBehaviour
{
    public List<Card> stableCards;
    public int maxCardsInStable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCardToStable(Card card)
    {
        if (stableCards.Count < maxCardsInStable)
        {
            stableCards.Add(card);
            card.transform.SetParent(transform, false);
            PositionCardsInStable();
            //PositionCardInStable(card);
        }
        else
        {
            Debug.LogWarning("Stable is full. Cannot add more cards.");
        }
    }

    protected virtual void PositionCardsInStable()
    {

    }

    protected virtual void PositionCardInStable(Card card)
    {
        RectTransform stableRect = GetComponent<RectTransform>();
        float stableWidth = stableRect.rect.width;

        Debug.Log("stableWidth is " + stableWidth);

        float cardSlotWidth = stableWidth / maxCardsInStable;

        Debug.Log("cardSlotWidth is " + cardSlotWidth);

        RectTransform cardRect = card.GetComponent<RectTransform>();

        Debug.Log("cardRect.rect.width is " + cardRect.rect.width);

        float cardBuffer = (cardSlotWidth - cardRect.rect.width) / 2;

        Debug.Log("cardBuffer is " + cardBuffer);

        float openSlot = (stableCards.Count - 1) * 2 * cardBuffer + cardRect.rect.width;

        float xPos = stableRect.anchoredPosition.x - (stableWidth / 2) + cardBuffer + (cardRect.rect.width / 2) + openSlot;

        Debug.Log("xPos is " + xPos);

        cardRect.anchoredPosition = new Vector2(xPos, 0);

        // Set the card's parent to be the stable, keeping its local scale
        card.transform.SetParent(transform, false);
    }
}
