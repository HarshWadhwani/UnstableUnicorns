# Mother Fuckin' Flying Unicorn

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
> "When this card enters your Stable, you may choose any player. That player must DISCARD a card. If this card would be sacrificed or destroyed, return it to your hand instead."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }
- NEW: passive ability — if this card would be sacrificed or destroyed, return it to hand instead.

## Passive Interfaces
None

## Implementation Notes
"Choose any player" — in 2-player this is the opponent. Also has a persistent passive return-to-hand on sacrifice/destroy, similar to Horny Flying Unicorn.
