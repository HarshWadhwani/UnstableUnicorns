# Straight But Curious Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "When this card enters your Stable, you may look at the top 3 cards in the deck, then return them to the top of the deck in the same order."

## Action Mapping
NEW: PeekDeckAction { numberOfCards=3 } — player views top N cards of the deck then returns them in the same order (no rearrangement).

## Passive Interfaces
None

## Implementation Notes
Information-only scry effect. Cards remain in the same order. No cards move to hand or discard.
