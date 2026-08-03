# Remaining Cards — Execution Plan

Snapshot as of 2026-08-02, after 23/91 cards implemented. Derived by scanning every `docs/cards/card-data/*.md` file not yet marked `impl_status: done` for `NEW:` markers in their **Action Mapping** section.

**Refreshing this doc:** re-run this to regenerate the counts —
```bash
cd docs/cards/card-data
for f in $(grep -L "impl_status | done" *.md | grep -v "_checklist\|_template" | sort); do
  grep -q "NEW:" "$f" && echo "BLOCKED: $f" || echo "READY: $f"
done
```
Cards move between sections as new action types/frameworks get built or cards get implemented — update this file (or just regenerate it) rather than trusting it blindly once it's stale.

---

## Ready now (23) — existing action types cover them

No blockers. Mostly plain Baby Unicorns and Magical Unicorns playable via the existing action list (`DiscardCardAction`, `GiveCardAction`, `DestroyCardAction`, `PullCardAction`, `SacrificeCardAction`, `StealUnicornAction`, `TakeFromDiscardAction`, `SearchDeckForCardAction`, `PlayCardFromHandAction`, `RevealTopDeckAction`) or no effect at all. Just run `/add-card` on each.

- Baby Unicorn of Incest
- Buck Naked
- Bye Bye Baby Unicorn
- Cannibal Baby Unicorn
- Conjoined Baby Unicorn
- Double Agent Unicorn
- Dumpster Baby Unicorn
- Faceless Baby Unicorn
- Free Candy Unicorn
- Fucking Cute Baby Unicorn
- Fucking Ugly Baby Unicorn
- Fuzzy Hoofcuffs
- Homicidal Psychocorn
- Manscaped Llamacorn
- Pageant Baby Unicorn
- Putting on a Show
- Sadomasocorn
- Semenbiscuit
- Shotgun Baby Unicorn
- Someone Else's Baby Unicorn
- Tasty Baby Unicorn
- The Bitchiest Unicorn
- Upside Down Baby Unicorn

---

## Blocked (45) — grouped by what's missing, roughly cheapest → most expensive

### Almost ready — small extension to an existing action (1 card)
- **Unicorn with Benefits** — extend `PlayCardFromHandAction` with an optional unicorn-subtype filter, same pattern as `StealUnicornAction.targetSubtype`. Not a new action type.

### Small, self-contained new actions — same shape as ones already built (7 cards, ~3 action types)
- `BringFromNurseryAction` (Nursery → active player's stable, mirrors `TakeFromDiscardAction`): **Black Market Baby Unicorn**, **Kittencorn in Heat**, **Unexpected Miracle Unicorn**
- `DrawCardAction` (draw N from own deck, no prompt — mirrors the normal Draw-phase `CardManager.DrawCard`): **Unicorn Dancer**, **Unicorn Speed** (both `numberOfCards=1`)
- `SearchDeckForTypeAction` (near-identical to existing `SearchDeckForCardAction`, but match by `CardType` instead of exact `CardData` subclass): **Moist Unicorn**

### Interrupt mechanic — one investment unlocks 5 cards
"Play a card outside your turn to cancel another play." Pre-existing gap, already tracked in `CLAUDE.md`'s Known Gaps table as "Neigh card interrupts."
- Hell Neigh!
- Neigh, Bitch!
- Neigh Means Neigh
- Neigh, Motherfucker!
- The Safeword is Neigh

### Passive/continuous stable-modifier effects — needs a general framework beyond `ISacrificeShield` (14 cards)
Not one mechanic — hand-limit modifiers, play restrictions, destroy-immunity, win-condition double-counting, sacrifice-redirects. Each likely needs its own hook point. Probably the largest architectural lift in the remaining set.
- Blow Up Unicorn (may-sacrifice-to-intercept, like `ISacrificeShield` but optional)
- Eunuchorn (blocks all Baby Unicorns from entering any stable)
- Giant Horned Cock (counts as 2 for win condition; blocks own Magic plays)
- Horse Shit (renames + immunizes affected player's Unicorns)
- Limp Horn (blocks own Upgrade plays)
- Mid-sex Charlie Horse (strips Magical Unicorn effects, treats as Basic)
- Mother Fuckin' Flying Unicorn (redirect-to-hand instead of discard on sacrifice/destroy)
- Horny Flying Unicorn (same redirect-to-hand behavior, plus a `TakeFromDiscardByTypeAction { cardType=NEIGH }` half)
- Total Stud Unicorn (counts as 2 for win condition; blocks own Basic Unicorn plays)
- Uncut Unicorn (+1 hand limit)
- Unicorn Butt Plug (-4 hand limit)
- Unicorn Chastity Belt (Unicorn destroy-immunity)
- Unicorn Dungeon (extra Action-phase play)
- Unicorn Flatulence (mandatory return-to-hand + auto-sacrifice-if-stable-has-no-unicorns passive check)

### "View + take/destroy from opponent's hand" — new hand-visibility interaction (3 cards)
Nothing currently lets a player see another player's hand at all.
- Entitled Unicorn (view + take a Unicorn)
- Hoof Job (view + take any card)
- Officer Hornie (view + destroy a Unicorn)

### "Choose between two effects" prompt (2 cards)
Needs a new pending-action type where the *target* player (not necessarily the active player) picks which of two action sequences resolves.
- Kink Shame
- Sex, Drugs, and Unicorns

### All-players effects — no "for each player" loop exists yet (4 cards)
- Cult Leader Unicorn (all sacrifice a Unicorn)
- Safe Sex (all return a Baby Unicorn to Nursery)
- Unicorn Acid Trip (all discard + redraw; active player draws +2 more)
- Rainbow Shitstorm (full game-state reset — most complex single card in the whole remaining set)

### Move/loan/return card between stables (4 cards)
- Dominatrix Whip (move a Unicorn from any stable to any *other* player's stable)
- Unicorn Cuckold (loan a Unicorn to the opponent, auto-return at end of turn — needs a new delayed/end-of-turn trigger system)
- Sextra-Terrestrial Unicorn (return a card from opponent's stable to their own hand)
- Unicorn Flatulence (also listed above — passive check half)

### Skip-phase/skip-turn variants (2 cards)
`TurnManager.skipNextDrawPhase` is a precedent but doesn't cover skipping Action phase or an entire turn.
- Sticky Situation (choose to skip own Draw or Action phase)
- Unicorn Hangover (opponent skips their entire next turn)

### Everything else — one-off, no clean bucket (5 cards)
- Make it Rain (`DrawCardAction{2}` + new `TakeAnotherTurnAction`)
- Make It Snow (`DrawCardAction{3}` + new `TakeAnotherTurnAction`)
- Unicorgy (`DrawCardAction` with a dynamic count = number of Basic Unicorns in stable)
- Straight But Curious Unicorn (`PeekDeckAction` — view top N cards without moving/reordering them; no "reveal without consuming" pattern exists yet)
- Peeping Narwhal (`RevealHandsAction` — force all other players to reveal hands; related to the hand-visibility gap above)

---

## Suggested order

1. Burn down the 23 ready cards via `/add-card`.
2. Build `PlayCardFromHandAction`'s subtype extension → Unicorn with Benefits.
3. Build the three small new actions (`BringFromNurseryAction`, `DrawCardAction`, `SearchDeckForTypeAction`) → 7 more cards.
4. Tackle the interrupt mechanic once → 5 Neigh cards at once.
5. Hand-visibility interaction → 3 cards, plus reusable for Peeping Narwhal.
6. Everything else roughly by ascending complexity, saving Rainbow Shitstorm and the passive-effects framework for last (highest design cost, most likely to need a real architecture decision rather than a quick add).
