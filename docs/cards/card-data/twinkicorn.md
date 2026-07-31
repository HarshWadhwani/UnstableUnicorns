# Twinkicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | TwinkicornCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, you may search the deck for a 'Bear Daddy Unicorn' card and add it to your hand, then shuffle the deck."

## Action Mapping
SearchDeckForCardAction { targetCardDataType = typeof(BearDaddyUnicornCardData) }

## Passive Interfaces
None

## Implementation Notes
Pairs with Bear Daddy Unicorn (which has the reverse effect). Ruling (per user): the search only looks at the play deck — if the target card is in the discard pile, a hand, or a stable, the effect finds nothing and silently does nothing (still shuffles).
