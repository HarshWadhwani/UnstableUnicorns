# Neigh, Motherfucker!

| Field | Value |
|-------|-------|
| wiki_type | Instant |
| card_type | NEIGH |
| copies | 3 |
| trigger | NONE |
| can_play | always |
| impl_status | done |
| impl_class | DiscardNeighCardData.cs |

## Effect (2nd Edition)
> "Play this card when another player tries to play a card. Stop their player's card from being played and send it to the discard pile. That player must DISCARD a card."

## Action Mapping
NEW: passive ability — interrupt mechanic; played out of turn to cancel another player's card play and force them to discard 1.

## Passive Interfaces
None

## Implementation Notes
Enhanced Neigh: also forces the owner of the card it cancels to discard 1. `DiscardNeighCardData` (`NeighType.ForceOpponentDiscard`). `NeighManager.ResolveContest` enqueues a `DiscardCardAction` (numberOfCards 1, PlayerChooses) against that owner for every *live* copy of this in the chain, run before the contested card resolves/discards. See CLAUDE.md "Neigh Interrupt Mechanic".
