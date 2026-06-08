# Unicorgy

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "DRAW a number of cards equal to the number of Basic Unicorn cards in your Stable."

## Action Mapping
NEW: DrawCardAction { numberOfCards=countOf(BasicUnicornsInActivePlayerStable) } — draws cards based on a dynamic count.

## Passive Interfaces
None

## Implementation Notes
Variable draw count based on game state (number of Basic Unicorns in stable). Requires dynamic numberOfCards evaluation rather than a fixed value.
