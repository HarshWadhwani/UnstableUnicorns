# Double Agent Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | DoubleAgentUnicornCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a Basic Unicorn card, then pull a card at random from each other player's hand."

## Action Mapping
- SacrificeCardAction { targetStable=Unicorn, sacrificeAll=false, targetSubtype=BASIC }
- PullCardAction { numberOfCards=1, skipDrawPhaseOnSuccess=false } — pull 1 card from opponent (per player in multi-player)

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC)
```
Without this, a player with no Basic Unicorn in their stable could still trigger the card: `SacrificeCardAction` would silently skip (no eligible target), but `PullCardAction` would still fire — a free pull with no cost paid.

## Implementation Notes
Optional. The sacrifice is specifically a Basic Unicorn (not any Unicorn) — implemented via `SacrificeCardAction.targetSubtype`, same pattern as `StealUnicornAction.targetSubtype`. In 2-player this is effectively sacrifice 1 Basic Unicorn + pull 1 random from opponent. Multi-player would need to pull from each player.
