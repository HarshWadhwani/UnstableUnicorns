# Unexpected Miracle Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | UnexpectedMiracleUnicornCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, you may DISCARD a card, then Bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }
- BringFromNurseryAction {}

## Passive Interfaces
None

## Implementation Notes
Discard cost + bring a Baby Unicorn from the Nursery. Similar to Black Market Baby Unicorn but cheaper (1 discard vs 2) and IMMEDIATE trigger instead of EVERY_TURN — no `CanActivateEveryTurn`-equivalent gate exists for IMMEDIATE cards, so an empty Nursery means the discard still fires with no card gained in return (same accepted-risk precedent as Free Candy Unicorn / Bear Daddy Unicorn's "may" IMMEDIATE effects).
