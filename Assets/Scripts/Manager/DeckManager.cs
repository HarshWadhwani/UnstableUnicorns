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
        ForceFmkToTop();
        ForceBreakingAndEnteringToTop();
        ForceDumpsterDivingUnicornToTop();
        ForcePolyamorousUnicornToTop();
        ForceHorrifyingImpalingToTop();

        foreach (var player in turnManager.players)
        {
            cardManager.DrawCard(nursery.spaceCards[0], nursery, player);
        }
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