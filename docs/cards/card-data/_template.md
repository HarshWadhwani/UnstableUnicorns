# Card Name

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

> **trigger values:** `IMMEDIATE` (fires when played / enters stable) · `EVERY_TURN` (start of owner's turn) · `NONE` (passive interface or no effect)
> **unicorn_subtype:** `BABY` · `BASIC` · `MAGIC` · `SPECIAL` — only for UNICORN card_type; omit row for other types
> **can_play:** describe condition that blocks play, or write `always`

## Effect (2nd Edition)
> "Exact 2nd edition card text."

## Action Mapping
Map the effect to existing action types from `docs/cards/card-implementation-guide.md`.
Use exact parameter syntax — this should be copy-pasteable into a CardData class.

```
- ActionType { param=Value, ... }
```

If no existing action type covers the effect, write:
```
- NEW: description of what needs to be built
```

## Passive Interfaces
List any `CardAbilities` interface this card implements (e.g. `ISacrificeShield`), or `None`.

## CanPlay Override
If `can_play` is not `always`, write the return expression:
```csharp
return opponentPlayer.unicornStable.spaceCards.Count > 0;
```

## Rulings
Notable wiki rulings that affect implementation. Omit section if none.

## Implementation Notes
Non-obvious edge cases, interactions, or deferred behaviour. Omit section if none.
