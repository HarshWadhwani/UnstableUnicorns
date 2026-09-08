using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Paints a card's face from <see cref="UiPalette"/> based on its <see cref="CardData"/>,
/// and adds the storybook trimmings that don't depend on the data (a hard offset shadow,
/// a mark on the card back). Lives on the Card prefab; <see cref="Card.Initialize"/> calls
/// <see cref="Apply"/> once the card data is known. Purely cosmetic — no game state.
/// </summary>
public class CardVisuals : MonoBehaviour
{
    [Tooltip("Top colour band — set to the card's type hue.")]
    public Image ribbon;

    [Tooltip("Framed art window — tinted toward the type hue.")]
    public Image artWindow;

    [Tooltip("Small corner chip — a second, redundant type signal for colour-blind readers.")]
    public Image typeChip;

    [Tooltip("Optional: written card type, e.g. \"Magical Unicorn\".")]
    public TMP_Text badgeLabel;

    [Tooltip("Optional: trigger timing in the card foot, e.g. \"Every turn\".")]
    public TMP_Text triggerLabel;

    void Awake()
    {
        AddSoftShadow(transform.Find("CardFront") as RectTransform);
        AddSoftShadow(transform.Find("CardBack") as RectTransform);
        AddBackMark(transform.Find("CardBack") as RectTransform);
    }

    public void Apply(CardData data)
    {
        Color hue = UiPalette.ForCard(data);

        if (ribbon != null) ribbon.color = hue;
        if (typeChip != null) typeChip.color = hue;
        if (artWindow != null) artWindow.color = Color.Lerp(UiPalette.CreamSunken, hue, 0.28f);
        if (badgeLabel != null) badgeLabel.text = UiPalette.TypeLabel(data);
        if (triggerLabel != null) triggerLabel.text = UiPalette.TriggerLabel(data);
    }

    private static void AddSoftShadow(RectTransform target)
    {
        if (target == null) return;
        Graphic g = target.GetComponent<Graphic>();
        if (g == null || g.GetComponent<Shadow>() != null) return;

        Shadow sh = g.gameObject.AddComponent<Shadow>();
        sh.effectColor = new Color(0.13f, 0.10f, 0.13f, 0.16f); // plum-brown, faint
        sh.effectDistance = new Vector2(4f, -5f);               // down-right, no blur — a paper cut-out
        sh.useGraphicAlpha = true;
    }

    // A faint diamond on the card back — a rotated square Image, so it needs no font glyph.
    private static void AddBackMark(RectTransform back)
    {
        if (back == null || back.Find("BackMark") != null) return;

        var go = new GameObject("BackMark", typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(back, false);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(34f, 34f);
        rt.localEulerAngles = new Vector3(0f, 0f, 45f);

        var img = go.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.14f);
        img.raycastTarget = false;
    }
}
