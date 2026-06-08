# Make It Snow

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
> "DISCARD your hand, then DRAW 3 cards and take another turn."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=Random, numberOfCards=999 } — discard entire hand
- NEW: DrawCardAction { numberOfCards=3 }
- NEW: TakeAnotherTurnAction {}

## Passive Interfaces
None

## Implementation Notes
Discard entire hand then draw 3. Requires "take another turn" mechanic. numberOfCards=999 is a convention for "all cards in hand."
