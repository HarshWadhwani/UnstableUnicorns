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
    public CardData cardScriptableObject;
    public CardType cardType;
    public SpecialActionType specialActionType;
    public AfterAction afterAction;
    public CardSpace cardSpace;

    private TurnManager turnManager;

    void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }

    public void Initialize(CardData cardScriptableObject)
    {
        this.name = cardScriptableObject.cardNameVariations[0];
        SetCardName(cardScriptableObject.cardNameVariations[0]);
        SetCardDescription(cardScriptableObject.cardDescriptionText);
        this.cardType = cardScriptableObject.cardType;
        this.specialActionType = cardScriptableObject.specialActionType;
        this.afterAction = cardScriptableObject.afterAction;
        this.cardScriptableObject = cardScriptableObject;
    }

    private void SetCardName(string cardNameText)
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

    private void SetCardDescription(string cardDescriptionText)
    {
        if (cardDescription != null)
        {
            this.cardDescription.text = cardDescriptionText;
        }
        else
        {
            Debug.LogError("Card Description Text component is not assigned!");
        }
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
