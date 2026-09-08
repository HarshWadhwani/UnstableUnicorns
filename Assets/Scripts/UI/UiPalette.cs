using UnityEngine;

/// <summary>
/// "Storybook Stable" palette — the single source of truth for gameplay UI colour.
/// Hex values mirror the Storybook Stable mockup; retune the game's look from here.
/// </summary>
public static class UiPalette
{
    private static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    // --- grounds & panels ---
    public static readonly Color Table       = Hex("#E8E3F1"); // the board surface
    public static readonly Color Recess      = Hex("#DBD3EA"); // empty slots, gutters
    public static readonly Color Cream       = Hex("#FCF8F1"); // card / panel face
    public static readonly Color CreamSunken = Hex("#F4EDDF"); // art windows, wells
    public static readonly Color PanelEdge   = Hex("#E7DFCB"); // hairline on cream

    // --- ink ---
    public static readonly Color Ink          = Hex("#3C2F3B"); // text on cream
    public static readonly Color InkSoft      = Hex("#857587");
    public static readonly Color TableInk     = Hex("#4B3E49"); // text on the board
    public static readonly Color TableInkSoft = Hex("#7C6E7B");

    // --- accent: whose turn it is / what you can click ---
    public static readonly Color Accent    = Hex("#23AEAA");
    public static readonly Color AccentInk = Hex("#0C6663");

    public static readonly Color CardBack = Hex("#4C3E68");

    // --- one hue per card type ---
    public static readonly Color TypeBasic     = Hex("#F4A4C0"); // Basic / Baby Unicorn
    public static readonly Color TypeMagical   = Hex("#A6B4EE"); // Magical Unicorn
    public static readonly Color TypeMagic     = Hex("#F1C74F"); // Magic
    public static readonly Color TypeUpgrade   = Hex("#8FD2A8"); // Upgrade
    public static readonly Color TypeDowngrade = Hex("#EE9877"); // Downgrade
    public static readonly Color TypeNeigh     = Hex("#C4A1DD"); // Neigh

    /// <summary>The ribbon/badge hue for a card.</summary>
    public static Color ForCard(CardData data)
    {
        if (data == null) return TypeBasic;
        switch (data.cardType)
        {
            case CardType.MAGIC:     return TypeMagic;
            case CardType.UPGRADE:   return TypeUpgrade;
            case CardType.DOWNGRADE: return TypeDowngrade;
            case CardType.NEIGH:     return TypeNeigh;
            case CardType.UNICORN:
                if (data is UnicornCardData u
                    && (u.unicornType == UnicornType.MAGIC || u.unicornType == UnicornType.SPECIAL))
                    return TypeMagical;
                return TypeBasic; // BASIC + BABY
            default: return TypeBasic;
        }
    }

    /// <summary>Human-readable type for the card's badge, e.g. "Magical Unicorn".</summary>
    public static string TypeLabel(CardData data)
    {
        if (data == null) return string.Empty;
        switch (data.cardType)
        {
            case CardType.MAGIC:     return "Magic";
            case CardType.UPGRADE:   return "Upgrade";
            case CardType.DOWNGRADE: return "Downgrade";
            case CardType.NEIGH:     return "Neigh";
            case CardType.UNICORN:
                if (data is UnicornCardData u)
                {
                    if (u.unicornType == UnicornType.BABY) return "Baby Unicorn";
                    if (u.unicornType == UnicornType.MAGIC || u.unicornType == UnicornType.SPECIAL)
                        return "Magical Unicorn";
                }
                return "Basic Unicorn";
            default: return string.Empty;
        }
    }

    /// <summary>Trigger timing shown in the card foot, e.g. "Every turn".</summary>
    public static string TriggerLabel(CardData data)
    {
        if (data == null) return string.Empty;
        if (data.specialActionType == SpecialActionType.EVERY_TURN) return "Every turn";
        if (data.cardType == CardType.NEIGH) return "Instant";
        if (data.specialActionType == SpecialActionType.IMMEDIATE) return "On play";
        return string.Empty;
    }
}
