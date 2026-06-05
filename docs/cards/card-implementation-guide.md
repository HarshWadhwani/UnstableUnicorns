# Card Implementation Guide

How to go from a card description to working code. Follow these decisions in order.

---

## Step 1 — Determine CardType

| If the card… | CardType |
|---|---|
| Counts toward the win condition (unicorn) | `UNICORN` |
| Has a one-time effect when played, then goes away | `MAGIC` |
| Gives the owner a persistent effect each turn | `UPGRADE` |
| Gives the opponent a persistent penalty each turn | `DOWNGRADE` |
| Cancels another card being played | `NEIGH` |

`cardType` determines where the card routes when played — do not set it manually on the asset. The subclass `OnEnable()` sets it automatically.

---

## Step 2 — Determine SpecialActionType

This is also set automatically by the subclass. Shown here for understanding:

| CardType | SpecialActionType | Meaning |
|---|---|---|
| `MAGIC` | `IMMEDIATE` | Effect fires the moment the card is played |
| `UPGRADE` | `EVERY_TURN` | Effect fires each turn while in play (not yet wired) |
| `DOWNGRADE` | `EVERY_TURN` | Same as UPGRADE |
| `UNICORN` | `NONE` | No automatic trigger |
| `NEIGH` | `NONE` | No automatic trigger |

---

## Step 3 — Map the effect to CardActions

Effects are built from three action types, run in the order they appear in the `actions` list. Each action pauses execution and waits for a player click before the next action runs.

### DiscardCardAction
Forces a player to send cards from their hand to the discard pile.

```
targetPlayer:   ActivePlayer | Opponent       — who must discard
selectionMode:  PlayerChooses | Random        — free choice or random
numberOfCards:  int                           — how many
```

### GiveCardAction
A player picks cards from their hand and transfers them to the other player's hand.

```
giver:          ActivePlayer | Opponent       — who gives away cards
numberOfCards:  int                           — how many
```
The receiver is always the other player.

### DestroyCardAction
A player picks cards from any of the opposing player's stables (unicorn, upgrade, or downgrade) and sends them to the discard pile.

```
destroyer:      ActivePlayer | Opponent       — who picks what to destroy
numberOfCards:  int                           — how many
```
Source is resolved at click time — no stable needs to be specified.

### PullCardAction
Randomly takes cards from the **opponent's** hand and moves them to the **active player's** hand. Always random — no player prompt.

```
numberOfCards:           int    — how many to pull (capped at opponent's hand size)
skipDrawPhaseOnSuccess:  bool   — if true and ≥1 card was pulled, skips the active player's Draw phase
```
If the opponent has fewer cards than `numberOfCards`, all remaining cards are pulled. `skipDrawPhaseOnSuccess` defaults to `false` and is backward compatible — existing cards are unaffected.

### SacrificeCardAction
Moves cards from the **active player's own** stables to the discard pile. Contrast with Destroy, which always targets the opponent.

```
targetStable:   Unicorn | Upgrade | Downgrade | Any   — which of the active player's stables
sacrificeAll:   bool                                   — true = take all cards automatically; false = not yet implemented
```
When `sacrificeAll = true`, no player input is required — cards are moved immediately.

### Mapping examples

| Card text | Action |
|---|---|
| "Opponent discards 1 card" | `DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }` |
| "Discard a random card from your hand" | `DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=Random, numberOfCards=1 }` |
| "Give 2 cards from your hand to your opponent" | `GiveCardAction { giver=ActivePlayer, numberOfCards=2 }` |
| "Destroy 1 card in any opponent stable" | `DestroyCardAction { destroyer=ActivePlayer, numberOfCards=1 }` |
| "Pull 2 random cards from opponent's hand" | `PullCardAction { numberOfCards=2 }` |
| "Sacrifice all downgrade cards in your stable" | `SacrificeCardAction { targetStable=Downgrade, sacrificeAll=true }` |
| "Sacrifice all cards in your stable" | `SacrificeCardAction { targetStable=Any, sacrificeAll=true }` |

If a step has no matching action type (e.g., draw a card, steal from stable), a new `CardAction` subclass is needed.

---

## Step 4 — Choose or create a C# class

### Use an existing base class directly (no effect)
If the card has no actions — it just sits in a stable as a unicorn or passive card — no new C# file is needed. Create only the `.asset` in Unity using the existing `[CreateAssetMenu]` on the base class.

Existing base classes with `[CreateAssetMenu]`:
- `UpgradeCardData` — `CardData/UpgradeCardData`
- `DowngradeCardData` — `CardData/DowngradeCardData`

### Create a new subclass (has effect)
If the card has a non-empty `actions` list, create a new C# file:

```
Assets/Scripts/CardData/<CardType>CardData/<CardNameNoSpaces>CardData.cs
```

The class extends the appropriate intermediate class:

| CardType | Extend |
|---|---|
| `MAGIC` | `MagicCardData` |
| `UNICORN` | `UnicornCardData` |
| `UPGRADE` | `UpgradeCardData` |
| `DOWNGRADE` | `DowngradeCardData` |
| `NEIGH` | `NeighCardData` |

Template:

```csharp
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/<CardType>CardData/<CardNameNoSpaces>CardData")]
public class <CardNameNoSpaces>CardData : <CardType>CardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            // actions here
        };
    }
}
```

`base.OnEnable()` sets `cardType` and `specialActionType` — always call it.

---

## Step 5 — Create the Unity asset

After Unity compiles the script:

1. Right-click in the Project window → Create → CardData → `<CardType>CardData` → `<CardName>CardData`
2. Save it at: `Assets/Resources/CardDataInstances/<CardType>CardDataInstances/<Card Name> Card Data.asset`
3. Set on the asset:
   - `cardNameVariations` — at minimum one entry matching the card name
   - `cardDescriptionText` — rules text as it will appear in-game
   - `instances` — number of copies in the deck

---

## Worked example — Fuck Marry Kill

**Description:** Opponent discards 1 card. You give 1 card from your hand to your opponent. You destroy 1 card in any of your opponent's stables.

| Decision | Answer |
|---|---|
| CardType | `MAGIC` — one-time effect |
| SpecialActionType | `IMMEDIATE` — set by `MagicCardData` |
| Actions | Three, in sequence (see below) |
| New class needed? | Yes — unique combination of three actions |
| instances | 1 |

```csharp
actions = new List<CardAction>
{
    new DiscardCardAction { targetPlayer = DiscardCardAction.TargetPlayer.Opponent, selectionMode = DiscardCardAction.SelectionMode.PlayerChooses, numberOfCards = 1 },
    new GiveCardAction    { giver = GiveCardAction.TargetPlayer.ActivePlayer, numberOfCards = 1 },
    new DestroyCardAction { destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer, numberOfCards = 1 }
};
```

Script: `Assets/Scripts/CardData/MagicCardData/FuckMarryKillCardData.cs`  
Asset: `Assets/Resources/CardDataInstances/MagicCardDataInstances/Fuck Marry Kill Card Data.asset`

See `docs/cards/fuck-marry-kill.md` for the full execution trace.
