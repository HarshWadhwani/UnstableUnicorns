using System.Collections.Generic;
using UnityEngine;

// "Choose between two effects." Presents `chooser` with two labelled options; the picked
// option's action list is spliced onto the front of the executor's queue and runs.
//
// Viability: optionAViable / optionBViable are evaluated against the live context before
// prompting. If only one option can do anything, it runs with no prompt; if neither can, the
// whole action is a silent no-op. This is also what keeps a >1-count sub-action (e.g. Sex,
// Drugs, and Unicorns' "discard 3") from ever being offered into a soft-lock.
//
// Predicates and the nested action lists are set in the card's OnEnable and are runtime-only
// (not serialized) — same as every other CardAction, which is rebuilt each load.
[System.Serializable]
public class ChooseEffectAction : CardAction
{
    public enum Chooser { ActivePlayer, Opponent }

    public Chooser chooser = Chooser.ActivePlayer;

    public string optionALabel = "Option A";
    public List<CardAction> optionA = new List<CardAction>();
    public System.Predicate<CardActionContext> optionAViable;

    public string optionBLabel = "Option B";
    public List<CardAction> optionB = new List<CardAction>();
    public System.Predicate<CardActionContext> optionBViable;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player chooserPlayer = chooser == Chooser.ActivePlayer
            ? context.activePlayer
            : context.opponentPlayer;

        if (chooserPlayer == null)
        {
            Debug.Log("ChooseEffectAction: no chooser player. Skipping.");
            return;
        }

        bool aViable = optionAViable == null || optionAViable(context);
        bool bViable = optionBViable == null || optionBViable(context);

        if (!aViable && !bViable)
        {
            Debug.Log("ChooseEffectAction: neither option is viable. Skipping.");
            return;
        }

        if (aViable && !bViable)
        {
            Debug.Log($"ChooseEffectAction: only [{optionALabel}] is viable — running it directly.");
            executor.PrependActions(optionA);
            return;
        }

        if (bViable && !aViable)
        {
            Debug.Log($"ChooseEffectAction: only [{optionBLabel}] is viable — running it directly.");
            executor.PrependActions(optionB);
            return;
        }

        string title = context.sourceCard != null ? context.sourceCard.name : "Choose an effect";
        executor.PromptEffectChoice(chooserPlayer, title, optionALabel, optionA, optionBLabel, optionB);
    }
}
