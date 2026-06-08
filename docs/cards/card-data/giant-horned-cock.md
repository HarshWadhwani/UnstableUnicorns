# Giant Horned Cock

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "This card counts for 2 Unicorns. You cannot play Magic cards."

## Action Mapping
NEW: passive ability — this card counts as 2 unicorns toward the win condition; while this card is in your stable you cannot play Magic cards.

## Passive Interfaces
None

## Implementation Notes
Two passive effects: (1) counts as 2 unicorns for win condition — requires unicorn count override; (2) restricts active player from playing Magic cards — requires a CanPlay-style restriction on the player level, not just the card level.
