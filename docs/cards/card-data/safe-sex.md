# Safe Sex

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
> "Each player (including you) must return a Baby Unicorn card from their Stable to the Nursery."

## Action Mapping
NEW: AllPlayersReturnBabyUnicornAction {} — each player returns one Baby Unicorn from their stable to the Nursery.

## Passive Interfaces
None

## Implementation Notes
Affects all players including active player. Each player must return exactly one Baby Unicorn from their stable to the Nursery. Requires Nursery interaction.
