using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighCardSO", menuName = "ScriptableObjects/NeighCardSO")]
public class NeighCardSO : CardSO
{
    public bool shouldOpponentDiscard;
    public bool isFinal;
}
