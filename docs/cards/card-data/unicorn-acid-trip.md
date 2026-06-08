# Unicorn Acid Trip

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
> "Each player (including you) must DISCARD their hand and DRAW the same number of cards they discarded. You may DRAW an additional 2 cards."

## Action Mapping
NEW: AllPlayersDiscardAndRedrawAction {} — all players discard hand and draw same count back. Active player also draws +2 additional cards.

## Passive Interfaces
None

## Implementation Notes
Complex multi-player effect. Requires tracking each player's hand size before the discard, then dealing that many cards back. Active player gets a +2 bonus draw on top.
