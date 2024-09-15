using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighCardSO", menuName = "ScriptableObjects/DowngradeCardSO")]
public class DowngradeCardSO : CardSO
{
    public bool canPlayBeforeNextDrawPhase;
    public bool shouldPlayImmediately;
}
