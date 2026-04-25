using UnityEngine;

public abstract class StackedStable : Stable
{
    protected abstract bool FromRightEdge { get; }

    protected sealed override void PositionCardsInStable()
    {
        if (spaceCards.Count == 0) return;

        var stableRect = GetComponent<RectTransform>();
        var cardWidth = spaceCards[0].cardFront.GetComponent<RectTransform>().rect.width;
        var overlap = 0.5f * cardWidth;

        var startingX = FromRightEdge ? stableRect.rect.width - cardWidth / 2f : cardWidth / 2f;
        var direction = FromRightEdge ? -1 : 1;

        for (var i = 0; i < spaceCards.Count; i++)
        {
            var cardRect = spaceCards[i].GetComponent<RectTransform>();
            cardRect.anchorMin = new Vector2(0, 0.5f);
            cardRect.anchorMax = new Vector2(0, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);
            cardRect.localScale = Vector3.one;
            cardRect.localEulerAngles = Vector3.zero;
            cardRect.anchoredPosition = new Vector2(startingX + direction * i * overlap, 0);
        }
    }
}
