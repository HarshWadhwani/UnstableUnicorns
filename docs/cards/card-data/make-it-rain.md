# Make it Rain

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "DISCARD 2 cards, then DRAW 2 cards and take another turn."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=2 }
- NEW: DrawCardAction { numberOfCards=2 }
- NEW: TakeAnotherTurnAction {} — grants the active player an additional turn immediately after this one.

## Passive Interfaces
None

## Implementation Notes
Requires a "take another turn" mechanic — the active player gets a full extra turn after this one resolves. Not currently implemented.
