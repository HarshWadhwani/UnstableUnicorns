# Neigh Means Neigh

| Field | Value |
|-------|-------|
| wiki_type | Instant |
| card_type | NEIGH |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | done |
| impl_class | FinalNeighCardData.cs |

## Effect (2nd Edition)
> "Play this card when another player tries to play a card. Stop their card from being played and send it to the discard pile. This card cannot be Neigh'd."

## Action Mapping
NEW: passive ability — interrupt mechanic; played out of turn to cancel another player's card play. This counter cannot itself be countered.

## Passive Interfaces
None

## Implementation Notes
Enhanced Neigh with "cannot be Neigh'd" — immune to counter-countering. `FinalNeighCardData` (`NeighType.Final`); `NeighCardData.CanBeNeighed` returns `false` for this type, so `NeighManager.SubmitNeigh` ends the counter-chain the moment it's played (no further Pass/Neigh window offered). See CLAUDE.md "Neigh Interrupt Mechanic".
