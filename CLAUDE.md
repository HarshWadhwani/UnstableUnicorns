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
- Each subclass sets `cardType`, `specialActionType`, and `afterAction` in `OnEnable()`
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

Card effects are composed from serializable `CardAction` subclasses (`DiscardCardAction`, `GiveCardAction`, `DestroyCardAction`) defined on the `CardData`.

**Execution flow:**
1. `CardData.TriggerSpecialAction(sourceCard)` → `CardActionExecutor.ExecuteActions()`
2. Actions run sequentially via a `Queue<CardAction>`
3. Actions requiring player input set a `PendingActionType` on `CardActionExecutor` and temporarily reassign `turnManager.activePlayer` to the prompted player — this is how click routing works during effects
4. The next click on the appropriate `CardSpace` calls `CardActionExecutor.ExecutePendingAction(card)`, which decrements `pendingCardsRemaining` and resumes the queue when done

### Adding a New Card

1. Create a new `CardData` subclass (or use an existing one) with `[CreateAssetMenu]`
2. Set `cardType`, `specialActionType`, `afterAction` in `OnEnable()`
3. Populate `actions` list with `CardAction` instances (see `FuckMarryKillCardData` as reference)
4. Create a `.asset` file in `Assets/Resources/CardDataInstances/<CardType>/`
5. Set `instances` to control how many copies appear in the deck

### Key Enums

- `CardType`: `UNICORN`, `MAGIC`, `UPGRADE`, `DOWNGRADE`, `NEIGH`
- `SpecialActionType`: `IMMEDIATE` (triggers on play), `EVERY_TURN`, `NONE`
- `AfterAction`: `DISCARD`, `PLACE_IN_STABLE`, `PLACE_IN_ENEMY_STABLE`
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
| `DestroyCardAction` with `TargetStable.Any` | Not implemented | Defaults to `unicornStable` with a warning log |
| `AfterAction.PLACE_IN_ENEMY_STABLE` | Not implemented | Enum value exists, never routed in `CardManager` |
| Hand size >8 cards | Not handled | `HandStable.PositionCardsInStable()` throws at >8 |
| Multi-player (>2) | Not started | `CardManager` hardcodes "first non-active player" as opponent |

---

## Reference Docs

Detailed reasoning and per-card notes live in `docs/` — read on demand, not needed every session.

- `docs/design-decisions.md` — full reasoning behind structural choices (CardActionExecutor player reassignment, DowngradeStable ownership, layout decisions, OnEnable action pattern, AfterAction separation)
- `docs/cards/fuck-marry-kill.md` — implementation detail, execution trace, quirks, and test checklist for FMK
- `docs/future-architecture-mvc.md` — when and how to migrate to a model-separated architecture (prerequisite for multiplayer, AI, save/load)
- `docs/issues.md` — tracked bugs and tech debt with fix directions
