using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DeckManager : MonoBehaviour
{
    public Card cardPrefab; // Assign your card prefab in the Inspector
    public Deck playDeck;  // The Deck GameObject where cards will be stored
    public Deck nursery;
    public List<CardData> playCardDatas; // List of ScriptableObjects containing card data
    public CardData babyUnicornCardData;

    void Start()
    {
        SetupGameDecks();
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

        cards.ForEach(card => HideCard(card));

        deck.AddCards(cards);
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

    void HideCard(Card card)
    {
        card.HideCard();
    }
}