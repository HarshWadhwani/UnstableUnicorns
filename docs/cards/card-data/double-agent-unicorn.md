# Double Agent Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a Basic Unicorn card, then pull a card at random from each other player's hand."

## Action Mapping
- SacrificeCardAction { targetStable=Unicorn, sacrificeAll=false } — player chooses a Basic Unicorn (would need targetSubtype=Basic)
- PullCardAction { numberOfCards=1, skipDrawPhaseOnSuccess=false } — pull 1 card from opponent (per player in multi-player)

## Passive Interfaces
None

## Implementation Notes
Optional. The sacrifice is specifically a Basic Unicorn (not any Unicorn). In 2-player this is effectively sacrifice 1 Basic Unicorn + pull 1 random from opponent. Multi-player would need to pull from each player.
