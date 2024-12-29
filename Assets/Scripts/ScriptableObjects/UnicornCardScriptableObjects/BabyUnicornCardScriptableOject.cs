using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BabyUnicornCardScriptableObject", menuName = "ScriptableObjects/UnicornCards/BabyUnicornCard")]
public class BabyUnicornCardScriptableOject : UnicornCardScriptableOject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("No Special Action for BabyUnicornCardScriptableOject");
    }
}
