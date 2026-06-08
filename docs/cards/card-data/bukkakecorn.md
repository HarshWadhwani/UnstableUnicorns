# Bukkakecorn

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
> "If this card is in your Stable at the beginning of your turn, you may DISCARD 3 cards, then STEAL a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=3 }
- StealUnicornAction {}

## Passive Interfaces
None

## Implementation Notes
Optional effect — player may choose not to discard. Cost is 3 discards; StealUnicornAction already exists.
