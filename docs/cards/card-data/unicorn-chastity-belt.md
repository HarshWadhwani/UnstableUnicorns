# Unicorn Chastity Belt

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Your Unicorn cards cannot be destroyed."

## Action Mapping
NEW: passive ability — while this card is in your stable, your Unicorn cards cannot be destroyed by any effect.

## Passive Interfaces
None

## Implementation Notes
Global passive protection for all unicorns in this player's stable. Requires a destroy-immunity check in DestroyCardAction (or wherever destroy is resolved) to skip protected stables.
