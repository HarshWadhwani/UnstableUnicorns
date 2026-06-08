# Pony Play

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | PonyPlayCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may pull a card at random from another player's hand. If you do, skip your Draw phase."

## Action Mapping
- PullCardAction { numberOfCards=1, skipDrawPhaseOnSuccess=true }

## Passive Interfaces
None
