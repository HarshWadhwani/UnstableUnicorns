using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DowngradeCardSO", menuName = "ScriptableObjects/DowngradeCardSO")]
public class DowngradeCardScriptableObject : CardScriptableObject
{
    public bool canPlayBeforeNextDrawPhase;
    public bool shouldPlayImmediately;

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }
}
