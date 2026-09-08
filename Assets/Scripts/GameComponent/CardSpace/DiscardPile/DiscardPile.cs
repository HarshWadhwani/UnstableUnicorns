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
            // Cards coming from a StackedStable (Upgrade/Downgrade) were left-edge anchored
            // (anchorMin/Max = (0, 0.5)) for the overlapping-stack layout. Restore centered
            // anchoring/pivot so anchoredPosition = zero actually centers the card here.
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);
            cardRect.anchoredPosition = Vector2.zero;
            cardRect.localEulerAngles = Vector3.zero;
            cardRect.localScale = Vector3.one;
        }
    }
}
