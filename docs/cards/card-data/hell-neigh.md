# Hell Neigh!

| Field | Value |
|-------|-------|
| wiki_type | Instant |
| card_type | NEIGH |
| copies | 3 |
| trigger | NONE |
| can_play | always |
| impl_status | done |
| impl_class | BasicNeighCardData.cs (rollup) |

## Effect (2nd Edition)
> "Play this card when another player tries to play a card. Stop their player's card from being played and send it to the discard pile."

## Action Mapping
NEW: passive ability — interrupt mechanic; played out of turn to cancel another player's card play.

## Passive Interfaces
None

## Implementation Notes
Standard Neigh/counter effect. Handled by `NeighManager` (see CLAUDE.md "Neigh Interrupt Mechanic"). No unique C# class — rolled into `BasicNeighCardData` (`NeighType.Basic`) alongside Neigh, Bitch! and The Safeword is Neigh via `cardNameVariations`.
