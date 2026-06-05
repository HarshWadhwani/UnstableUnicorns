# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Working Instructions

- When the user declares an issue complete, update its **Status** line in `docs/issues.md` and mark it ✅ in the tracker table — do this in the same response.

## Project Overview

A Unity 6 (6000.2.6f2) digital implementation of the card game **Unstable Unicorns**. UI-based (Canvas/RectTransform), currently supporting 2 players.

## Development

This is a Unity project — there are no CLI build or test commands. All development happens in the Unity Editor (open the project and use Play mode to test). Scripts are in `Assets/Scripts/` and are compiled automatically by Unity when saved.

Card data assets (ScriptableObjects) live in `Assets/Resources/CardDataInstances/` and are loaded at runtime via `Resources.LoadAll<CardData>`.

## Architecture

### Core Data Model

**`CardData` (ScriptableObject)** is the base for all card types:
- Subclasses: `UnicornCardData`, `MagicCardData`, `UpgradeCardData`, `DowngradeCardData`, `NeighCardData`
- Each subclass sets `cardType` and `specialActionType` in `OnEnable()`
- Cards with effects define a `List<CardAction> actions` — these are executed sequentially by `CardActionExecutor`

**`Card` (MonoBehaviour)** is the runtime instance. It holds a reference to its `CardData` and its current `CardSpace`.

### CardSpace Hierarchy

`CardSpace` (abstract) → `Stable` → `HandStable`, `UnicornStable`, `UpgradeStable`, `DowngradeStable`  
`CardSpace` → `Deck`, `DiscardPile`

- `CardSpace.HandleCardClick(Card)` is the entry point for all player interactions
- `Stable.PositionCardsInStable()` handles card layout; `HandStable` overrides it with a fan layout
- `allowedTurnPhases` on each `CardSpace` gates which clicks are valid

### Manager Layer

- **`TurnManager`** — owns the player list, active player, and turn phase (`Draw → Action → Special → Draw`). Call `StartNextTurnPhase()` to advance.
- **`CardManager`** — handles all card movement (`MoveCard`, `DrawCard`, `PlayCardForCurrentPlayer`). Routes played cards to the correct destination based on `CardType`.
- **`DeckManager`** — loads all `CardData` from Resources on `Start()`, instantiates `Card` prefabs, shuffles, and deals baby unicorns.

### CardAction System

Card effects are composed from serializable `CardAction` subclasses defined on the `CardData`. Three action types currently exist:

| Action | Parameters | Effect |
|--------|------------|--------|
| `DiscardCardAction` | `targetPlayer` (ActivePlayer/Opponent), `selectionMode` (PlayerChooses/Random), `numberOfCards` | Forces target player to discard cards from hand to the discard pile. Skips silently if the target's hand is empty. |
| `GiveCardAction` | `giver` (ActivePlayer/Opponent), `numberOfCards` | Giver picks N cards from their hand and transfers them to the other player's hand. |
| `DestroyCardAction` | `destroyer` (ActivePlayer/Opponent), `targetStable` (Any/Unicorn), `numberOfCards` | Destroyer picks N cards from the opposing player's stables and sends them to the discard pile. `targetStable=Any` allows unicorn/upgrade/downgrade; `targetStable=Unicorn` restricts to unicorn cards only (uses `PendingActionType.DestroyUnicornCard`). |
| `PullCardAction` | `numberOfCards`, `skipDrawPhaseOnSuccess` (bool) | Randomly moves N cards from the **opponent's** hand to the **active player's** hand. Capped at opponent's hand size. If `skipDrawPhaseOnSuccess=true` and ≥1 card was pulled, sets `TurnManager.skipNextDrawPhase` to skip the Draw phase. |
| `SacrificeCardAction` | `targetStable` (Unicorn/Upgrade/Downgrade/Any), `sacrificeAll` (bool) | Moves cards from the **active player's own** stables to the discard pile. `sacrificeAll=true` auto-moves all; PlayerChooses not yet implemented. |

Each action's `Execute(executor, context)` resolves which players/spaces are involved and calls a `Prompt*` method on `CardActionExecutor` to pause the queue for player input.

**Execution flow:**
1. `CardData.TriggerSpecialAction(sourceCard)` → `CardActionExecutor.ExecuteActions()`
2. Actions run sequentially via a `Queue<CardAction>`
3. Actions requiring player input set a `PendingActionType` on `CardActionExecutor` and temporarily reassign `turnManager.activePlayer` to the prompted player — this is how click routing works during effects
4. The next click on the appropriate `CardSpace` calls `CardActionExecutor.ExecutePendingAction(card)`, which decrements `pendingCardsRemaining` and resumes the queue when done

`pendingSourceStable` locks the source space when an action targets a single stable (e.g., discard from hand). For `DestroyCard` it is `null` — the source is derived from `card.cardSpace` at click time, allowing the destroyer to pick from any opposing stable.

`pendingDestroyTargetPlayer` is set by `DestroyCardAction` before prompting and records exactly which player's stable cards may be selected from. `Stable.HandleCardClick` checks `player == pendingDestroyTargetPlayer` to accept or reject a click — this is more reliable than comparing against `turnManager.activePlayer`, which can be temporarily reassigned during multi-step action sequences.

Cards that cannot always be played (e.g., require a non-empty opponent stable) override `CardData.CanPlay(activePlayer, opponentPlayer)`. `CardManager.PlayCardForCurrentPlayer` calls this before triggering any action; returning `false` keeps the card in hand and cancels the play.

**Worked example — Fuck Marry Kill** (see `docs/cards/fuck-marry-kill.md` for the full trace):

```
actions[0] = DiscardCardAction { targetPlayer = Opponent,    selectionMode = PlayerChooses, numberOfCards = 1 }
actions[1] = GiveCardAction    { giver        = ActivePlayer,                                numberOfCards = 1 }
actions[2] = DestroyCardAction { destroyer    = ActivePlayer,                                numberOfCards = 1 }
```

The three actions queue up and pause for input between each step: opponent picks a hand card to discard → active player picks a hand card to give → active player picks any card in any of the opponent's stables to destroy.

### Adding a New Card

Use the `/add-card` skill: `/add-card "<card description>"` — Claude will walk through the decision tree, write the C# class if needed, and tell you what to create in Unity.

Full decision tree, action-type reference, and worked examples: `docs/cards/card-implementation-guide.md`

### Key Enums

- `CardType`: `UNICORN`, `MAGIC`, `UPGRADE`, `DOWNGRADE`, `NEIGH`
- `SpecialActionType`: `IMMEDIATE` (triggers on play), `EVERY_TURN`, `NONE`
- `TurnPhase`: `Draw`, `Action`, `Special`
- `PendingActionType`: `None`, `DiscardCard`, `GiveCard`, `DestroyCard`

---

## Gameplay Rules

### Turn Structure
`Draw → Action → Special → (next player's Draw)`
- **Draw:** Active player clicks the top card of the play deck.
- **Action:** Active player plays one card from hand (pass not yet implemented).
- **Special:** EVERY_TURN effects trigger. Not yet wired — phase exists but does nothing.

### Win Condition
Player wins when `UnicornStable` reaches `maxCardsInStable` unicorns. Currently logs only — no game-over state.

### Card Routing on Play
| CardType   | Destination                                         |
|------------|-----------------------------------------------------|
| UNICORN    | `activePlayer.unicornStable`                        |
| UPGRADE    | `activePlayer.upgradeStable`                        |
| DOWNGRADE  | First non-active player's `downgradeStable`         |
| MAGIC      | `discardPile` (effect fires before card is moved)   |
| NEIGH      | `discardPile`                                       |

---

## Known Gaps & Next Steps

| Area | Status | Notes |
|------|--------|-------|
| Neigh card interrupts | Not started | Requires playing a card outside your turn; no interrupt mechanic exists |
| EVERY_TURN special phase | Partial | `ActivePlayerHasEveryTurnCards()` exists but never called |
| Win condition UI | Partial | `CheckWinCondition()` logs only; no game-over screen or state |
| Pass action | Not started | Player can't skip their Action phase turn |
| Hand size >8 cards | Not handled | `HandStable.PositionCardsInStable()` throws at >8 |
| Multi-player (>2) | Not started | `CardManager` hardcodes "first non-active player" as opponent |

---

## Reference Docs

Detailed reasoning and per-card notes live in `docs/` — read on demand, not needed every session.

- `docs/design-decisions.md` — full reasoning behind structural choices (CardActionExecutor player reassignment, DowngradeStable ownership, layout decisions, OnEnable action pattern)
- `docs/cards/card-implementation-guide.md` — decision tree, action-type reference, and template for implementing any card
- `docs/cards/fuck-marry-kill.md` — implementation detail, execution trace, quirks, and test checklist for FMK
- `docs/stable-positioning.md` — layout formula, B2 fix explanation, subclass override guide, and regression test
- `docs/future-architecture-mvc.md` — when and how to migrate to a model-separated architecture (prerequisite for multiplayer, AI, save/load)
- `docs/issues.md` — tracked bugs and tech debt with fix directions
