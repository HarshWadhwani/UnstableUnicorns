using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public List<CardScriptableObject> cardScriptableObjects;

    public GameObject cardPrefab;

    public List<GameObject> deckCards;

    public TurnManager turnManager;

    void Start()
    {
        LoadCardSOs();
        GenerateCardDeck();
    }

    void LoadCardSOs()
    {
        // Load all CardSO assets from the Resources folder
        cardScriptableObjects.AddRange(Resources.LoadAll<CardScriptableObject>("CardScriptableObjects"));
    }

    void GenerateCardDeck()
    {
        int numberOfCards = cardScriptableObjects.Select(cardSO => cardSO.instances).Sum();

        for (int i = 0; i < numberOfCards; i++)
        {
            CardScriptableObject selectedCardData = cardScriptableObjects[Random.Range(0, cardScriptableObjects.Count)];

            GameObject newCard = GenerateNewCard(cardPrefab, selectedCardData);

            PositionAndAppendCardDeck(newCard);
        }
    }

    GameObject GenerateNewCard(GameObject cardPrefab, CardScriptableObject selectedCardData)
    {
        var newCard = Instantiate(cardPrefab);
        Card cardComponent = newCard.GetComponent<Card>();

        cardComponent.AssignCardData(selectedCardData);
        cardComponent.HideCard();

        return newCard;
    }

    void PositionAndAppendCardDeck(GameObject card)
    {
        card.transform.SetParent(transform, false);
        deckCards.Add(card);
    }
}
