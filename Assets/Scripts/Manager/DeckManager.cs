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
}