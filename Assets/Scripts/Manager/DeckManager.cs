using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Card cardPrefab;
    public CardData babyUnicornCardData;

    public Deck playDeck;  
    public Deck nursery;

    public TurnManager turnManager;
    public CardManager cardManager;

    private List<CardData> playCardDatas;
    
    void Start()
    {
        LoadAllCardData();
        SetupGameDecks();
        ShuffleDecks();
        // Call order is reversed by MoveToTop (each call bumps its card above the previous one),
        // so the LAST call here is drawn FIRST. Listed in call order (= reverse draw order).
        // New cards under active development go last so they're the first thing drawn.
        ForceFmkToTop();
        ForceBreakingAndEnteringToTop();
        ForceDumpsterDivingUnicornToTop();
        ForcePolyamorousUnicornToTop();
        ForceAutoeroticAsphyxiationToTop();
        ForceHorrifyingImpalingToTop(); // drawn 2nd — Hentaicorn below is drawn 1st and played first to test the shield
        ForceHentaicornToTop();         // drawn 1st unless bumped by a newer Force call below
        ForceBabyTrapToTop();           // needs a Baby Unicorn already played to a stable
        ForceFleshEatingUnicornToTop();
        ForceBearDaddyUnicornToTop();   // needs Twinkicorn also in the deck
        ForcePonyPlayToTop();           // a known Upgrade card kept in hand to test A Little Side Hustle's "bring in" branch
        ForceALittleSideHustleToTop();
        ForceBukkakecornToTop();        // needs a hand of 3+ cards (Discard 3) and an opponent unicorn (Steal) to fully exercise
        ForceBlazeAndGrazeToTop();      // Bukkakecorn (a Unicorn) is now the next card, so playing this reveals a Unicorn and exercises the "bring to Stable" branch; only 1 copy exists, so testing the "add to hand" (non-Unicorn) branch needs a separate Play session with different stacking
        ForceTheBitchiestUnicornToTop();   // drawn last among this batch — CanPlay requires a Basic Unicorn already in your Stable; play one in an earlier turn first
        ForceFuzzyHoofcuffsToTop();        // EVERY_TURN choice — needs a hand of 2+ cards and an opponent Unicorn to fully exercise discard-then-steal
        ForceHomicidalPsychocornToTop();   // EVERY_TURN choice — needs a hand of 1+ cards and an opponent Unicorn to fully exercise discard-then-destroy
        ForceFreeCandyUnicornToTop();      // needs a Baby Unicorn already in the opponent's Stable to see the steal fire (partial impl — no return-on-leave tracking yet)
        ForceSemenbiscuitToTop();          // needs a Downgrade card already in your own Stable to see the sacrifice fire
        ForceManscapedLlamacornToTop();    // no setup needed, opponent just needs a hand card to discard
        ForceDoubleAgentUnicornToTop();    // drawn last among this batch — needs a Basic Unicorn already in your own Stable to see the sacrifice fire
        ForceSadomasocornToTop();          // needs an opponent Unicorn to see the destroy fire
        ForcePuttingOnAShowToTop();        // sacrifice always has a target (the card itself sits in your Upgrade stable), opponent just needs any stable card to destroy
        ForceUnicornWithBenefitsToTop();   // drawn 1st (currently under test) — CanPlay requires a Basic Unicorn already in your Stable, and hand needs a 2nd Basic Unicorn to bring in

        foreach (var player in turnManager.players)
        {
            cardManager.DrawCard(nursery.spaceCards[0], nursery, player);
        }
    }

    // DEBUG: stack the play deck so the next draw is a Flesh-Eating Unicorn card.
    void ForceFleshEatingUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is FleshEatingUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceFleshEatingUnicornToTop: no FleshEatingUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Baby Trap card.
    void ForceBabyTrapToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is BabyTrapCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceBabyTrapToTop: no BabyTrapCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is an Autoerotic Asphyxiation card.
    void ForceAutoeroticAsphyxiationToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is AutoeroticAsphyxiationCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceAutoeroticAsphyxiationToTop: no AutoeroticAsphyxiationCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Polyamorous Unicorn card.
    void ForcePolyamorousUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is PolyamorousUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForcePolyamorousUnicornToTop: no PolyamorousUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Dumpster Diving Unicorn card.
    void ForceDumpsterDivingUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is DumpsterDivingUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceDumpsterDivingUnicornToTop: no DumpsterDivingUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Breaking and Entering card.
    void ForceBreakingAndEnteringToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is BreakingAndEnteringCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceBreakingAndEnteringToTop: no BreakingAndEnteringCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Hentaicorn card.
    void ForceHentaicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is HentaicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceHentaicornToTop: no HentaicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Horrifying Impaling card.
    void ForceHorrifyingImpalingToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is HorrifyingImpalingCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceHorrifyingImpalingToTop: no HorrifyingImpalingCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Fuck Marry Kill card.
    void ForceFmkToTop()
    {
        Card fmkCard = playDeck.spaceCards.Find(c => c.cardData is FuckMarryKillCardData);
        if (fmkCard == null)
        {
            Debug.LogWarning("ForceFmkToTop: no FuckMarryKillCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(fmkCard);
    }

    // DEBUG: stack the play deck so the next draw is a Bear Daddy Unicorn card.
    void ForceBearDaddyUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is BearDaddyUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceBearDaddyUnicornToTop: no BearDaddyUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is an A Little Side Hustle card.
    void ForceALittleSideHustleToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is ALittleSideHustleCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceALittleSideHustleToTop: no ALittleSideHustleCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Pony Play card.
    void ForcePonyPlayToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is PonyPlayCardData);
        if (card == null)
        {
            Debug.LogWarning("ForcePonyPlayToTop: no PonyPlayCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Bukkakecorn card.
    void ForceBukkakecornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is BukkakecornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceBukkakecornToTop: no BukkakecornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Blaze and Graze card.
    void ForceBlazeAndGrazeToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is BlazeAndGrazeCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceBlazeAndGrazeToTop: no BlazeAndGrazeCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Manscaped Llamacorn card.
    void ForceManscapedLlamacornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is ManscapedLlamacornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceManscapedLlamacornToTop: no ManscapedLlamacornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a The Bitchiest Unicorn card.
    void ForceTheBitchiestUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is TheBitchiestUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceTheBitchiestUnicornToTop: no TheBitchiestUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Semenbiscuit card.
    void ForceSemenbiscuitToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is SemenbiscuitCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceSemenbiscuitToTop: no SemenbiscuitCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Homicidal Psychocorn card.
    void ForceHomicidalPsychocornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is HomicidalPsychocornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceHomicidalPsychocornToTop: no HomicidalPsychocornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Fuzzy Hoofcuffs card.
    void ForceFuzzyHoofcuffsToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is FuzzyHoofcuffsCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceFuzzyHoofcuffsToTop: no FuzzyHoofcuffsCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Free Candy Unicorn card.
    void ForceFreeCandyUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is FreeCandyUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceFreeCandyUnicornToTop: no FreeCandyUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Double Agent Unicorn card.
    void ForceDoubleAgentUnicornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is DoubleAgentUnicornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceDoubleAgentUnicornToTop: no DoubleAgentUnicornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Sadomasocorn card.
    void ForceSadomasocornToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is SadomasocornCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceSadomasocornToTop: no SadomasocornCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Putting on a Show card.
    void ForcePuttingOnAShowToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is PuttingOnAShowCardData);
        if (card == null)
        {
            Debug.LogWarning("ForcePuttingOnAShowToTop: no PuttingOnAShowCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    // DEBUG: stack the play deck so the next draw is a Unicorn with Benefits card.
    void ForceUnicornWithBenefitsToTop()
    {
        Card card = playDeck.spaceCards.Find(c => c.cardData is UnicornWithBenefitsCardData);
        if (card == null)
        {
            Debug.LogWarning("ForceUnicornWithBenefitsToTop: no UnicornWithBenefitsCardData found in play deck.");
            return;
        }
        playDeck.MoveToTop(card);
    }

    void LoadAllCardData()
    {
        CardData[] allCardData = Resources.LoadAll<CardData>("CardDataInstances");
        
        playCardDatas = new List<CardData>();
        
        foreach (var cardData in allCardData)
        {
            if (cardData != babyUnicornCardData)
            {
                playCardDatas.Add(cardData);
            }
        }
        
        Debug.Log($"Loaded {playCardDatas.Count} card data instances for play deck");
    }

    void SetupGameDecks()
    {
        GeneratePlayDeckCards();
        GenerateNurseryCards();
    }

    void GeneratePlayDeckCards()
    {
        foreach (var playCardData in playCardDatas)
        {
            SetupDeck(playDeck, playCardData);
        }
    }

    void GenerateNurseryCards()
    {
        SetupDeck(nursery, babyUnicornCardData);
    }

    void SetupDeck(Deck deck, CardData cardData)
    {
        List<Card> cards = GenerateCards(cardData);

        foreach (var card in cards)
        {
            card.HideCard();
            deck.AddCard(card);
        }
    }

    List<Card> GenerateCards(CardData cardData)
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < cardData.instances; i++)
        {
            Card newCard = Instantiate(cardPrefab); // Instantiate card from prefab
            newCard.Initialize(cardData);               // Call a void method for the card
            cards.Add(newCard);                      // Add the card to the list
        }

        return cards;
    }

    void ShuffleDecks()
    {
        ShuffleDeck(playDeck);
        ShuffleDeck(nursery);
    }

    public void ShuffleDeck(Deck deck)
    {
        for (int i = deck.spaceCards.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (deck.spaceCards[i], deck.spaceCards[j]) = (deck.spaceCards[j], deck.spaceCards[i]);
        }

        for (int i = 0; i < deck.spaceCards.Count; i++)
            deck.spaceCards[i].transform.SetSiblingIndex(i);
    }
}