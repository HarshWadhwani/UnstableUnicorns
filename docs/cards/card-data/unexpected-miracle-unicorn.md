# Unexpected Miracle Unicorn

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
> "When this card enters your Stable, you may DISCARD a card, then Bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }
- NEW: BringFromNurseryAction { cardSubtype=Baby }

## Passive Interfaces
None

## Implementation Notes
Optional. Discard cost + bring a Baby Unicorn from the Nursery. Similar to Black Market Baby Unicorn but cheaper (1 discard vs 2) and IMMEDIATE trigger instead of EVERY_TURN.
