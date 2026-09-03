# Kittencorn in Heat

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | KittencornInHeatCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, you may bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
- BringFromNurseryAction {} — brings the next Baby Unicorn from Nursery into the active player's Unicorn stable.

## Passive Interfaces
None

## Implementation Notes
On-enter effect. Single action, no partial-effect risk — skips silently if the Nursery is empty. (Note: NSFW Base version had an EVERY_TURN discard cost variant — 2nd Edition is simpler.)
