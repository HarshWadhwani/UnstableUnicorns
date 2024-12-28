using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : CardSpace
{
    public List<CardScriptableObject> cardScriptableObjects;

    public GameObject cardPrefab;

    public List<GameObject> deckCards;

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

    public override void HandleCardClick(Card card)
    {
        turnManager.MoveCardBetweenDecks(card);
        RemoveCardFromCurrentStable(card);
    }

    void GenerateCardDeck()
    {
        int numberOfCards = cardScriptableObjects.Select(cardSO => cardSO.instances).Sum();

        for (int i = 0; i < numberOfCards; i++)
        {
            CardScriptableObject selectedCardData = cardScriptableObjects[Random.Range(0, cardScriptableObjects.Count)];

            Card newCard = GenerateNewCard(selectedCardData);

            this.AddCardToSpace(newCard);

            newCard.HideCard();
        }
    }

    Card GenerateNewCard(CardScriptableObject selectedCardData)
    {
        var newCardObject = Instantiate(cardPrefab);
        Card cardComponent = newCardObject.GetComponent<Card>();

        cardComponent.AssignCardData(selectedCardData);

        return cardComponent;
    }
}
