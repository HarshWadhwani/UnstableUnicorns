using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Card cardPrefab; // Assign your card prefab in the Inspector
    public PlayDeck playDeck;  // The Deck GameObject where cards will be stored
    public List<CardData> cardDataList; // List of ScriptableObjects containing card data

    void Start()
    {
        SetupDeck();
    }

    void SetupDeck()
    {
        foreach (CardData data in cardDataList)
        {
            Card newCard = Instantiate(cardPrefab); // Your Card script
            newCard.Initialize(data); // Apply the data

            playDeck.AddCard(newCard);
        }
    }
}