Read `docs/cards/card-implementation-guide.md` first.

Then implement the following card: $ARGUMENTS

Work through the guide's decisions in order:
1. Determine CardType from the description
2. Note the SpecialActionType (set automatically by subclass — confirm it matches the effect timing)
3. Map each step of the effect to a CardAction in sequence
4. Decide whether a new C# subclass is needed or an existing base class suffices
5. Determine `instances`

If a new C# class is needed, write it now following the template in the guide.

Once the card's C# class exists, add a debug stacking method to `DeckManager.cs`:
1. Add a private `Force<CardNameNoSpaces>ToTop()` method following the same pattern as `ForceFmkToTop()` and `ForceBreakingAndEnteringToTop()` — find the card in `playDeck.spaceCards` by its CardData type, call `playDeck.MoveToTop(card)`.
2. Call it from `DeckManager.Start()` after the existing Force* calls.

After writing the code, tell the user:
- What `.asset` to create in Unity, where to save it, and what fields to fill in
- Any effect steps from the description that could not be mapped to an existing CardAction (these need a new action type)
- A short testing checklist for the card's effect in Play mode
