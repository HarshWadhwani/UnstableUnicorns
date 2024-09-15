using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public GameObject cardFront;
    public GameObject cardBack;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;

    public void SetCardName(string cardNameText)
    {
        if (cardName != null)
        {
            cardName.text = cardNameText;
        }
        else
        {
            Debug.LogError("Card Name Text component is not assigned!");
        }
    }

    public void SetCardDescription(string cardDescriptionText)
    {
        if (cardDescription != null)
        {
            cardDescription.text = cardDescriptionText;
        }
        else
        {
            Debug.LogError("Card Description Text component is not assigned!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RevealCard();
        Debug.Log("Card clicked: " + cardName.text);
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
