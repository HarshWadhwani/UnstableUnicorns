using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the board's furniture at runtime — a defined play surface, empty-stable slot
/// wells, zone labels, one unified heads-up panel (turn, win progress, phase, Skip/Pass),
/// an active-player glow, and a game-over overlay — so no GameObjects have to be added to
/// the scene by hand. Created by <see cref="CardActionExecutor"/> via AddComponent;
/// discovers everything else itself. Cosmetic only.
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

    private GameObject gameOverRoot;
    private TMP_Text gameOverText;
    private bool gameOverShown;

    // The built-in rounded UISprite isn't reachable via Resources.GetBuiltinResource in this
    // Unity/uGUI version, so borrow it from an existing UI Image in the scene (the Skip button
    // and the Card prefab both use it). Falls back to null → plain sharp rectangles.
    private Sprite roundedSprite;
    private bool roundedResolved;
    private Sprite Rounded
    {
        get
        {
            if (roundedResolved) return roundedSprite;
            roundedResolved = true;

            Image[] images = Object.FindObjectsByType<Image>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Image img in images)
                if (img != null && img.sprite != null && img.sprite.border.sqrMagnitude > 0f)
                { roundedSprite = img.sprite; break; }
            if (roundedSprite == null)
                foreach (Image img in images)
                    if (img != null && img.sprite != null) { roundedSprite = img.sprite; break; }

            return roundedSprite;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    void Start()
    {
        canvas = GameObject.Find("CardCanvas")?.GetComponent<Canvas>()
                 ?? Object.FindAnyObjectByType<Canvas>();
        turnManager = Object.FindAnyObjectByType<TurnManager>();

        if (canvas == null || turnManager == null)
        {
            Debug.LogWarning("[BoardChrome] No Canvas / TurnManager found — chrome not built.");
            enabled = false;
            return;
        }
        canvasRt = canvas.transform as RectTransform;

        BuildBoardSurface();
        BuildSlotWells();
        BuildZoneLabels();
        BuildPlayerGlows();
        BuildHud();
        BuildGameOver();
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
                if (winFills[i] != null)
                    winFills[i].rectTransform.anchorMax = new Vector2(Mathf.Clamp01((float)have / need), 1f);
                if (winLabels[i] != null) winLabels[i].text = $"{p.name}   {have}/{need}";

                if (!gameOverShown && have >= need)
                {
                    gameOverShown = true;
                    if (gameOverText != null) gameOverText.text = $"{p.name} wins!";
                    if (gameOverRoot != null) gameOverRoot.SetActive(true);
                }
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

    // Dashed-looking placeholders where the 7 winning unicorns will sit — the win track,
    // visible from turn one. Cards drop on top and cover them.
    private void BuildSlotWells()
    {
        Color well = UiPalette.Recess; well.a = 0.6f;
        foreach (Player p in turnManager.players)
        {
            UnicornStable us = p != null ? p.unicornStable : null;
            if (us == null) continue;
            RectTransform srt = us.GetComponent<RectTransform>();
            if (srt == null) continue;

            int slots = Mathf.Max(1, us.maxCardsInStable);
            float w = srt.rect.width;
            float slotW = w / slots;
            float startX = srt.anchoredPosition.x - w / 2f + slotW / 2f;
            float cellW = Mathf.Min(slotW * 0.8f, 96f);
            float cellH = cellW * 1.4f;

            for (int i = 0; i < slots; i++)
            {
                Image cell = NewImage($"SlotWell{i}", srt, well);
                cell.rectTransform.anchorMin = cell.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                cell.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                cell.rectTransform.sizeDelta = new Vector2(cellW, cellH);
                cell.rectTransform.anchoredPosition = new Vector2(startX + i * slotW, 0f);
                cell.transform.SetAsFirstSibling();
            }
        }
    }

    private void BuildZoneLabels()
    {
        LabelZone(DeckManager.Instance != null ? DeckManager.Instance.playDeck : null, "Deck");
        LabelZone(DeckManager.Instance != null ? DeckManager.Instance.nursery : null, "Nursery");
        LabelZone(Object.FindAnyObjectByType<DiscardPile>(), "Discard");
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
        panel.rectTransform.sizeDelta = new Vector2(244, 176);
        panel.rectTransform.anchoredPosition = new Vector2(-16f, -16f);

        turnLabel = NewLabel("HudTurn", panel.rectTransform, "Turn 1",
                             17, UiPalette.Ink, FontStyles.Bold, TextAlignmentOptions.Left);
        DockRow(turnLabel.rectTransform, -9f, 24f);

        Color[] hues = { UiPalette.TypeBasic, UiPalette.TypeDowngrade };
        for (int i = 0; i < 2; i++)
        {
            Image track = NewImage($"WinTrack{i}", panel.rectTransform, UiPalette.CreamSunken);
            DockRow(track.rectTransform, -38f - i * 24f, 13f);

            // Width driven by anchorMax.x in Update (0..1) — works with or without a sprite.
            Image fill = NewImage($"WinFill{i}", track.rectTransform, hues[i]);
            fill.rectTransform.anchorMin = new Vector2(0f, 0f);
            fill.rectTransform.anchorMax = new Vector2(0f, 1f);
            fill.rectTransform.pivot = new Vector2(0f, 0.5f);
            fill.rectTransform.offsetMin = Vector2.zero;
            fill.rectTransform.offsetMax = Vector2.zero;
            winFills[i] = fill;

            TMP_Text lbl = NewLabel($"WinLabel{i}", track.rectTransform, "",
                                    9, UiPalette.Ink, FontStyles.Bold, TextAlignmentOptions.Right);
            lbl.rectTransform.anchorMin = Vector2.zero;
            lbl.rectTransform.anchorMax = Vector2.one;
            lbl.rectTransform.offsetMin = new Vector2(6f, 0f);
            lbl.rectTransform.offsetMax = new Vector2(-5f, 0f);
            winLabels[i] = lbl;
        }

        // Fold the scene's phase text + Skip/Pass button into the same panel.
        PhaseIndicator pi = Object.FindAnyObjectByType<PhaseIndicator>();
        if (pi != null && pi.phaseLabel != null)
        {
            pi.phaseLabel.rectTransform.SetParent(panel.rectTransform, false);
            DockRow(pi.phaseLabel.rectTransform, -96f, 22f);
            pi.phaseLabel.fontSize = 15f;
            pi.phaseLabel.color = UiPalette.InkSoft;
            pi.phaseLabel.alignment = TextAlignmentOptions.Left;
            pi.phaseLabel.enableAutoSizing = false;
        }
        if (pi != null && pi.skipButton != null)
        {
            RectTransform brt = pi.skipButton.GetComponent<RectTransform>();
            brt.SetParent(panel.rectTransform, false);
            DockRow(brt, -122f, 34f);

            Image bimg = pi.skipButton.GetComponent<Image>();
            if (bimg != null) bimg.color = UiPalette.Accent; // keep the button's own rounded sprite
            TMP_Text btxt = pi.skipButton.GetComponentInChildren<TMP_Text>();
            if (btxt != null)
            {
                btxt.color = UiPalette.AccentInk;
                btxt.fontSize = 15f;
                btxt.fontStyle = FontStyles.Bold;
                btxt.enableAutoSizing = false;
            }
        }
    }

    private void BuildGameOver()
    {
        Color scrim = UiPalette.Ink; scrim.a = 0.6f;
        Image backdrop = NewImage("GameOver", canvasRt, scrim);
        Stretch(backdrop.rectTransform, 0, 0);
        backdrop.transform.SetAsLastSibling();
        backdrop.raycastTarget = true; // swallow clicks to the board underneath

        Image plaque = NewImage("GameOverCard", backdrop.rectTransform, UiPalette.Cream);
        plaque.rectTransform.anchorMin = plaque.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        plaque.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        plaque.rectTransform.sizeDelta = new Vector2(480f, 220f);

        gameOverText = NewLabel("GameOverText", plaque.rectTransform, string.Empty,
                                42, UiPalette.Ink, FontStyles.Bold, TextAlignmentOptions.Center);
        Stretch(gameOverText.rectTransform, 26, 26);

        gameOverRoot = backdrop.gameObject;
        gameOverRoot.SetActive(false);
    }

    // ---------------------------------------------------------------- helpers

    // Lay a row across the top of the HUD panel, `y` down from the top, `h` tall.
    private static void DockRow(RectTransform rt, float y, float h)
    {
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(-26f, h);
        rt.anchoredPosition = new Vector2(0f, y);
    }

    private Image NewImage(string goName, RectTransform parent, Color color)
    {
        var go = new GameObject(goName, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        Sprite spr = Rounded;
        if (spr != null)
        {
            img.sprite = spr;
            img.type = Image.Type.Sliced;
        }
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
