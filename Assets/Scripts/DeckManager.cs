using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    // List of all available card data (ScriptableObjects)
    public List<CardScriptableObject> cardScriptableObjects;

    // Reference to the card prefab
    public GameObject cardPrefab;

    public GameObject cardCanvas;

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

        Debug.Log(numberOfCards);

        for (int i = 0; i < numberOfCards; i++)
        {
            // Choose a random card from the cardDataList (or have logic to select specific cards)
            CardScriptableObject selectedCardData = cardScriptableObjects[Random.Range(0, cardScriptableObjects.Count)];

            // Instantiate the card prefab and assign its data
            GameObject newCard = Instantiate(cardPrefab, cardCanvas.transform);

            validateCard(newCard);

            // Set the card's data
            Card cardComponent = newCard.GetComponent<Card>();
            
            //cardComponent.SetCardName(selectedCardData.cardNameVariations[0]);
            //cardComponent.SetCardDescription(selectedCardData.cardDescriptionText);
            //cardComponent.cardType = selectedCardData.cardType;
            //cardComponent.specialActionType = selectedCardData.specialActionType;
            //cardComponent.afterAction = selectedCardData.afterAction;
            cardComponent.cardScriptableObject = selectedCardData;

            cardComponent.HideCard();

        }
    }

    private void validateCard(GameObject card)
    {

    }
}
