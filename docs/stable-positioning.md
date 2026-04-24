# Stable Card Positioning

How cards are laid out inside `UnicornStable`, and the base `Stable` class that drives it.

---

## Layout Model

Each stable divides its total width into equal slots — one slot per `maxCardsInStable`. Cards are centered inside their slot. The formula:

```
cardSlotWidth = stableWidth / maxCardsInStable
startX        = stableCenter - stableWidth/2 + cardSlotWidth/2   ← center of slot 0
position[i]   = startX + i * cardSlotWidth
```

For a 400px wide stable with `maxCardsInStable = 4`:

```
cardSlotWidth = 100px
startX        = -150px   (center of leftmost slot)

slot 0 → -150px
slot 1 →  -50px
slot 2 →   50px
slot 3 →  150px
```

Cards are placed at `spaceCards[i]` → `position[i]`. Only cards currently in the stable are placed — empty slots are just empty space.

---

## What Was Fixed (B2)

The original implementation used a mutable accumulator (`leftMostOpenPosition`) that was conditionally incremented inside the loop:

```csharp
// Old — accumulator pattern
float leftMostOpenPosition = startX;
for (int i = 0; i < spaceCards.Count; i++)
{
    if (spaceCards.Count > 1 && i > 0)
        leftMostOpenPosition += cardSlotWidth;  // incremented BEFORE placement
    cardRect.anchoredPosition = new Vector2(leftMostOpenPosition, 0);
}
```

This worked correctly at runtime but was fragile: the position of card `i` depended on the accumulated state from cards `0..i-1`. Any future refactor that reordered or skipped iterations could silently break placement. The condition `spaceCards.Count > 1 && i > 0` was also redundant — it was always equivalent to just `i > 0`.

Replaced with a direct index formula:

```csharp
// New — direct formula
for (int i = 0; i < spaceCards.Count; i++)
    cardRect.anchoredPosition = new Vector2(startX + i * cardSlotWidth, 0);
```

Each card's position is now derived independently from its index. No mutation, no ordering dependency.

---

## Subclass Overrides

`Stable.PositionCardsInStable` is `virtual`. Subclasses override it for different visual layouts:

- **`HandStable`** — fan layout with rotation and Y-arc offsets (see `HandStable.cs`)
- **`UpgradeStable`** — overlapping stack anchored to right edge
- **`DowngradeStable`** — overlapping stack anchored to left edge

`PositionCardsInStable` is called in two places: `Stable.AddCard` (when a card enters) and manually in `HandleCardClick` after a pending action removes a card. If you add a new removal path, make sure to call it there too.

---

## Testing

To verify slot positions visually without Play mode, add `OnDrawGizmos` to `Stable.cs`:

```csharp
void OnDrawGizmos()
{
    RectTransform stableRect = GetComponent<RectTransform>();
    if (stableRect == null || maxCardsInStable == 0) return;

    float cardSlotWidth = stableRect.rect.width / maxCardsInStable;
    float startX = stableRect.anchoredPosition.x - (stableRect.rect.width / 2) + cardSlotWidth / 2;

    Gizmos.color = Color.cyan;
    for (int i = 0; i < maxCardsInStable; i++)
    {
        Vector3 slotCenter = transform.TransformPoint(new Vector3(startX + i * cardSlotWidth, 0, 0));
        Gizmos.DrawWireSphere(slotCenter, 10f);
    }
}
```

Key regression test: play exactly 2 unicorns and confirm they occupy slot 0 and slot 1, not both shifted right (that was the visible symptom of the old accumulator bug with a single-card stable being reused).
