# Kittencorn in Heat

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
> "When this card enters your Stable, you may bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
NEW: BringFromNurseryAction { cardSubtype=Baby } — brings a Baby Unicorn from Nursery into active player's stable.

## Passive Interfaces
None

## Implementation Notes
On-enter effect; optional. Requires Nursery interaction not currently implemented. (Note: NSFW Base version had an EVERY_TURN discard cost variant — 2nd Edition is simpler.)
