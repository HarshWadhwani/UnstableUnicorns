using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The two-option chooser UI for <see cref="ChooseEffectAction"/>. Runtime-built, no scene
/// wiring — created by <see cref="CardActionExecutor"/> via AddComponent (like BoardChrome /
/// NeighManager / HandVisibilityController). Shows a centred plaque with a title and two
/// buttons whenever the executor has a <c>ChooseEffect</c> pending; each button calls
/// <see cref="CardActionExecutor.ResolveEffectChoice"/>. Cosmetic shell — the executor owns
/// the state.
/// </summary>
public class EffectChoicePanel : MonoBehaviour
{
    public static EffectChoicePanel Instance { get; private set; }

    private GameObject root;
    private TMP_Text titleText;
    private TMP_Text buttonALabel;
    private TMP_Text buttonBLabel;
    private bool built;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    void Update()
    {
        CardActionExecutor exec = CardActionExecutor.Instance;
        bool show = exec != null && exec.currentPendingAction == PendingActionType.ChooseEffect;

        if (show && !built) Build();
        if (!built) return;

        if (show)
        {
            titleText.text = exec.pendingChoiceTitle ?? string.Empty;
            buttonALabel.text = exec.pendingChoiceLabelA ?? "Option A";
            buttonBLabel.text = exec.pendingChoiceLabelB ?? "Option B";
        }
        if (root.activeSelf != show) root.SetActive(show);
    }

    private void Build()
    {
        Canvas canvas = GameObject.Find("CardCanvas")?.GetComponent<Canvas>()
                        ?? Object.FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("[EffectChoicePanel] No Canvas found — panel not built.");
            enabled = false;
            return;
        }
        var canvasRt = canvas.transform as RectTransform;

        Image scrim = NewImage("EffectChoice", canvasRt, WithAlpha(UiPalette.Ink, 0.55f));
        Stretch(scrim.rectTransform);
        scrim.raycastTarget = true;                // swallow clicks to the board underneath
        scrim.transform.SetAsLastSibling();
        root = scrim.gameObject;

        Image plaque = NewImage("Panel", scrim.rectTransform, UiPalette.Cream);
        plaque.rectTransform.anchorMin = plaque.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        plaque.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        plaque.rectTransform.sizeDelta = new Vector2(540f, 250f);

        titleText = NewLabel("Title", plaque.rectTransform, string.Empty, 28, UiPalette.Ink, FontStyles.Bold);
        var tr = titleText.rectTransform;
        tr.anchorMin = new Vector2(0f, 1f);
        tr.anchorMax = new Vector2(1f, 1f);
        tr.pivot = new Vector2(0.5f, 1f);
        tr.sizeDelta = new Vector2(-48f, 76f);
        tr.anchoredPosition = new Vector2(0f, -22f);

        buttonALabel = MakeButton("BtnA", plaque.rectTransform, new Vector2(0f, -128f),
                                  () => CardActionExecutor.Instance?.ResolveEffectChoice(0));
        buttonBLabel = MakeButton("BtnB", plaque.rectTransform, new Vector2(0f, -190f),
                                  () => CardActionExecutor.Instance?.ResolveEffectChoice(1));

        root.SetActive(false);
        built = true;
    }

    private static TMP_Text MakeButton(string name, RectTransform parent, Vector2 anchoredPos,
                                       UnityEngine.Events.UnityAction onClick)
    {
        Image img = NewImage(name, parent, UiPalette.Accent);
        img.raycastTarget = true;
        var rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(460f, 52f);
        rt.anchoredPosition = anchoredPos;

        var btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(onClick);

        TMP_Text label = NewLabel(name + "Label", rt, string.Empty, 20, UiPalette.Cream, FontStyles.Bold);
        Stretch(label.rectTransform);
        return label;
    }

    // ---- self-contained UI helpers (BoardChrome's equivalents are private) ----

    private static Image NewImage(string name, RectTransform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;
        return img;
    }

    private static TMP_Text NewLabel(string name, RectTransform parent, string text, float size,
                                     Color color, FontStyles style)
    {
        var go = new GameObject(name, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.fontStyle = style;
        t.alignment = TextAlignmentOptions.Center;
        t.raycastTarget = false;
        t.overflowMode = TextOverflowModes.Overflow;
        return t;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static Color WithAlpha(Color c, float a) { c.a = a; return c; }
}
