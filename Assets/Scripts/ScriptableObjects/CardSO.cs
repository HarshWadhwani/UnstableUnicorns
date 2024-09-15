using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSO : ScriptableObject
{
    public List<string> cardNameVariations;
    public CardType cardType;
    public string cardDescriptionText;
    public int instances;
}
