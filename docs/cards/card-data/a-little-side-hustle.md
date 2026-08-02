# A Little Side Hustle

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | ALittleSideHustleCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may bring an Upgrade card from your hand into your Stable."

## Action Mapping
PlayCardFromHandAction { cardType = CardType.UPGRADE } — prompts the active player to click an Upgrade card in their own hand; routes the click through `CardManager.PlayCardForCurrentPlayer` (respects that card's own `CanPlay`). Skips silently if the hand has no Upgrade cards.

## Passive Interfaces
None

## Implementation Notes
Optional. Allows playing an extra Upgrade from hand at the start of turn, separate from the Action phase. The EVERY_TURN choice queue is snapshotted once at the start of the turn, so a card brought in this way does not retroactively join this turn's queue even if it's itself an EVERY_TURN Upgrade — it becomes eligible starting next turn. Upgrade stable has no card cap (`maxCardsInStable: 0` = unlimited), so bringing a card in can't silently fail/lose the card.
