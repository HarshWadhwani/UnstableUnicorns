# A Little Side Hustle

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may bring an Upgrade card from your hand into your Stable."

## Action Mapping
NEW: PlayCardFromHandToStableAction { cardType=UPGRADE } — active player may play an Upgrade card from hand directly into their stable (bypassing the normal Action phase play).

## Passive Interfaces
None

## Implementation Notes
Optional. Allows playing an extra Upgrade from hand at the start of turn, separate from the Action phase. Requires an action to play a specific card type from hand to stable.
