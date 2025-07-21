using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardData cardData;

    public GameObject cardFront;
    public GameObject cardBack;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    
    public CardType cardType;
    public SpecialActionType specialActionType;
    public AfterAction afterAction;

    public CardSpace cardSpace;

    public void Initialize(CardData cardData)
    {
        name = cardData.cardNameVariations[0];
        cardName.text = cardData.cardNameVariations[0];
        cardDescription.text = cardData.cardDescriptionText;
        cardType = cardData.cardType;
        specialActionType = cardData.specialActionType;
        afterAction = cardData.afterAction;
        this.cardData = cardData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cardSpace.HandleCardClick(this);
    }

    public void HideCard()
    {
        cardFront.SetActive(false);
        cardBack.SetActive(true);
    }

    public void RevealCard()
    {
        cardFront.SetActive(true);
        cardBack.SetActive(false);
    }

}
