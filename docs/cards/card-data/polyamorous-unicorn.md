# Polyamorous Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | PolyamorousUnicornCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may move this card to another player's Stable, then STEAL a Unicorn card from that player's Stable."

## Action Mapping
- MoveSelfToOpponentStableAction {}
- StealUnicornAction {}

## Passive Interfaces
None
