using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCardSO", menuName = "ScriptableObjects/UpgradeCardSO")]
public class UpgradeCardScriptableObject : CardScriptableObject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }
}
