using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stable : CardSpace
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
        if (spaceCards.Count < maxCardsInStable)
        {
            this.AddCardToSpace(card);
            PositionCardsInStable(card);
        }
        else
        {
            Debug.LogWarning("Stable is full. Cannot add more cards.");
        }
    }

    public override void HandleCardClick(Card card)
    {
    }

    protected virtual void PositionCardsInStable(Card card)
    {
        Debug.Log("Position in Unicorn");
        RectTransform stableRect = GetComponent<RectTransform>();
        float stableWidth = stableRect.rect.width;

        float cardSlotWidth = stableWidth / maxCardsInStable;

        RectTransform cardRect = card.GetComponent<RectTransform>();

        float cardBuffer = (cardSlotWidth - cardRect.rect.width) / 2;

        float openSlot = (spaceCards.Count - 1) * 2 * cardBuffer + cardRect.rect.width;

        float xPos = stableRect.anchoredPosition.x - (stableWidth / 2) + cardBuffer + (cardRect.rect.width / 2) + openSlot;

        cardRect.anchoredPosition = new Vector2(xPos, 0);

        // Set the card's parent to be the stable, keeping its local scale
        card.transform.SetParent(transform, false);
    }
}
