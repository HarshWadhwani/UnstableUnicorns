# Semenbiscuit

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
> "When this card enters your Stable, SACRIFICE all Downgrade cards in your Stable."

## Action Mapping
- SacrificeCardAction { targetStable=Downgrade, sacrificeAll=true }

## Passive Interfaces
None

## Implementation Notes
Mandatory (not optional). Sacrifices all downgrade cards in the active player's own stable — effectively clears all downgrades on entry.
