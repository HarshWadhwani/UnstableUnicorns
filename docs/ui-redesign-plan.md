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

### ☐ Phase 4 — Board chrome (runtime-built, zero scene edits)
New `Assets/Scripts/UI/BoardChrome.cs`, created via `AddComponent` from an existing manager (same
pattern as `NeighManager`). In `Start`, reads the existing `CardSpace` RectTransforms and builds:
- uppercase zone labels: Deck · Discard · Nursery, and Your Stable · Upgrades · Downgrades per player
- a cream **HUD panel** grouping the existing `PhaseIndicatorText` + Skip button, plus turn number
  and two win-progress bars (`Image` fill = `unicornStable.spaceCards.Count / winConditionCount`)
- an **active-player glow** `Image` behind `turnManager.activePlayer`'s area
- restyle the Skip button (cream, ink)
Also: move `GameBoard` from a world-space `SpriteRenderer` into the canvas as a full-rect `Image`
so the table and UI scale together (fixes the drift on non-16:9 windows).

### ☐ Phase 5 — Fonts
Import **Fredoka** + **Nunito** as TMP font assets (Font Asset Creator), repoint `UiPalette` /
prefab text. Needs the `.ttf` files + an Editor pass.

### ☐ Phase 6 — Polish
Storybook offset shadows (`Shadow` UI component or a shadow `Image`), empty-stable slot wells
(runtime `Image` per stable), card-back motif, rounded HUD panel sprite, hand-fan spacing tune
for the larger cards.
