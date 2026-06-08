# Rainbow Shitstorm

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
> "Each player (including you) must SACRIFICE a card and DISCARD their hand. Shuffle the discard pile into the deck, then deal 5 cards to each player."

## Action Mapping
NEW: Full reset action — all players sacrifice 1 card + discard hands, shuffle discard pile back into deck, deal 5 to each player. No existing action type covers this.

## Passive Interfaces
None

## Implementation Notes
Highly complex multi-step effect affecting all players. Requires: AllPlayersSacrificeAction, AllPlayersDiscardHandAction, ShuffleDiscardIntoDeckAction, DealCardsToAllPlayersAction. Significant new infrastructure needed.
