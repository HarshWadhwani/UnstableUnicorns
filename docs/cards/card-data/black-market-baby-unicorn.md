# Black Market Baby Unicorn

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
> "If this card is in your Stable at the beginning of your turn, you may DISCARD 2 cards, then bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=2 }
- NEW: BringFromNurseryAction { cardSubtype=Baby } — brings a Baby Unicorn from Nursery into active player's stable.

## Passive Interfaces
None

## Implementation Notes
Optional effect — player may choose not to discard. Requires a BringFromNursery action type not yet implemented.
