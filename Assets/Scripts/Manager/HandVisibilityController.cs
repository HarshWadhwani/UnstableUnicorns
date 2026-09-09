using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hotseat hand privacy: only the current active player's hand renders face-up. Every other
/// player's hand is face-down, except while it is being looked at for a specific reason:
///  - the player is the current Neigh responder (NeighManager.AwaitingResponse),
///  - the player's hand is the target of a look-at-hand effect (Hoof Job / Entitled Unicorn /
///    Officer Hornie — PendingActionType.TakeFromHand),
///  - Peeping Narwhal has revealed it for the caster's next turn.
///
/// A prompt that reassigns turnManager.activePlayer to a non-active player (e.g. "opponent
/// discards 1" — DiscardCardAction, GiveCardAction) reveals that player's hand for free via the
/// active-player rule, which is correct: they need to see their hand to choose.
///
/// Enforced every LateUpdate rather than through move hooks — cards change hands constantly
/// (draw, give, pull, take, Neigh) and a per-frame sweep over two small hands is trivial and
/// can't be bypassed. No gameplay logic reads a card's face; this is display only.
///
/// Created at runtime by CardActionExecutor (AddComponent), like NeighManager / BoardChrome,
/// so it needs no scene wiring.
/// </summary>
public class HandVisibilityController : MonoBehaviour
{
    public static HandVisibilityController Instance { get; private set; }

    private TurnManager turnManager;

    // Peeping Narwhal. Casting a Unicorn ends the caster's turn immediately, so revealing the
    // target hand "this turn" would be too late to act on — it's deferred to the caster's next
    // turn (the first turn they're active again after casting), then cleared.
    private Player peepCaster;
    private Player peepTarget;
    // 0 = cast this turn (caster still active); 1 = an intervening turn is running;
    // 2 = caster's reveal turn (target hand shown); then cleared back to 0.
    private int peepPhase;

    private readonly HashSet<Player> visible = new HashSet<Player>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;

        CardActionExecutor executor = CardActionExecutor.Instance;
        if (executor != null) turnManager = executor.turnManager;
    }

    /// <summary>Peeping Narwhal: show <paramref name="target"/>'s hand to <paramref name="caster"/>
    /// on the caster's next turn.</summary>
    public void RevealOnCasterNextTurn(Player caster, Player target)
    {
        peepCaster = caster;
        peepTarget = target;
        peepPhase = 0;
    }

    void LateUpdate()
    {
        if (turnManager == null || turnManager.activePlayer == null) return;

        AdvancePeepState();

        visible.Clear();
        visible.Add(turnManager.activePlayer);

        if (NeighManager.Instance != null && NeighManager.Instance.AwaitingResponse
            && NeighManager.Instance.Responder != null)
        {
            visible.Add(NeighManager.Instance.Responder);
        }

        CardActionExecutor exec = CardActionExecutor.Instance;
        if (exec != null
            && exec.currentPendingAction == PendingActionType.TakeFromHand
            && exec.pendingTakeFromHandTargetPlayer != null)
        {
            visible.Add(exec.pendingTakeFromHandTargetPlayer);
        }

        if (peepCaster != null && peepPhase == 2 && peepTarget != null)
        {
            visible.Add(peepTarget);
        }

        if (turnManager.players == null) return;
        foreach (Player player in turnManager.players)
        {
            if (player == null || player.handStable == null) continue;
            bool faceUp = visible.Contains(player);
            foreach (Card card in player.handStable.spaceCards)
                SetFace(card, faceUp);
        }
    }

    private void AdvancePeepState()
    {
        if (peepCaster == null) return;
        bool casterActive = turnManager.activePlayer == peepCaster;
        switch (peepPhase)
        {
            case 0: if (!casterActive) peepPhase = 1; break;
            case 1: if (casterActive) peepPhase = 2; break;
            case 2:
                if (!casterActive) { peepCaster = null; peepTarget = null; peepPhase = 0; }
                break;
        }
    }

    private static void SetFace(Card card, bool faceUp)
    {
        if (card == null || card.cardFront == null) return;
        if (card.cardFront.activeSelf == faceUp) return;
        if (faceUp) card.RevealCard();
        else card.HideCard();
    }
}
