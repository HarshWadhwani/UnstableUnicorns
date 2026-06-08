# Unicorn Dancer

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

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may DRAW a card and DISCARD a card."

## Action Mapping
NEW: DrawCardAction { numberOfCards=1 }
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
Optional. Draw 1 then discard 1 — net neutral card exchange. DrawCardAction does not yet exist as an explicit action type (drawing happens via the Draw phase, not a CardAction).
