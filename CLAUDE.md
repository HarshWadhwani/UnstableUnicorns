# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Working Instructions

- When the user declares an issue complete, update its **Status** line in `docs/issues.md` and mark it ✅ in the tracker table — do this in the same response.
- When the user confirms a card or feature has been tested and works (in Play mode or otherwise), treat that confirmation as a standing instruction to, in the same response, without being asked again each time:
  1. Verify the underlying Unity `.asset`(s) are actually correct — read them directly and check `cardNameVariations` and `instances` are non-empty/non-zero (a recurring gap on nearly every card added so far; see `.claude/commands/add-card.md`'s "Known recurring gotcha").
  2. Update the card's file in `docs/cards/card-data/` (`impl_status`, `impl_class`) and `_checklist.md` (✅ + Summary count) if not already done.
  3. Update `CLAUDE.md` if the change touched architecture (new action type, new enum value, changed table/enum documented above) — check the relevant section isn't now stale.
  4. Add a `CHANGELOG.md` entry.
  5. `git add` the specific changed files, commit, and push to `origin/master`.
  Skip only the steps that don't apply (e.g. no CLAUDE.md change needed for a vanilla no-effect card). Don't wait for a separate "commit and push" prompt — the test-confirmation itself is the trigger.

## Project Overview

A Unity 6 (6000.2.6f2) digital implementation of the card game **Unstable Unicorns**. UI-based (Canvas/RectTransform), currently supporting 2 players.

## Development

This is a Unity project — there are no CLI build or test commands. All development happens in the Unity Editor (open the project and use Play mode to test). Scripts are in `Assets/Scripts/` and are compiled automatically by Unity when saved.

Card data assets (ScriptableObjects) live in `Assets/Resources/CardDataInstances/` and are loaded at runtime via `Resources.LoadAll<CardData>`.

## Architecture

### Core Data Model

**`CardData` (ScriptableObject)** is the base for all card types:
- Subclasses: `UnicornCardData`, `MagicCardData`, `UpgradeCardData`, `DowngradeCardData`, `NeighCardData`
- Each subclass sets `cardType` and `specialActionType` in `OnEnable()`, always calling `base.OnEnable()` first so the chain reaches `CardData.OnEnable()`
- Cards with effects define a `List<CardAction> actions` — these are executed sequentially by `CardActionExecutor`
- `CardData.NextCardName()` cycles through `cardNameVariations` by index (reset in `OnEnable()`) — the default behavior for every subclass, not just unicorns. This is how one shared asset (e.g. `BabyUnicornCardDataInstance.asset`, `Basic Unicorn Card Data.asset`) can represent many distinctly-named cards with identical effects; any future rollup (e.g. Neigh cards) gets this for free without overriding anything.

**`Card` (MonoBehaviour)** is the runtime instance. It holds a reference to its `CardData` and its current `CardSpace`.

### CardSpace Hierarchy

`CardSpace` (abstract) → `Stable` → `HandStable`, `UnicornStable`, `UpgradeStable`, `DowngradeStable`  
`CardSpace` → `Deck`, `DiscardPile`

- `CardSpace.HandleCardClick(Card)` is the entry point for all player interactions
- `Stable.PositionCardsInStable()` handles card layout; `HandStable` overrides it with a fan layout
- `allowedTurnPhases` on each `CardSpace` gates which clicks are valid

### Manager Layer

- **`TurnManager`** — owns the player list, active player, and turn phase (`Draw → Action → (ImmediateSpecial) → (EveryTurnSpecial) → Draw`). Call `StartNextTurnPhase()` to advance. In `EveryTurnSpecial`, `EVERY_TURN` cards in the active player's stables split into two queues: Downgrade cards are **mandatory** — `AdvanceToNextPlayerTurn` auto-fires them in order via `ActivateNextMandatoryCard`, no click required, and the Skip button (`SkipEveryTurnPhase`) no-ops while any remain (`TurnManager.CanSkipEveryTurnPhase` drives the button's `interactable` state). Unicorn/Upgrade cards are **choice** — the player clicks the card in its stable (`TryActivateEveryTurnCard`, routed through `Stable.HandleCardClick`) or presses Skip to bypass the rest. Both queues are snapshotted once, in `AdvanceToNextPlayerTurn`, from the state of the active player's stables at the moment their turn starts — a card brought into a stable mid-phase (e.g. via A Little Side Hustle) does not retroactively join that turn's queue even if it's itself an EVERY_TURN card; it becomes eligible starting next turn. A `CardAction` that can't run (e.g. `DiscardCardAction` on an empty hand) returns without setting a pending action, so `CardActionExecutor` silently chains to the next one — this is what makes mandatory effects skip cleanly when impossible, with no extra guard needed. For a choice card whose effect could otherwise run *partially* (e.g. Bukkakecorn: discard 3 then steal — a hand of 1-2 cards shouldn't discard-then-nothing), `TryActivateEveryTurnCard` checks `CardData.CanActivateEveryTurn(activePlayer, opponentPlayer)` before running the action queue at all; returning `false` removes the card from the choice queue (the click is still consumed) without firing any of its actions. Only wired into the choice path, not `ActivateNextMandatoryCard`.
- **`CardManager`** — handles all card movement (`MoveCard`, `DrawCard`, `PlayCardForCurrentPlayer`). Routes played cards to the correct destination based on `CardType`. `PlayCardForCurrentPlayer` first checks `CanPlay`, then offers a Neigh window via `NeighManager.TryBeginContest`; if a contest opens the card stays in hand and `NeighManager` drives the rest. The confirmed-play body (fire `IMMEDIATE` effect, route the card) is split into the public `ResolvePlay` so `NeighManager` can call it when a contest resolves in the caster's favour.
- **`DeckManager`** — loads all `CardData` from Resources on `Start()`, instantiates `Card` prefabs, shuffles, and deals baby unicorns. `CardActionExecutor` holds a reference to it (wired in the scene) so actions like `SearchDeckForCardAction` can reach `playDeck` and call `ShuffleDeck`.
- **`NeighManager`** — the "play a Neigh out of turn to cancel a card" interrupt. Created at runtime by `CardActionExecutor.Awake` (`AddComponent` on the same GameObject; reads its `TurnManager`/`CardManager` refs off `CardActionExecutor.Instance`), so it needs **no scene wiring**. `TryBeginContest` opens a contest only for a plain Action-phase hand play (not a `PlayCardFromHandAction`/effect-triggered play, not a NEIGH card itself) when the other player holds a Neigh; while `AwaitingResponse` the current `Responder` clicks a `NeighCardData` in their hand (routed through a branch at the top of `HandStable.HandleCardClick`) or presses the Skip button, which doubles as **Pass** (`PhaseIndicator` adds a runtime `onClick` listener and relabels/enables it; `TurnManager.SkipEveryTurnPhase` now early-returns unless `currentPhase == EveryTurnSpecial` so its scene-persistent binding is inert during a contest). Each Neigh can be counter-Neigh'd by the other player, alternating, until someone passes or a `NeighType.Final` Neigh (`NeighCardData.CanBeNeighed == false`, i.e. Neigh Means Neigh) ends the chain. `ResolveContest`: an **even** `neighStack` count ⇒ the contested card resolves normally (via `CardManager.ResolvePlay`, then the phase advances exactly as `HandStable` would); **odd** ⇒ it goes to the discard pile and its effect never fires. A Neigh is "live" if its distance from the top of the stack is even; every live `NeighType.ForceOpponentDiscard` (Neigh, Motherfucker!) enqueues a `DiscardCardAction` (1 card, PlayerChooses) against the owner of the card it cancelled, run **before** the contested card resolves/discards. That sequencing uses the new optional `onComplete` callback on `CardActionExecutor.ExecuteActions` (when set, it replaces the built-in phase auto-advance; backward-compatible — existing callers pass nothing).

### CardAction System

Card effects are composed from serializable `CardAction` subclasses defined on the `CardData`:

| Action | Parameters | Effect |
|--------|------------|--------|
| `DiscardCardAction` | `targetPlayer` (ActivePlayer/Opponent), `selectionMode` (PlayerChooses/Random), `numberOfCards` | Forces target player to discard cards from hand to the discard pile. Skips silently if the target's hand is empty. |
| `GiveCardAction` | `giver` (ActivePlayer/Opponent), `numberOfCards` | Giver picks N cards from their hand and transfers them to the other player's hand. |
| `DestroyCardAction` | `destroyer` (ActivePlayer/Opponent), `targetStable` (Any/Unicorn/Upgrade), `numberOfCards`, `destroyAll` (bool) | Destroyer picks N cards from the opposing player's stables and sends them to the discard pile. `targetStable=Any` allows unicorn/upgrade/downgrade; `targetStable=Unicorn` restricts to unicorn cards only (uses `PendingActionType.DestroyUnicornCard`). `destroyAll=true` mirrors `SacrificeCardAction.sacrificeAll`: every card in scope is moved to discard immediately, no prompt, `numberOfCards` ignored — and sacrifice shields (`ISacrificeShield`) are **not** consulted (a mass destroy has no single target to intercept). `targetStable=Upgrade` is only supported with `destroyAll=true` (the interactive Upgrade path logs a warning and no-ops — no `PendingActionType` for it). Used by Buck Naked (`targetStable=Upgrade, destroyAll=true`). |
| `PullCardAction` | `numberOfCards`, `skipDrawPhaseOnSuccess` (bool) | Randomly moves N cards from the **opponent's** hand to the **active player's** hand. Capped at opponent's hand size. If `skipDrawPhaseOnSuccess=true` and ≥1 card was pulled, sets `TurnManager.skipNextDrawPhase` to skip the Draw phase. |
| `SacrificeCardAction` | `targetStable` (Unicorn/Upgrade/Downgrade/Any), `sacrificeAll` (bool), `numberOfCards`, `targetSubtype` (`UnicornType?`) | Moves cards from the **active player's own** stables to the discard pile. `sacrificeAll=true` auto-moves all matching cards immediately, no prompt. `sacrificeAll=false` prompts the active player to click a card in one of their own stables matching `targetStable`/`targetSubtype` (mirrors `DiscardCardAction`'s `PlayerChooses` mode); skips silently (no prompt) if no eligible card exists anywhere in scope. `targetSubtype` works like `StealUnicornAction.targetSubtype` and is only meaningful when `targetStable` includes the Unicorn stable. |
| `StealUnicornAction` | `targetSubtype` (`UnicornType?`, default `null` = any) | Active player picks a unicorn from the **opponent's** unicorn stable to move into their own. If `targetSubtype` is set, only unicorns of that `UnicornType` are eligible — skips silently if none match. The filter is carried through the click-prompt via `CardActionExecutor.pendingStealSubtypeFilter`; `Stable.HandleCardClick`'s `StealCard` branch enforces both `this is UnicornStable` and the subtype match. |
| `MoveSelfToOpponentStableAction` | none | Moves the source card itself from the active player's unicorn stable into the opponent's unicorn stable. Used by Polyamorous Unicorn to hop stables each turn. |
| `TakeFromDiscardAction` | none | Moves the top card of the discard pile into the active player's hand. Skips silently if the discard pile is empty. Currently always takes the top card — no player choice yet (see `docs/issues.md` M4). |
| `SearchDeckForCardAction` | `targetCardDataType` (`System.Type`) | Searches the **play deck only** (not hand/discard/stables) for a card whose `CardData` matches the given type, reveals it, moves it to the active player's hand, then shuffles the deck. Skips silently (but still shuffles) if no match is in the deck. No player prompt. Used by Bear Daddy Unicorn / Twinkicorn to fetch each other. |
| `PlayCardFromHandAction` | `cardType` (`CardType`), `targetSubtype` (`UnicornType?`, default `null` = any) | Prompts the active player to click a card of the given `CardType` in their **own hand**; the click routes through `CardManager.PlayCardForCurrentPlayer` (not a raw move), so it respects that card's own `CanPlay` and any `IMMEDIATE` trigger. Skips silently if the hand has no card of that type/subtype. `HandStable.HandleCardClick` enforces the filters via `CardActionExecutor.pendingPlayCardTypeFilter`/`pendingPlayCardSubtypeFilter`, rejecting clicks on the wrong type or subtype without clearing the prompt. Used by A Little Side Hustle (`UPGRADE`) to bring an Upgrade card from hand into the Stable outside the Action phase, and by Unicorn with Benefits (`UNICORN`, `targetSubtype=BASIC`) to bring in a Basic Unicorn. Basic Unicorns are `specialActionType=NONE`, so this is still untested for `IMMEDIATE`-triggering types (`MAGIC`/other `UNICORN` subtypes) — reusing it there would re-enter `CardActionExecutor.ExecuteActions` while the outer queue is still in flight. |
| `RevealTopDeckAction` | none | Reveals the top card of the play deck; routes it based on its own `CardType` — `UNICORN` to the active player's Unicorn stable (checks win condition), anything else to the active player's hand. No player prompt. Skips silently if the deck is empty. Used by Blaze and Graze. |
| `DrawCardAction` | `numberOfCards` | Draws N cards from the top of the active player's **own play deck** into their hand. No player prompt — mirrors the normal Draw-phase `CardManager.DrawCard`, called directly. Skips remaining draws silently if the deck runs out partway through. Used by Unicorn Dancer, Unicorn Speed. |
| `BringFromNurseryAction` | none | Moves the next Baby Unicorn from the Nursery directly into the active player's **Unicorn stable** (not hand) — mirrors `TakeFromDiscardAction`'s shape. Always takes `nursery.spaceCards[0]`; the Nursery is homogeneous (all Baby Unicorns), so which card moves is irrelevant. Checks the win condition after moving. Skips silently if the Nursery is empty. Used by Black Market Baby Unicorn, Kittencorn in Heat, Unexpected Miracle Unicorn. |
| `SearchDeckForTypeAction` | `targetCardType` (`CardType`) | Near-identical to `SearchDeckForCardAction`, but matches by `CardType` instead of an exact `CardData` subclass — for "search for a Unicorn card" (any Unicorn) rather than a specific named card. Used by Moist Unicorn. |

Each action's `Execute(executor, context)` resolves which players/spaces are involved and calls a `Prompt*` method on `CardActionExecutor` to pause the queue for player input.

**Execution flow:**
1. `CardData.TriggerSpecialAction(sourceCard)` → `CardActionExecutor.ExecuteActions()`
2. Actions run sequentially via a `Queue<CardAction>`
3. Actions requiring player input set a `PendingActionType` on `CardActionExecutor` and temporarily reassign `turnManager.activePlayer` to the prompted player — this is how click routing works during effects
4. The next click on the appropriate `CardSpace` calls `CardActionExecutor.ExecutePendingAction(card)`, which decrements `pendingCardsRemaining` and resumes the queue when done

`pendingSourceStable` locks the source space when an action targets a single stable (e.g., discard from hand). For `DestroyCard` it is `null` — the source is derived from `card.cardSpace` at click time, allowing the destroyer to pick from any opposing stable.

`pendingDestroyTargetPlayer` is set by `DestroyCardAction` before prompting and records exactly which player's stable cards may be selected from. `Stable.HandleCardClick` checks `player == pendingDestroyTargetPlayer` to accept or reject a click — this is more reliable than comparing against `turnManager.activePlayer`, which can be temporarily reassigned during multi-step action sequences. `pendingSacrificeTargetPlayer` is the same pattern for `SacrificeCardAction`'s `PlayerChooses` mode (`PendingActionType.SacrificeCard`); `pendingSacrificeTargetStable`/`pendingSacrificeSubtypeFilter` additionally restrict which stable type and unicorn subtype are valid to click, mirroring `pendingStealSubtypeFilter`.

Cards that cannot always be played (e.g., require a non-empty opponent stable) override `CardData.CanPlay(activePlayer, opponentPlayer)`. `CardManager.PlayCardForCurrentPlayer` calls this before triggering any action; returning `false` keeps the card in hand and cancels the play. The sibling hook `CanActivateEveryTurn(activePlayer, opponentPlayer)` (see `TurnManager` above) is the equivalent gate for an EVERY_TURN choice card's *stable* activation, not its play.

### Card Ability Interfaces

Passive card abilities that react to game events (rather than firing actions on play) are modelled as C# interfaces in `Assets/Scripts/CardData/CardAbilities/`. A `CardData` subclass implements the interface; game systems check for it at runtime.

| Interface | Method | Checked by | Effect |
|-----------|--------|------------|--------|
| `ISacrificeShield` | `bool CanInterceptDestroy(DestroyCardAction.TargetStable)` | `DestroyCardAction.Execute` | If any card in the target player's stables implements this and `CanInterceptDestroy` returns `true`, that card is automatically moved to the discard pile and the destroyer is never prompted to select a target. First match in unicorn → upgrade → downgrade order wins. |
| `IReturnsStolenCardOnLeave` | marker, no methods | `CardActionExecutor.ExecutePendingAction` (records the link), `CardManager.MoveCard` (performs the return) | When a `StealCard` steal resolves for a card implementing this, and the stolen card is a Baby Unicorn (`UnicornType.BABY` — never for anything else), the stolen card and its origin stable are recorded on `Card.linkedBabyUnicorn`/`linkedBabyUnicornOriginStable`. `CardManager.MoveCard` — the single choke-point for all card movement — checks this on every move out of a `UnicornStable`; if set, and the linked Baby Unicorn is still sitting where it was left (not itself discarded/destroyed/moved since), it's sent back to its origin stable. Used by Free Candy Unicorn. |

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

### UI Layer (`Assets/Scripts/UI/`)

Visual-only. Gameplay never reads from here.

- **`UiPalette`** — static class, the single source of truth for gameplay UI colour ("Storybook Stable" pastel scheme). `ForCard(CardData)` returns the type hue; `TypeLabel` / `TriggerLabel` return badge strings. Retune the whole look by editing this file.
- **`CardVisuals`** — on the `Card` prefab. `Card.Initialize` calls `Apply(cardData)` once the data is known; it paints the ribbon band, art-window tint, and type chip from `UiPalette`. Serialized refs to those child Images live on the prefab.
- **`CardHoverZoom`** — on the `Card` prefab. Lifts + scales a card ~1.7× on hover, but only while `card.cardSpace is HandStable`; no-op in stables / deck / discard. Cosmetic — clicks still route normally.
- **`BoardChrome`** — created at runtime by `CardActionExecutor.Awake` (`AddComponent`, like `NeighManager`); needs no scene wiring. In `Start` it discovers the Canvas / `TurnManager` / stables / piles and builds the board surface (disabling the world-space `GameBoard`), the `DECK` / `NURSERY` / `DISCARD` zone labels, a top-right HUD panel (turn number + two live win-progress bars), and a per-frame active-player glow. `TurnManager.turnNumber` (display-only counter) feeds the HUD.

Full plan and remaining phases (fonts, polish): `docs/ui-redesign-plan.md`.

### Key Enums

- `CardType`: `UNICORN`, `MAGIC`, `UPGRADE`, `DOWNGRADE`, `NEIGH`
- `SpecialActionType`: `IMMEDIATE` (triggers on play), `EVERY_TURN`, `NONE`
- `TurnPhase`: `Draw`, `Action`, `ImmediateSpecial`, `EveryTurnSpecial`
- `PendingActionType`: `None`, `DiscardCard`, `GiveCard`, `DestroyCard`, `DestroyUnicornCard`, `StealCard`, `PlayCardFromHand`, `SacrificeCard`

---

## Gameplay Rules

### Turn Structure
`Draw → Action → (ImmediateSpecial) → (EveryTurnSpecial) → (next player's Draw)`
- **Draw:** Active player clicks the top card of the play deck.
- **Action:** Active player plays one card from hand (pass not yet implemented).
- **ImmediateSpecial:** Runs when a `MAGIC` card's actions fire on play.
- **EveryTurnSpecial:** Runs at the start of each turn if the active player has any `EVERY_TURN` cards in their stables. Downgrade cards fire automatically (mandatory); Unicorn/Upgrade cards wait for a click or the Skip button (optional). See `TurnManager` in Manager Layer above.

### Win Condition
Player wins when `UnicornStable` reaches `winConditionCount` unicorns (default 7 — separate from `maxCardsInStable`, the layout/cap value). Currently logs only — no game-over state.

`maxCardsInStable <= 0` means uncapped (`Stable.AddCard` skips the full-stable check). The Upgrade stable is set to `0` — no printed limit in the real game. Only relevant to `UnicornStable` (base `Stable.PositionCardsInStable` divides layout width by it) and `HandStable`; `UpgradeStable`/`DowngradeStable` use `StackedStable`'s overlapping-stack layout, which ignores it entirely.

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
| Neigh card interrupts | Done (2-player) | `NeighManager` — see Manager Layer. Covers all 5 Neigh cards, counter-Neigh recursion, `Final` (uncounterable), and `ForceOpponentDiscard`. Only plain Action-phase hand plays are contestable; effect-triggered plays (`PlayCardFromHandAction`) are not, and the recursion assumes exactly 2 players. |
| Win condition UI | Partial | `CheckWinCondition()` logs only; no game-over screen or state |
| Pass action | Not started | Player can't skip their Action phase turn |
| Hand size >8 cards | Partial | Crash fixed (capped display at 7), but overflow cards aren't visible/selectable — see `docs/issues.md` M3 |
| Multi-player (>2) | Not started | `CardManager` hardcodes "first non-active player" as opponent |
| Dumpster Diving Unicorn discard-pile picker | Open | Always takes the top discard card automatically; card text implies free choice — see `docs/issues.md` M4 |

---

## Reference Docs

Detailed reasoning and per-card notes live in `docs/` — read on demand, not needed every session.

- `docs/design-decisions.md` — full reasoning behind structural choices (CardActionExecutor player reassignment, DowngradeStable ownership, layout decisions, OnEnable action pattern)
- `docs/cards/card-implementation-guide.md` — decision tree, action-type reference, and template for implementing any card
- `docs/cards/execution-plan.md` — which of the remaining 68 cards are ready to build now vs. blocked on missing functionality (grouped by what's missing), and a suggested build order — check before picking the next card to implement
- `docs/cards/fuck-marry-kill.md` — implementation detail, execution trace, quirks, and test checklist for FMK
- `docs/stable-positioning.md` — layout formula, B2 fix explanation, subclass override guide, and regression test
- `docs/ui-redesign-plan.md` — "Storybook Stable" visual pass: palette tokens, mockup link, phase checklist (Phase 0–2 done)
- `docs/future-architecture-mvc.md` — when and how to migrate to a model-separated architecture (prerequisite for multiplayer, AI, save/load)
- `docs/issues.md` — tracked bugs and tech debt with fix directions
