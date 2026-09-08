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

    // Set only for cards implementing IReturnsStolenCardOnLeave, and only when the stolen card
    // is itself a Baby Unicorn (see CardActionExecutor.ExecutePendingAction) — never populated
    // for non-Baby-Unicorn steals, so this mechanism can't affect unrelated cards.
    public Card linkedBabyUnicorn;
    public CardSpace linkedBabyUnicornOriginStable;

    public void Initialize(CardData cardData)
    {
        string cardDisplayName = cardData.NextCardName();
        name = cardDisplayName;
        cardName.text = cardDisplayName;
        cardDescription.text = cardData.cardDescriptionText;
        this.cardData = cardData;

        GetComponent<CardVisuals>()?.Apply(cardData);
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
