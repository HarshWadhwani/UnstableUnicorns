using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Paints a card's face from <see cref="UiPalette"/> based on its <see cref="CardData"/>.
/// Lives on the Card prefab; <see cref="Card.Initialize"/> calls <see cref="Apply"/> once
/// the card data is known. Purely cosmetic — no game state.
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

    public void Apply(CardData data)
    {
        Color hue = UiPalette.ForCard(data);

        if (ribbon != null) ribbon.color = hue;
        if (typeChip != null) typeChip.color = hue;
        if (artWindow != null) artWindow.color = Color.Lerp(UiPalette.CreamSunken, hue, 0.28f);
        if (badgeLabel != null) badgeLabel.text = UiPalette.TypeLabel(data);
        if (triggerLabel != null) triggerLabel.text = UiPalette.TriggerLabel(data);
    }
}
