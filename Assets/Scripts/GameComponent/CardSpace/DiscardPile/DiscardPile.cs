using UnityEngine;

public class DiscardPile : CardSpace
{
    public override void HandleCardClick(Card card)
    {
        
    }

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        
        card.HideCard();
        
        RectTransform cardRect = card.GetComponent<RectTransform>();
        if (cardRect != null)
        {
            cardRect.anchoredPosition = Vector2.zero;
            cardRect.localEulerAngles = Vector3.zero;
            cardRect.localScale = Vector3.one;
        }
    }
}
