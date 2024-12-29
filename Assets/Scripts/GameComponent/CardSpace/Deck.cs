using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : CardSpace
{
    public List<CardScriptableObject> cardScriptableObjects;

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

            AddCardToSpace(newCard);

            newCard.HideCard();
        }


        // Todo: remove this. deck cards should not be sorted
        spaceCards.Sort((card1, card2) => card1.name.CompareTo(card2.name));
    }

    Card GenerateNewCard(CardScriptableObject selectedCardData)
    {
        var newCardObject = Instantiate(cardPrefab);
        Card cardComponent = newCardObject.GetComponent<Card>();

        cardComponent.AssignCardData(selectedCardData);

        return cardComponent;
    }
}
