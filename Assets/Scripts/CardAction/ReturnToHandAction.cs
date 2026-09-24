using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// "Return a card in a Stable to its owner's hand." The active player clicks a card in an eligible
// player's stable; it goes to the hand of whoever owns that stable (not the chooser's). Skips
// silently if no eligible card exists. A Baby Unicorn goes to the Nursery instead
// (CardManager.MoveCard). Used by Sextra-Terrestrial Unicorn (OtherPlayers/Any) and Unicorn
// Flatulence (Self/Unicorn).
[System.Serializable]
public class ReturnToHandAction : CardAction
{
    public enum Whose { Self, OtherPlayers }
    public enum TargetStable { Any, Unicorn }

    public Whose whose = Whose.OtherPlayers;
    public TargetStable targetStable = TargetStable.Any;

    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        Player chooser = context.activePlayer;
        List<Player> owners = whose == Whose.Self
            ? new List<Player> { chooser }
            : context.turnManager.players.Where(p => p != chooser).ToList();

        bool unicornOnly = targetStable == TargetStable.Unicorn;
        bool anyTarget = owners.Any(p => p.unicornStable.spaceCards.Count > 0
            || (!unicornOnly && (p.upgradeStable.spaceCards.Count > 0 || p.downgradeStable.spaceCards.Count > 0)));

        if (!anyTarget)
        {
            Debug.Log("ReturnToHandAction: no eligible card to return. Skipping.");
            return;
        }

        executor.pendingReturnEligibleOwners = owners;
        executor.pendingReturnUnicornOnly = unicornOnly;
        executor.PromptPlayerToSelectCards(chooser, null, null, 1, PendingActionType.ReturnToHand);
    }
}
