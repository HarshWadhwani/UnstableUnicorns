using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseIndicator : MonoBehaviour
{
    public TurnManager turnManager;
    public TMP_Text phaseLabel;
    public Button skipButton;

    void Update()
    {
        if (turnManager == null || phaseLabel == null) return;

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

        if (skipButton != null)
        {
            skipButton.interactable = turnManager.CanSkipEveryTurnPhase;
        }
    }
}
