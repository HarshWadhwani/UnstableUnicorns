# Autoerotic Asphyxiation

| Field | Value |
|-------|-------|
| wiki_type | Downgrade |
| card_type | DOWNGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, DISCARD a card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
Mandatory (not optional) — active player must discard 1 card each turn. Applied to the player who has this Downgrade in their stable.
