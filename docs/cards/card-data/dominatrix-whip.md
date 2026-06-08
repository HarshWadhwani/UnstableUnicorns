# Dominatrix Whip

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may move a Unicorn card from any player's Stable to any other player's Stable. You cannot move that card to your own Stable."

## Action Mapping
NEW: MoveUnicornBetweenStablesAction { excludeSelf=true } — active player picks a unicorn from any stable and moves it to any other player's stable (not own stable).

## Passive Interfaces
None

## Implementation Notes
Optional. Active player is a broker — moves any unicorn between other players. Cannot move to own stable. In 2-player, effectively moves opponent's unicorn to... still opponent's stable (since only two players), making this largely a no-op in 2-player. Only meaningful in 3+ players.
