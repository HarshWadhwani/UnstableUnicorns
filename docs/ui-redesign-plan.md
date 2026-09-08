# UI Redesign — "Storybook Stable"

Visual pass to fix the ad-hoc colours, the unreadable card text, and the unlabelled
board. Direction: **bright pastel / storybook** (sugar-paper table, warm cream cards,
one pastel hue per card type, a single turquoise accent for "your turn / clickable").

**Mockup (target):** https://claude.ai/code/artifact/16ecd2a9-b048-4c29-aa3c-faee524b484a
— rendered board, card anatomy, palette tokens with Unity 0–1 RGB, and translation notes.

## Palette — single source of truth is `Assets/Scripts/UI/UiPalette.cs`

| Role | Hex | Unity RGB |
|---|---|---|
| Table (board) | `#E8E3F1` | 0.910, 0.890, 0.945 |
| Recess / empty slot | `#DBD3EA` | 0.859, 0.827, 0.918 |
| Card / panel cream | `#FCF8F1` | 0.988, 0.973, 0.945 |
| Art window (sunken) | `#F4EDDF` | 0.957, 0.929, 0.875 |
| Ink (text on cream) | `#3C2F3B` | 0.235, 0.184, 0.231 |
| Ink-soft | `#857587` | 0.522, 0.459, 0.529 |
| Accent (turn / clickable) | `#23AEAA` | 0.137, 0.682, 0.667 |
| Card back / deck | `#4C3E68` | 0.298, 0.243, 0.408 |
| Basic / Baby Unicorn | `#F4A4C0` | 0.957, 0.643, 0.753 |
| Magical Unicorn | `#A6B4EE` | 0.651, 0.706, 0.933 |
| Magic | `#F1C74F` | 0.945, 0.780, 0.310 |
| Upgrade | `#8FD2A8` | 0.561, 0.824, 0.659 |
| Downgrade | `#EE9877` | 0.933, 0.596, 0.467 |
| Neigh | `#C4A1DD` | 0.769, 0.631, 0.867 |

Type font: **Fredoka** (display / names / HUD) + **Nunito** (rules text) — not imported yet
(Phase 5); everything currently renders on the default LiberationSans SDF.

## Phases

### ✅ Phase 0 — Foundation
`Assets/Scripts/UI/UiPalette.cs` — palette + `ForCard()` / `TypeLabel()` / `TriggerLabel()` helpers.

### ✅ Phase 1 — Readability
- `GameScene.unity`: CanvasScaler → **Scale With Screen Size**, 1920×1080, match 0.5; camera clear
  colour → Table (opaque); GameBoard sprite tint → Table.
- `Card.prefab`: card 66×94 → **100×140**, cream rounded face; name/description TMP auto-size
  (name 9–17 bold ink, description 7–11 ink-soft); dark ink instead of pure black.

### ✅ Phase 2 — Card visual system
- `Card.prefab` restructured: type-coloured **Ribbon** band holding the name, framed **art window**,
  small type **chip**, navy framed card-back; vestigial `SpriteRenderer` removed.
- `Assets/Scripts/UI/CardVisuals.cs` on the prefab — `Card.Initialize` calls `Apply(cardData)` which
  paints ribbon / art tint / chip from `UiPalette.ForCard`.
- `Assets/Scripts/UI/CardHoverZoom.cs` on the prefab — hovering a **hand** card lifts + scales it
  ~1.7× so rules are readable without shrinking the fan. No-op for cards in stables / deck / discard.

### ☐ Phase 3 — (folded into Phase 2) hover-to-read
Shipped early with Phase 2 (`CardHoverZoom`). Possible follow-ups: straighten the card's fan
rotation while hovered; re-capture base transform if `HandStable.PositionCardsInStable` runs mid-hover.

### ✅ Phase 4 — Board chrome + layout
`Assets/Scripts/UI/BoardChrome.cs` — created via `AddComponent` from `CardActionExecutor` (same
pattern as `NeighManager`); discovers Canvas / `TurnManager` / stables / piles itself in `Start`
and builds, with **zero scene edits**:
- a `Recess`-rimmed `Table` **board surface** (and disables the world-space `GameBoard` sprite, which
  couldn't scale with the canvas — fixes the world/UI drift)
- uppercase zone labels: `DECK` · `NURSERY` · `DISCARD` above each pile
- a cream **HUD panel** (top-right): turn number + active player, and two win-progress bars
  (`Image` fill = `unicornStable.spaceCards.Count / winConditionCount`, live)
- an **active-player glow** `Image` behind `turnManager.activePlayer`'s area, toggled per frame

`TurnManager.turnNumber` (display only) added for the HUD.

Layout, in `GameScene.unity` + `Player.prefab` (value edits): CanvasScaler reference `1520×855`
(uniform ~1.26× scale-up of the whole board — bigger, more readable, relative positions kept);
player bands pulled to root Y `±230`, root size `1000×400`; hand / stable / stack offsets spread;
centre piles to x `±330`; phase text + Skip button moved to the top-right near the HUD.

### ☐ Phase 5 — Fonts
Import **Fredoka** + **Nunito** as TMP font assets (Font Asset Creator), repoint `UiPalette` /
prefab text. Needs the `.ttf` files + an Editor pass.

### ✅ Phase 6 — Polish
- **Drop shadows** — `CardVisuals.Awake` adds a hard down-right `Shadow` to the card front/back
  (paper cut-out look).
- **Card-back motif** — a faint rotated-square diamond `Image` on the back (no font glyph, so no
  fallback-character warning).
- **Empty-stable slot wells** — `BoardChrome` builds 7 faint placeholder cells in each unicorn
  stable, matching `Stable.PositionCardsInStable`'s slot maths; cards drop on top and cover them.
- **HUD unification** — `BoardChrome` reparents the scene's phase text + Skip/Pass button *into*
  the top-right HUD panel and restyles them (turquoise button, ink text). Scene label →
  "Skip / Pass". Win bars now actually update (a `winFills[i]` assignment was missing; width is
  driven by `anchorMax.x` so it needs no sprite).
- **Game-over overlay** — hidden scrim + "X wins!" plaque, shown when a stable reaches
  `winConditionCount` (input freezes; no restart button yet).
- **Hand fan rewrite** — `HandStable.PositionCardsInStable` is now parametric arc maths instead of
  the 1–8 lookup tables: one card sits straight up and centred; each extra card opens the fan by
  `fanTotalAngle / 6` degrees; centre card upright and highest, outer cards tilt and dip. New
  `fanCardSpacing` field (56) beside `fanTotalAngle` (90).
- **Rounded runtime sprites** — `BoardChrome` scavenges the built-in 9-slice UISprite from an
  existing scene `Image` (`Resources.GetBuiltinResource` no longer works in this uGUI version);
  falls back to sharp rectangles.

### ☐ Follow-ups
- Game-over needs a restart / rematch button.
- Board surface is still a flat two-tone panel — the mockup's subtle radial + hatch texture is
  unshipped.
- `docs/stable-positioning.md` predates the hand-fan rewrite (it documents the base `Stable`
  formula, not the fan, but worth a check next time it's touched).
