# Horny Flying Unicorn

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
> "When this card enters your Stable, you may add a Neigh card from the discard pile to your hand. If this card is sacrificed or destroyed, return it to your hand."

## Action Mapping
- NEW: TakeFromDiscardByTypeAction { cardType=NEIGH } — take a Neigh card specifically from the discard pile.
- NEW: passive ability — if this card is sacrificed or destroyed, return it to hand instead of discard pile.

## Passive Interfaces
None

## Implementation Notes
Two parts: (1) on-enter optional effect to retrieve a Neigh from discard (more specific than TakeFromDiscardAction which lets player choose any card); (2) persistent passive: returns to hand instead of discard when sacrificed/destroyed.
