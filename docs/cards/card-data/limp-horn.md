# Limp Horn

| Field | Value |
|-------|-------|
| wiki_type | Downgrade |
| card_type | DOWNGRADE |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "You cannot play Upgrade cards."

## Action Mapping
NEW: passive ability — while this card is in your stable, you cannot play Upgrade cards.

## Passive Interfaces
None

## Implementation Notes
Passive restriction on the player who has this Downgrade. Requires a CanPlay gate at the player/game level that checks for Limp Horn presence before allowing Upgrade cards to be played.
