# Black Market Baby Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | BlackMarketBabyUnicornCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may DISCARD 2 cards, then bring a Baby Unicorn card from the Nursery into your Stable."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=2 }
- BringFromNurseryAction {} — brings the next Baby Unicorn from Nursery into the active player's Unicorn stable.

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
activePlayer.handStable.spaceCards.Count >= 2 && DeckManager.Instance.nursery.spaceCards.Count > 0
```
Without this, a hand of 0-1 cards or an empty Nursery could still trigger a partial effect (discard succeeds/partially succeeds, bring-in fails silently, or vice versa) — same pattern as Bukkakecorn.

## Implementation Notes
Optional effect. `BringFromNurseryAction` always takes `nursery.spaceCards[0]` — the Nursery is homogeneous (all Baby Unicorns), so which specific card is irrelevant; it reveals the card and checks the win condition after moving it into the Unicorn stable.
