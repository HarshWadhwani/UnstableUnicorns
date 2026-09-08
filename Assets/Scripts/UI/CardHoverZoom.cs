using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Lifts and enlarges a hand card on hover so its rules text is readable without
/// shrinking the fan. Only acts while the card is in a <see cref="HandStable"/>;
/// cards in stables, the deck, or the discard pile are untouched. Cosmetic only —
/// clicks still fall through to <see cref="Card"/>'s own click handler.
/// </summary>
[RequireComponent(typeof(Card))]
public class CardHoverZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoom = 1.7f;
    public float lift = 46f;   // local Y offset while hovered
    public float speed = 14f;  // lerp rate

    private Card card;
    private RectTransform rt;
    private Vector3 baseScale;
    private Vector2 baseAnchoredPos;
    private int baseSiblingIndex = -1;
    private bool hovered;
    private bool captured;

    void Awake()
    {
        card = GetComponent<Card>();
        rt = GetComponent<RectTransform>();
    }

    void OnDisable()
    {
        hovered = false;
        Restore();
    }

    private bool InHand => card != null && card.cardSpace is HandStable;

    void Update()
    {
        if (hovered && InHand)
        {
            if (!captured) Capture();
            rt.localScale = Vector3.Lerp(rt.localScale, baseScale * zoom, Time.deltaTime * speed);
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, baseAnchoredPos + Vector2.up * lift, Time.deltaTime * speed);
        }
        else if (captured)
        {
            rt.localScale = Vector3.Lerp(rt.localScale, baseScale, Time.deltaTime * speed);
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, baseAnchoredPos, Time.deltaTime * speed);
            if ((rt.localScale - baseScale).sqrMagnitude < 0.00005f)
                Restore();
        }
    }

    private void Capture()
    {
        baseScale = rt.localScale;
        baseAnchoredPos = rt.anchoredPosition;
        baseSiblingIndex = rt.GetSiblingIndex();
        rt.SetAsLastSibling(); // draw above neighbouring cards in the fan
        captured = true;
    }

    private void Restore()
    {
        if (!captured) return;
        rt.localScale = baseScale;
        rt.anchoredPosition = baseAnchoredPos;
        if (rt.parent != null && baseSiblingIndex >= 0 && baseSiblingIndex < rt.parent.childCount)
            rt.SetSiblingIndex(baseSiblingIndex);
        captured = false;
    }

    public void OnPointerEnter(PointerEventData eventData) => hovered = true;
    public void OnPointerExit(PointerEventData eventData) => hovered = false;
}
