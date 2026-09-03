# Moist Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | MoistUnicornCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, you may search the deck for a Unicorn card and add it to your hand, then shuffle the deck."

## Action Mapping
- SearchDeckForTypeAction { targetCardType=UNICORN }

## Passive Interfaces
None

## Implementation Notes
Deck search by `CardType` rather than a specific `CardData` subclass — `SearchDeckForTypeAction` mirrors the existing `SearchDeckForCardAction` (used by Bear Daddy Unicorn / Twinkicorn) but matches on `c.cardData.cardType == targetCardType` instead of an exact type check. Finds the first matching card, adds it to hand, shuffles the deck; skips silently (but still shuffles) if no Unicorn card is in the deck.
