# Fuzzy Hoofcuffs

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may discard 2 cards, then steal a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=2 }
- StealUnicornAction {}

## Passive Interfaces
None

## Implementation Notes
Optional. Cost is 2 discards from own hand. StealUnicornAction already exists. Functionally similar to Bukkakecorn (3 discards) but costs 1 less.
