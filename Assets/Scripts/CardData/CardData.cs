using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public List<string> cardNameVariations;
    public string cardDescriptionText;
    public int instances;
    public CardType cardType;
    public SpecialActionType specialActionType;
    public AfterAction afterAction;

    public abstract void OnEnable();

    public abstract void TriggerSpecialAction();
}
