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

    public CardSpace cardSpace;

    public void Initialize(CardData cardData)
    {
        string cardDisplayName = cardData.NextCardName();
        name = cardDisplayName;
        cardName.text = cardDisplayName;
        cardDescription.text = cardData.cardDescriptionText;
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
