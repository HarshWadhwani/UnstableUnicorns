# Unicorn Enema

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | active player has at least 1 Downgrade card in their Stable |
| impl_status | done |
| impl_class | UnicornEnemaCardData.cs |

## Effect (2nd Edition)
> "SACRIFICE all Downgrade cards in your Stable."

## Action Mapping
- SacrificeCardAction { targetStable=Downgrade, sacrificeAll=true }

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.downgradeStable.Cards.Count > 0
```
