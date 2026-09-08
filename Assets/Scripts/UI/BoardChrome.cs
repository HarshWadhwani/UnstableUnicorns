using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the board's furniture at runtime — a defined play surface, uppercase zone
/// labels, a heads-up panel (turn + win progress), and an active-player glow — so no
/// GameObjects have to be added to the scene by hand. Created by
/// <see cref="CardActionExecutor"/> via AddComponent; discovers everything else itself.
/// Cosmetic only.
/// </summary>
public class BoardChrome : MonoBehaviour
{
    public static BoardChrome Instance { get; private set; }

    private Canvas canvas;
    private RectTransform canvasRt;
    private TurnManager turnManager;

    private readonly RectTransform[] glows = new RectTransform[2];
    private readonly Image[] winFills = new Image[2];
    private readonly TMP_Text[] winLabels = new TMP_Text[2];
    private TMP_Text turnLabel;

    private Sprite roundedSprite;
    private Sprite Rounded => roundedSprite != null
        ? roundedSprite
        : (roundedSprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd"));

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    void Start()
    {
        canvas = GameObject.Find("CardCanvas")?.GetComponent<Canvas>()
                 ?? Object.FindFirstObjectByType<Canvas>();
        turnManager = Object.FindFirstObjectByType<TurnManager>();

        if (canvas == null || turnManager == null)
        {
            Debug.LogWarning("[BoardChrome] No Canvas / TurnManager found — chrome not built.");
            enabled = false;
            return;
        }
        canvasRt = canvas.transform as RectTransform;

        BuildBoardSurface();
        BuildZoneLabels();
        BuildPlayerGlows();
        BuildHud();
    }

    void Update()
    {
        if (turnManager == null || turnManager.players == null) return;

        for (int i = 0; i < turnManager.players.Count && i < 2; i++)
        {
            Player p = turnManager.players[i];
            if (p == null) continue;

            if (glows[i] != null)
                glows[i].gameObject.SetActive(p == turnManager.activePlayer);

            UnicornStable us = p.unicornStable;
            if (us != null)
            {
                int have = us.spaceCards.Count;
                int need = Mathf.Max(1, us.winConditionCount);
                if (winFills[i] != null) winFills[i].fillAmount = Mathf.Clamp01((float)have / need);
                if (winLabels[i] != null) winLabels[i].text = $"{p.name}   {have}/{need}";
            }
        }

        if (turnLabel != null)
            turnLabel.text = turnManager.activePlayer != null
                ? $"{turnManager.activePlayer.name}  ·  Turn {turnManager.turnNumber}"
                : $"Turn {turnManager.turnNumber}";
    }

    // ---------------------------------------------------------------- builders

    private void BuildBoardSurface()
    {
        GameObject world = GameObject.Find("GameBoard");
        if (world != null) world.SetActive(false); // retire the world-space sprite; it can't scale with the canvas

        Image mat = NewImage("BoardSurface", canvasRt, UiPalette.Recess);
        Stretch(mat.rectTransform, 24, 20);
        mat.transform.SetAsFirstSibling();

        Image inner = NewImage("BoardField", mat.rectTransform, UiPalette.Table);
        Stretch(inner.rectTransform, 6, 6);
    }

    private void BuildZoneLabels()
    {
        LabelZone(DeckManager.Instance != null ? DeckManager.Instance.playDeck : null, "Deck");
        LabelZone(DeckManager.Instance != null ? DeckManager.Instance.nursery : null, "Nursery");
        LabelZone(Object.FindFirstObjectByType<DiscardPile>(), "Discard");
    }

    private void LabelZone(Component zone, string text)
    {
        if (zone == null) return;
        RectTransform zrt = zone.GetComponent<RectTransform>();
        if (zrt == null) return;

        TMP_Text lbl = NewLabel("ZoneLabel_" + text, canvasRt, text.ToUpperInvariant(),
                                14, UiPalette.TableInkSoft, FontStyles.Bold, TextAlignmentOptions.Center);
        lbl.characterSpacing = 8f;
        lbl.rectTransform.anchorMin = lbl.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        lbl.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        lbl.rectTransform.sizeDelta = new Vector2(170, 20);
        lbl.rectTransform.anchoredPosition = zrt.anchoredPosition + new Vector2(0f, 62f);
    }

    private void BuildPlayerGlows()
    {
        Color wash = UiPalette.Accent; wash.a = 0.13f;
        for (int i = 0; i < turnManager.players.Count && i < 2; i++)
        {
            RectTransform prt = turnManager.players[i] != null
                ? turnManager.players[i].GetComponent<RectTransform>() : null;
            if (prt == null) continue;

            Image glow = NewImage("TurnGlow", prt, wash);
            Stretch(glow.rectTransform, -16, -16); // bleed slightly past the player's area
            glow.transform.SetAsFirstSibling();
            glows[i] = glow.rectTransform;
            glow.gameObject.SetActive(false);
        }
    }

    private void BuildHud()
    {
        Image panel = NewImage("HudPanel", canvasRt, UiPalette.Cream);
        panel.rectTransform.anchorMin = panel.rectTransform.anchorMax = new Vector2(1f, 1f);
        panel.rectTransform.pivot = new Vector2(1f, 1f);
        panel.rectTransform.sizeDelta = new Vector2(236, 98);
        panel.rectTransform.anchoredPosition = new Vector2(-16f, -16f);

        turnLabel = NewLabel("HudTurn", panel.rectTransform, "Turn 1",
                             17, UiPalette.Ink, FontStyles.Bold, TextAlignmentOptions.Left);
        turnLabel.rectTransform.anchorMin = new Vector2(0f, 1f);
        turnLabel.rectTransform.anchorMax = new Vector2(1f, 1f);
        turnLabel.rectTransform.pivot = new Vector2(0.5f, 1f);
        turnLabel.rectTransform.sizeDelta = new Vector2(-24f, 24f);
        turnLabel.rectTransform.anchoredPosition = new Vector2(0f, -9f);

        Color[] hues = { UiPalette.TypeBasic, UiPalette.TypeDowngrade };
        for (int i = 0; i < 2; i++)
        {
            float y = -38f - i * 26f;

            Image track = NewImage($"WinTrack{i}", panel.rectTransform, UiPalette.CreamSunken);
            track.rectTransform.anchorMin = new Vector2(0f, 1f);
            track.rectTransform.anchorMax = new Vector2(1f, 1f);
            track.rectTransform.pivot = new Vector2(0.5f, 1f);
            track.rectTransform.sizeDelta = new Vector2(-28f, 13f);
            track.rectTransform.anchoredPosition = new Vector2(0f, y);

            Image fill = NewImage($"WinFill{i}", track.rectTransform, hues[i]);
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = 0; // left
            fill.fillAmount = 0f;
            Stretch(fill.rectTransform, 0, 0);

            TMP_Text lbl = NewLabel($"WinLabel{i}", track.rectTransform, "",
                                    9, UiPalette.Ink, FontStyles.Bold, TextAlignmentOptions.Right);
            lbl.rectTransform.anchorMin = Vector2.zero;
            lbl.rectTransform.anchorMax = Vector2.one;
            lbl.rectTransform.offsetMin = new Vector2(6f, 0f);
            lbl.rectTransform.offsetMax = new Vector2(-5f, 0f);
            winLabels[i] = lbl;
        }
    }

    // ---------------------------------------------------------------- helpers

    private Image NewImage(string goName, RectTransform parent, Color color)
    {
        var go = new GameObject(goName, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        img.sprite = Rounded;
        img.type = Image.Type.Sliced;
        img.raycastTarget = false;
        return img;
    }

    private TMP_Text NewLabel(string goName, RectTransform parent, string text, float size,
                              Color color, FontStyles style, TextAlignmentOptions align)
    {
        var go = new GameObject(goName, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.fontStyle = style;
        t.alignment = align;
        t.raycastTarget = false;
        t.overflowMode = TextOverflowModes.Overflow;
        return t;
    }

    private static void Stretch(RectTransform rt, float xInset, float yInset)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(xInset, yInset);
        rt.offsetMax = new Vector2(-xInset, -yInset);
    }
}
