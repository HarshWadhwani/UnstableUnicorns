using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DeckManager : MonoBehaviour
{
    public Card cardPrefab;
    public List<CardData> playCardDatas;
    public CardData babyUnicornCardData;

    public Deck playDeck;  
    public Deck nursery;

    public TurnManager turnManager;
    public CardManager cardManager;
    
    void Start()
    {
        SetupGameDecks();
        Debug.Log("Before shuffle: " + string.Join(",", playDeck.spaceCards.Select(c => c.name)));
        ShuffleDecks();
        Debug.Log("After shuffle: " + string.Join(",", playDeck.spaceCards.Select(c => c.name)));

        foreach (var player in turnManager.players)
        {
            cardManager.DrawCard(nursery.spaceCards[0], nursery, player);
        }
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
        // Step 1: Put all children into a list
        List<Transform> children = new List<Transform>();
        foreach (Transform child in deck.transform)
            children.Add(child);

        // Step 2: Shuffle the list
        for (int i = children.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (children[i], children[j]) = (children[j], children[i]);
        }

        // Step 3: Reassign sibling indices
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }

        // Debug: log shuffled order
        Debug.Log("Shuffled deck: " + string.Join(", ", children.ConvertAll(c => c.name)));
    }
}