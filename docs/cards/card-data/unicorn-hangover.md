# Unicorn Hangover

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Force another player to skip their next turn."

## Action Mapping
NEW: SkipTurnAction { targetPlayer=Opponent } — causes the target player to skip their entire next turn.

## Passive Interfaces
None

## Implementation Notes
Requires a "skip turn" flag on TurnManager or Player that gets consumed at the start of that player's next turn.
