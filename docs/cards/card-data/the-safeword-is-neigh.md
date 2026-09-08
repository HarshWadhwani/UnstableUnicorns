# The Safeword is Neigh

| Field | Value |
|-------|-------|
| wiki_type | Instant |
| card_type | NEIGH |
| copies | 5 |
| trigger | NONE |
| can_play | always |
| impl_status | done |
| impl_class | BasicNeighCardData.cs (rollup) |

## Effect (2nd Edition)
> "Play this card when another player tries to play a card. Stop their card from being played and send it to the discard pile."

## Action Mapping
NEW: passive ability — interrupt mechanic; played out of turn to cancel another player's card play.

## Passive Interfaces
None

## Implementation Notes
Functionally identical to Hell Neigh! and Neigh, Bitch! Standard counter/interrupt, handled by `NeighManager` (see CLAUDE.md). Rolled into `BasicNeighCardData` (`NeighType.Basic`) via `cardNameVariations`. Most copies of any Neigh variant (5 copies).
