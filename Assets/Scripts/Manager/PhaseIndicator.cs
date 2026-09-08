using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseIndicator : MonoBehaviour
{
    public TurnManager turnManager;
    public TMP_Text phaseLabel;
    public Button skipButton;

    private TMP_Text skipButtonLabel;
    private string skipButtonDefaultText;

    void Awake()
    {
        if (skipButton != null)
        {
            // The Skip button doubles as the "Pass" control during a Neigh contest. Its scene
            // onClick still calls TurnManager.SkipEveryTurnPhase (a no-op outside EveryTurnSpecial);
            // this extra listener is the Pass side (a no-op unless a contest is awaiting a response).
            skipButton.onClick.AddListener(OnSkipOrPass);

            skipButtonLabel = skipButton.GetComponentInChildren<TMP_Text>();
            if (skipButtonLabel != null) skipButtonDefaultText = skipButtonLabel.text;
        }
    }

    void OnSkipOrPass()
    {
        if (NeighManager.Instance != null && NeighManager.Instance.AwaitingResponse)
        {
            NeighManager.Instance.Pass();
        }
    }

    void Update()
    {
        if (turnManager == null || phaseLabel == null) return;

        bool awaitingNeigh = NeighManager.Instance != null && NeighManager.Instance.AwaitingResponse;

        if (awaitingNeigh)
        {
            phaseLabel.text =
                $"{NeighManager.Instance.Responder.name}: Neigh {NeighManager.Instance.ContestedCardName}? (Skip = pass)";
        }
        else
        {
            phaseLabel.text = turnManager.currentPhase switch
            {
                TurnPhase.Draw             => $"{turnManager.activePlayer.name}: Draw",
                TurnPhase.Action           => $"{turnManager.activePlayer.name}: Action",
                TurnPhase.ImmediateSpecial => $"{turnManager.activePlayer.name}: Resolving effect",
                TurnPhase.EveryTurnSpecial => turnManager.currentEveryTurnCard != null
                                                ? $"{turnManager.activePlayer.name}: Resolving {turnManager.currentEveryTurnCard.name}"
                                                : $"{turnManager.activePlayer.name}: Resolving effects",
                _                          => string.Empty
            };
        }

        if (skipButton != null)
        {
            skipButton.interactable = turnManager.CanSkipEveryTurnPhase || awaitingNeigh;
        }

        if (skipButtonLabel != null)
        {
            skipButtonLabel.text = awaitingNeigh ? "Pass" : skipButtonDefaultText;
        }
    }
}
