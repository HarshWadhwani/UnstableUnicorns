Read `docs/cards/card-implementation-guide.md` first.

## Step 1 — Look up pre-extracted card data

Check `docs/cards/card-data/_checklist.md` for the card named in $ARGUMENTS.

If a matching row exists (Extracted ✅), read its card data file (e.g. `docs/cards/card-data/hentaicorn.md`).
The file contains: canonical effect text, copy count, trigger type, action mapping, CanPlay conditions, CanActivateEveryTurn conditions (for EVERY_TURN choice cards whose effect could otherwise run partially), passive interfaces, and rulings.
Use this as your primary source — it saves re-deriving everything from scratch and ensures 2nd Edition wording is used.

If the card is not in the checklist, derive everything from the description in $ARGUMENTS as usual.

## Step 2 — Implement the card

Work through the guide's decisions in order:
1. Determine CardType (from the data file's `card_type` field, or from the description)
2. Note the SpecialActionType — confirm the `trigger` field matches the effect timing
3. Map each effect step to a CardAction — the data file's **Action Mapping** section has this pre-done; use it directly
4. Check the **Passive Interfaces** section — implement any listed interface (e.g. `ISacrificeShield`)
5. Check the **CanPlay Override** section — add `CanPlay()` if listed
6. Check the **CanActivateEveryTurn Override** section — add `CanActivateEveryTurn()` if listed. Only needed for EVERY_TURN choice cards (Unicorn/Upgrade) with a multi-step effect where an early step failing shouldn't let a later step still run (e.g. Bukkakecorn: discard 3 then steal — a hand of 1-2 cards shouldn't discard-then-nothing). Most cards don't need this; per-action self-skipping (like `DiscardCardAction` on an empty hand) is the default and is usually enough.
7. Decide whether a new C# subclass is needed or an existing base class suffices
8. Set `instances` from the data file's `copies` field

If a new C# class is needed, write it following the template in the guide.

If the Action Mapping contains `NEW: ...` entries, those action types don't exist yet — flag them to the user before writing any code and do not proceed until the user decides how to handle them.

## Step 3 — Create the Unity asset directly

Don't ask the user to create the `.asset` in the Inspector — author it yourself. `.asset`/`.meta` files are plain YAML text; Unity accepts externally-authored GUIDs the same way it accepts its own. This also permanently closes the "Known recurring gotcha" below, since you set `cardNameVariations`/`instances` correctly by construction instead of leaving Inspector defaults or picking up the wrong text (e.g. an accidental "Card Data" suffix from copy-pasting the asset name).

1. **Get the script's GUID.** Check whether `<CardName>CardData.cs.meta` already exists (Unity may have auto-generated it already if the Editor happened to be open and focused). If it exists, read the `guid:` line from it. If not, generate one yourself and write the file:
   ```
   fileFormatVersion: 2
   guid: <32 lowercase hex chars, e.g. via `python3 -c "import secrets; print(secrets.token_hex(16))"`>
   ```
   Verify it's actually unique before writing: `grep -rl "<guid>" --include="*.meta" Assets/` should return nothing.

2. **Write the `.asset`** at `Assets/Resources/CardDataInstances/<CardType>CardDataInstances/<Card Name> Card Data.asset`. Read an existing sibling asset of the same CardType first (e.g. another Unicorn asset) to match its exact field set — Unicorn assets have a trailing `unicornType` field that Magic/Upgrade/Downgrade/Neigh assets don't:
   ```yaml
   %YAML 1.1
   %TAG !u! tag:unity3d.com,2011:
   --- !u!114 &11400000
   MonoBehaviour:
     m_ObjectHideFlags: 0
     m_CorrespondingSourceObject: {fileID: 0}
     m_PrefabInstance: {fileID: 0}
     m_PrefabAsset: {fileID: 0}
     m_GameObject: {fileID: 0}
     m_Enabled: 1
     m_EditorHideFlags: 0
     m_Script: {fileID: 11500000, guid: <script guid from step 1>, type: 3}
     m_Name: <Card Name> Card Data
     m_EditorClassIdentifier: Assembly-CSharp::<CardNameNoSpaces>CardData
     cardNameVariations:
     - <Card Name, exact display text — no "Card"/"Card Data" suffix>
     cardDescriptionText:
     instances: <copies from the card data file>
     cardType: <int — UNICORN=0, MAGIC=1, UPGRADE=2, DOWNGRADE=3, NEIGH=4>
     specialActionType: <int — IMMEDIATE=0, EVERY_TURN=1, NONE=2>
     unicornType: <int — BASIC=0, MAGIC=1, BABY=2, SPECIAL=3 — Unicorn subclasses only>
   ```
   `cardDescriptionText` is conventionally left blank across every existing card (not filled in from the effect text) — match that.

3. **Write the `.asset.meta`** next to it, with a second freshly generated + uniqueness-checked GUID:
   ```
   fileFormatVersion: 2
   guid: <different 32-char guid>
   NativeFormatImporter:
     externalObjects: {}
     mainObjectFileID: 11400000
     userData:
     assetBundleName:
     assetBundleVariant:
   ```

4. Sanity-check both new files by reading them back before moving on.

## Step 4 — Add debug stacking to DeckManager

Once the card's C# class exists, add a debug stacking method to `DeckManager.cs`:
1. Add a private `Force<CardNameNoSpaces>ToTop()` method — find the card in `playDeck.spaceCards` by its CardData type, call `playDeck.MoveToTop(card)`.
2. **Call ordering matters.** `Deck.MoveToTop` appends the card to the end of `spaceCards`, and each subsequent call pushes its own card above the one before it. This means the Force calls in `DeckManager.Start()` are listed in *reverse draw order* — the LAST call in the list is the FIRST card drawn. Add the new card's Force call as the **last line** of the Force-call block (immediately before the baby-unicorn `foreach` draw loop) so it's the very next card drawn — that's almost always what you want when testing a card you just added.
3. **Don't break existing ordering dependencies.** A couple of Force calls are intentionally sequenced relative to each other and have inline comments saying so (e.g. Hentaicorn must be drawn and played to a stable *before* the opponent draws Horrifying Impaling, to test the sacrifice-shield interaction). If you're inserting your new call among calls that have such a comment, insert it *before* that dependent group rather than splitting it apart, and update the "drawn 1st" wording in the comment since it will no longer be literally first.
4. If the card's `CanPlay` needs board state that doesn't exist at game start (e.g. a card already sitting in a stable), still put the Force call at the top — leave a comment noting what manual setup is needed first. Being immediately drawable is more useful for testing than being buried deep in the deck, and verifying the `CanPlay` guard itself (does it correctly block the card before setup?) is part of testing too.

## Step 5 — Update the card data file and checklist

After the code is written:
1. In the card's data file (e.g. `docs/cards/card-data/hentaicorn.md`), set:
   - `impl_status` → `done`
   - `impl_class` → the C# filename (e.g. `HentaicornCardData.cs`)
2. In `docs/cards/card-data/_checklist.md`, change the card's **Implemented** column from ⬜ to ✅ and update the Summary count.

## Step 6 — Tell the user

- Confirmation that the `.asset` is already created (with its path) — no Inspector work needed. Ask the user to open/focus the Unity Editor once so it imports the new files, and flag if this is the first time in the session (so they know to check for import errors before testing).
- Any `NEW:` action types that blocked implementation (if any)
- A short testing checklist for the card's effect in Play mode

**Known recurring gotcha (historical, now closed by Step 3):** on nearly every card added before Claude started authoring assets directly, the `.asset` got created with `cardNameVariations: []` and `instances: 0` left at their defaults in the Inspector — usually because the list-add wasn't committed before saving. `instances: 0` means `DeckManager.GenerateCards` creates zero copies of the card, so it silently never enters the deck at all (including for the Force-to-top debug hook — it'll just log a "not found" warning). If a `.asset` was ever created by hand instead of via Step 3 (e.g. the user made it directly), read it and check both fields before treating the card as ready to test.
