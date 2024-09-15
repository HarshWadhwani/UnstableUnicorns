using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    // List of all available card data (ScriptableObjects)
    public List<CardSO> cardSOs;

    // Reference to the card prefab
    public GameObject cardPrefab;

    void Start()
    {
        LoadCardSOs();
        GenerateCardDeck();
    }

    void LoadCardSOs()
    {
        // Load all CardSO assets from the Resources folder
        cardSOs.AddRange(Resources.LoadAll<CardSO>("CardSOs"));
    }

    void GenerateCardDeck()
    {
        int numberOfCards = cardSOs.Select(cardSO => cardSO.instances).Sum();

        for (int i = 0; i < numberOfCards; i++)
        {
            // Choose a random card from the cardDataList (or have logic to select specific cards)
            CardSO selectedCardData = cardSOs[Random.Range(0, cardSOs.Count)];

            // Instantiate the card prefab and assign its data
            GameObject newCard = Instantiate(cardPrefab, transform);

            validateCard(newCard);

            // Set the card's data
            Card cardComponent = newCard.GetComponent<Card>();
            
            cardComponent.SetCardName(selectedCardData.cardNameVariations[0]);
            //cardComponent.SetCardType(selectedCardData.cardType);
            cardComponent.SetCardDescription(selectedCardData.cardDescriptionText);

            cardComponent.HideCard();
        }
    }

    private void validateCard(GameObject card)
    {

    }
}
