using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighCardSO", menuName = "ScriptableObjects/NeighCardSO")]
public abstract class NeighCardScriptableObject : CardScriptableObject
{
    public NeighType neighType;

    private void OnEnable()
    {
        cardType = CardType.Neigh;
        specialActionType = SpecialActionType.Immediate;
        afterAction = AfterAction.Discard;
    }
}
