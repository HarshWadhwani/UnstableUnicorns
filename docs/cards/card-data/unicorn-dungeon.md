# Unicorn Dungeon

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
> "If this card is in your Stable at the beginning of your turn, you may play 2 cards during your Action Phase."

## Action Mapping
NEW: passive ability — grants the active player an extra card play during their Action Phase (2 cards instead of 1).

## Passive Interfaces
None

## Implementation Notes
Requires Action Phase to support playing 2 cards. Currently hardcoded to 1 card per Action Phase. Would need a modifier on TurnManager or player state.
