using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicCardSO", menuName = "ScriptableObjects/MagicCardSO")]
public class MagicCardScriptableObject : CardScriptableObject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }
}
